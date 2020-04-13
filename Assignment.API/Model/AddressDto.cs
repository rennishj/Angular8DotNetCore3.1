using System.ComponentModel.DataAnnotations;

namespace Assignment.API
{
    public class AddressDto
    {
        public int? Id { get; set; }
       
        public string ProviderId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Address1 must be 50 characters long.")]
        public string Address1 { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "City must be 20 characters long.")]
        public string City { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "State must be 10 characters long.")]
        public string State { get; set; }

        [Required]
        [StringLength(10,MinimumLength =5,ErrorMessage = "ZipCode must be 5 characters long.")]
        public string ZipCode { get; set; }

        public int AddressTypeId { get; set; }
    }
}
