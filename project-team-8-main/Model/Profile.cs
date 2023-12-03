using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_Authentication.Model
{
    public class Profile
    {
       public int ProfileID { get ; set; }
       [Required]
       public int UserID { get; set; }

       [ForeignKey("UserID")]
       public User User { get; set; }

        public string? Semester { get; set; }
        public string? Interest { get; set;  }
        public string? MainCoding_Language { get; set; } 
        public string? Contact_Detail { get; set; }
        public string? UserName { get; set; }    

    }
}
