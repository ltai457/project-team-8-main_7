using System.ComponentModel.DataAnnotations;

namespace Project_Authentication.Model
{
    public class Role
    {
        [Key] 
        public int RoleID { get; set; } 
        public string RoleName { get; set; } = string.Empty;
        public string? RoleDescription { get; set; }
        public ICollection<User>? Users { get; set; }
        ///// one role can contain many user
    }
}
