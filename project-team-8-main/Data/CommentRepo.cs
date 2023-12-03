using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Project_Authentication.Model;

namespace Project_Authentication.Data
{
    public class CommentRepo:ICommentRepo
    {
        private readonly ProjectDBContext _dbcontext;
        public CommentRepo(ProjectDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public Comment WriteComment(Comment comment)
        {
            // Retrieve the user from the database based on the UserID in the Comment
            User user = _dbcontext.Users.FirstOrDefault(u => u.UserID == comment.UserID);

            // Set the UserName property of the Comment to the Username of the User
            comment.UserName = user?.UserName;

            // Add the Comment to the DbContext and save changes
            EntityEntry<Comment> e = _dbcontext.Comments.Add(comment);
            Comment c = e.Entity;
            _dbcontext.SaveChanges();

            return c;
        }

            public void DeleteComment(Comment comment)
        {
            // Get all comments
            IEnumerable<Comment> comments = _dbcontext.Comments.ToList<Comment>();

            // Check if comment exists
            if (comments.FirstOrDefault(e => e.CommentID == comment.CommentID) != null)
            {
                // Delete comment
                Comment commentToDelete = comments.FirstOrDefault(e => e.CommentID == comment.CommentID);
                _dbcontext.Comments.Remove(commentToDelete);
                _dbcontext.SaveChanges();
            }
        }
        public Comment GetCommentByID(int id)
        {
            Comment comment = _dbcontext.Comments.FirstOrDefault(e => e.CommentID == id);
            return comment;
        }
        public IEnumerable<Comment> GetAllCommentsByID(int num)
        {
            IEnumerable<Comment> comments = _dbcontext.Comments.ToList<Comment>();
            IEnumerable<Comment> items = comments.Where(e => e.ProjectID == num);
            return items;
        }
        public IEnumerable<Comment> GetComments()
        {
            IEnumerable<Comment> comments = _dbcontext.Comments.ToList<Comment>();
            return comments;
        }
       
    }
}
