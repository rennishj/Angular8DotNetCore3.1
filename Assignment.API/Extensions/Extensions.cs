using Assignment.Model.Entity;
using Assignment.Model.Enum;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.API.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Checks whether the input has parenthesis in the well formatted fashion.
        /// </summary>
        /// <param name="lispCode">A string that is basically a LISP code</param>
        /// <returns>True is parentheses are well formed false if not.</returns>
        public static bool IsWellFormedParenthesis_V2(string filePath)
        {
            var lispCode = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(lispCode))
                return false;

            var startDelimiterCount = 0;
            var endDelimiterCount = 0;
            using (var reader = new StreamReader(filePath))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {

                    var charArray = line.ToCharArray();
                    foreach (var item in charArray)
                    {
                        if (item == '(') startDelimiterCount++;
                        else if (item == ')') endDelimiterCount++;
                    }
                }
            }

            return (startDelimiterCount > 0 && endDelimiterCount > 0 && (startDelimiterCount == endDelimiterCount));

        }

        public static bool IsWellFormedParenthesis(this string fileContenets)
        {

            var startDelimiterCount = 0;
            var endDelimiterCount = 0;
            var charArray = fileContenets.ToCharArray();
            foreach (var item in charArray)
            {
                if (item == '(') startDelimiterCount++;
                else if (item == ')') endDelimiterCount++;
            }

            return (startDelimiterCount > 0 && endDelimiterCount > 0 && (startDelimiterCount == endDelimiterCount));

        }

        public static async Task<string> ReadAsList(this IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString();
        }

        

        /// <summary>
        /// Converts viemodel to Entity
        /// </summary>
        /// <param name="dto">Viewmodel</param>
        /// <returns>Entity Model</returns>
        public static HealthCareProvider ToEntityModel(this ProviderDto dto)
        {
            if (dto == null)
                return null;
            return new HealthCareProvider
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NPINumber = dto.NPINumber,
                Phone = dto.Phone,
                Email  = dto.Email,
                ProviderAddresses = new Assignment.Model.Entity.Address
                {
                    
                    Address1 = dto.ProviderAddress.Address1,
                    City = dto.ProviderAddress.City,
                    StateId = int.Parse(dto.ProviderAddress.State),
                    ZipCode = dto.ProviderAddress.ZipCode,
                    AddressTypeId = (int)AddressType.BUSINESS
                }
            };
        }

        public static ProviderDto ToDto(this HealthCareProvider model)
        {
            if (model == null)
                return null;
            return new ProviderDto
            {
                Id = model.ProviderId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                NPINumber = model.NPINumber,
                Phone = model.Phone,
                ProviderAddress = new AddressDto
                {
                    Address1 = model.ProviderAddresses.Address1,
                    City = model.ProviderAddresses.City,
                    State = model.ProviderAddresses.State,
                    ZipCode = model.ProviderAddresses.ZipCode
                }
            };
        }


    }
}
