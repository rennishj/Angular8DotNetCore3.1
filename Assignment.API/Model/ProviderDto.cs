using System.ComponentModel.DataAnnotations;

namespace Assignment.API
{
    public class ProviderDto
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(maximumLength:50, ErrorMessage = "FirstName must be a max of  50 characters long.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "LastName must be 50 characters long.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10,MinimumLength =10, ErrorMessage = "NPINumber must be 10 characters long.")]
        public string NPINumber { get; set; }

        [Required]
        [StringLength(12,MinimumLength =10, ErrorMessage = "Phone must be 10 characters long.")]
        public string Phone { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Email must be a max of 30 characters long.")]
        public string Email { get; set; }

        [Required]
        public AddressDto ProviderAddress { get; set; } = new AddressDto();
    }
}
