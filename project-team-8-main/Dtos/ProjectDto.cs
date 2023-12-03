using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace Project_Authentication.Dtos
{
    public class ProjectDto
    {
        
        [Required]
        public string TeamName { get; set; }
        [Required]
        public int TeamNumber { get; set; }
        public string? TeamMember { get; set; }
        [Required]
        public string? ProjectName { get; set; }
        [Required]
        public string? ProjectOverView { get; set; }
        //public string? Introduction { get; set; }
        //public string? Description { get; set; }
        [Required]
        public string? Semester { get; set; }

        public IFormFile? Img { get; set; }
        public string? LinktoGit { get; set; }
        [Required]
        public string? LinktoYoutube { get; set; }
        public string? LanguageUsed { get; set; }
        
        
    }
}
