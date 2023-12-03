using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Authentication.Model
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        [Required]
        
        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        public Project Project { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        public string? UserName { get; set; }
        public string? CommentText { get; set; }
        public DateTime DateTimes { get; set; } = DateTime.Now;
    }
}