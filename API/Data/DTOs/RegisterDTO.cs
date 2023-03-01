using System.ComponentModel.DataAnnotations;

namespace API.Data.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}