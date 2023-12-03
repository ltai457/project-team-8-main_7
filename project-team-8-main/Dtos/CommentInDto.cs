using MessagePack;
using System.Security;

namespace Project_Authentication.Dto
{
    public class CommentInDto
    {
        public int ProjectID { get; set; }

        public string? CommentText { get; set; }

        
    }
}
