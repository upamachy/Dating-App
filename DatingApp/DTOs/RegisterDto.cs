using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
    {
    public class RegisterDto
        {


        [Required] //specifies that the property is required and cannot be null or empty.
        public string DisplayName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = ""; 

        [Required]
        [MinLength(4)]
        public string Password { get; set; } = "";
        }
    }
