using Project_Authentication.Model;

namespace Project_Authentication.Data
{
    public interface ICommentRepo
    {
        public Comment WriteComment(Comment comment);
        public void DeleteComment(Comment comment);
        public IEnumerable<Comment> GetAllCommentsByID(int projectID);
        public IEnumerable<Comment> GetComments();
        public Comment GetCommentByID(int id);
        
    }
}
