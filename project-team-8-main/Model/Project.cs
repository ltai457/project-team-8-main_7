using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_Authentication.Model
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }

        [Required]
        public string TeamName { get; set; }

        [Required]
        public int TeamNumber { get; set; }

        public string? TeamMember { get; set; }

        [Required]
        public string? ProjectName { get; set; }

        [Required]
        public string? ProjectOverView { get; set; }
        ///public string? Introduction { get; set; }
        ///public string? Description { get; set; }

        [Required]
        public string? Semester { get; set; }

        public string? Img { get; set; }

        public string? LinktoGit { get; set; }

        [Required]
        public string? LinktoYoutube { get; set; }

        public string? LanguageUsed { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Like>? Likes { get; set; }
        public bool IsWinner { get; set; }  
        public int LikeCount { get; set; }
        public int OwnerID { get; set; }
        public bool IsApproved { get; set; } = false; 

        ///public bool ExCellenceAwardWinner { get; set; } = false;
       

        ///public bool ExCellenceAwardRunnerUp { get; set; } = false;
        //public bool CommunityImpactAwardWinner { get; set; } = false;
        ///public bool CommunityImpactAwardRunnerUp { get; set; } = false;




        ///public bool PeopleChoiceAwardWinner { get; set; } = false;
        ///public bool PeopleChoiceAwardRunnerUp { get; set; } = false;


    }
}
/* admin@gmail.com
    admin123456
    Lykheang@gmail.com
    Lykheang123
    Kunye@gmail.com
    Kunye123456
    Ganming@gmail.com
    Ganming123456
    Chasity@gmail.com
    Chasity123456
    Yutian@gmail.com
    Yutian123456
    Abby@gmail.com
    Abby123456
    Best@gmail.com
    Best123456
    Cici@gmail.com
    Cici123456
*/