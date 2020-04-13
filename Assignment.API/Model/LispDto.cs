using Microsoft.AspNetCore.Http;

namespace Assignment.API
{
    public class LispDto
    {
        public string FilePath { get; set; }
        public IFormFile FileToUpload { get; set; }
    }
}
