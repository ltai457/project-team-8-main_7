using System.ComponentModel.DataAnnotations;

namespace Project_Authentication.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        
        public string? Email { get; set; }

        [Required]
        
        public string? Password { get; set; }

        
        [Required]
        public int RoleID { get; set; }

       
    }
}
