using Microsoft.Build.ObjectModelRemoting;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Authentication.Model
{
    public class User
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        
        public string? ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public int RoleID { get; set; }

       
        public string? RoleName { get; set; }
        public Role? Role { get; set; }
        public ICollection<Comment>? Comment { get; set; }   
        public ICollection<Like>? LikedProjects { get; set; } // Updated to a collection of ProjectLike entities

       
    }
}

