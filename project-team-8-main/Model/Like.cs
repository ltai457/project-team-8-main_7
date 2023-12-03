using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project_Authentication.Model;

public class Like
{
    /*public Like() { }*/

    [Key]
    public int ProjectLikeID { get; set; }

    [Required]
    public int ProjectID { get; set; }

    [ForeignKey("ProjectID")]
    public Project Project { get; set; }

    [Required]
    public int UserID { get; set; }

    [ForeignKey("UserID")]
    public User User { get; set; }

    [Required]
    public string UserName { get; set; }

    public DateTime DateLiked { get; set; } = DateTime.Now;

    /*public Like(int projectId, int userId, string userName)
    {
        this.ProjectID = projectId;
        this.UserID = userId;
        this.UserName = userName;
    }*/
}
