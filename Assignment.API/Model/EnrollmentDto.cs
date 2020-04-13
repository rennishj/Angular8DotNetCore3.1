using Microsoft.AspNetCore.Http;

namespace Assignment.API.Model
{
    public class FileUploadDto
    {        
        public IFormFile FileContent { get; set; }
        public string PathToBurstedFile { get; set; }
    }

    public class EnrollmentFile
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Version { get; set; }
        public string InsuranceCompany { get; set; }

        /// <summary>
        /// Returns a comma separated property names.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{InsuranceCompany},{FirstName},{LastName},{UserId},{Version}";
        }

    }
}
