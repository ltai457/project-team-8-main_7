using System.ComponentModel.DataAnnotations;

namespace Project_Authentication.Dto
{
    public class RoleDto
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? RoleDescription { get; set; }
    }
}
