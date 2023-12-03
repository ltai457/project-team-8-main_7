using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Project_Authentication.Dto
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
