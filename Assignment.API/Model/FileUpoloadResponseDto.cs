using System.Collections.Generic;

namespace Assignment.API.Model
{
    public class FileUpoloadResponseDto
    {
        public string UploadedFileName { get; set; }
        public List<string> BursetedFiles { get; set; } = new List<string>();
        public bool IsValid { get; set; }
    }
}
