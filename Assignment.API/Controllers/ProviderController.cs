using Assignment.API.Extensions;
using Assignment.API.Helpers;
using Assignment.API.Model;
using Assignment.Services.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Assignment.API.Controllers
{
    
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;
        private readonly IWebHostEnvironment _env;

        public ProviderController(IProviderService providerService, IWebHostEnvironment env)
        {
            _providerService = providerService;
            _env = env;
        }

        [Route("api/provider/create")]
        [HttpPost]
        public async Task<IActionResult> CreateProvider(ProviderDto model)
        {
            if (model == null)
                return BadRequest();
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var providerEntity = model.ToEntityModel();
            if (providerEntity != null)
            {
                var provider = await _providerService.CreateProvider(providerEntity);                
                return CreatedAtRoute("GetProviderById", new { id = provider.ProviderId }, provider.ToDto());
            }
            return BadRequest("Something went wrong while registering the provider. Please call customer service.");
        }        

        [Route("{id}",Name="GetProviderById")]
        [HttpGet]
        public async Task<IActionResult> GetAllProviders(int id)
        {
            var provider = await _providerService.GetById(id);
            return Ok(provider);

        }

        [Route("api/register/states")]
        [HttpGet]
        public async Task<IActionResult> GetAllStates()
        {
            var states = await _providerService.GetAllStates();
            if(states == null || states.Count == 0)
            {
                //This is a one time trick to insert states into dbo.States table by loading  the states form wwwroot/usa_states.csv file
                var allStates = FileHelper.GetAllStatesFromFile(_env.WebRootPath + "\\" + "usa_states.csv");
                await _providerService.CreateAllStates(allStates);
                states = await _providerService.GetAllStates();
            }
            
            return Ok(states);
        }

        [Route("api/lisp/validate")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            FileUploadDto dto = new FileUploadDto();
            var file = Request.Form.Files[0];
            dto.FileContent = file;
            if (dto.FileContent == null)
                return Content("file not selected");
            var response = new FileUpoloadResponseDto();
            var fileContents = await dto.FileContent.ReadAsList();
            response.IsValid = fileContents.IsWellFormedParenthesis();
            return Ok(response);
        }

        [Route("api/enrollment/burst")]
        [HttpPost]
        public async Task<IActionResult> BurstEnrollmentFile()
        {
            FileUploadDto dto = new FileUploadDto();  
            var file = Request.Form.Files[0];
            dto.FileContent = file;            
            if (dto.FileContent == null)
                return Content("file not selected");            
            var uploadedFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            dto.PathToBurstedFile = _env.WebRootPath;
            var burstedFileList = await FileHelper.BurstEnrollmentFile(dto);
            var uploadResponse = new FileUpoloadResponseDto { UploadedFileName = uploadedFileName, BursetedFiles = burstedFileList };
            return Ok(uploadResponse);
        }
    }
}