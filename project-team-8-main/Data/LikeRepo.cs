using Microsoft.Build.ObjectModelRemoting;
using Microsoft. EntityFrameworkCore;
using Project_Authentication.Dtos;
using Project_Authentication.Model;

namespace Project_Authentication.Data
{
    public class LikeRepo : ILikeRepo
    {
        private readonly ProjectDBContext _dbContext;

        public LikeRepo(ProjectDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void LikeProject(Like like)
        {
            User user = _dbContext.Users
                .Include(u => u.LikedProjects)
                .FirstOrDefault(u => u.UserID == like.UserID);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            bool hasLiked = user.LikedProjects.Any(l => l.ProjectID == like.ProjectID);

            if (hasLiked)
            {
                throw new InvalidOperationException("The user has already liked the project.");
            }

            user.LikedProjects.Add(like);

            _dbContext.SaveChanges();
        }


        public int AwardProject(AwardDto awardDto)
        {
            Project project = _dbContext.Projects.FirstOrDefault(p => p.ProjectID == awardDto.ProjectID);
            if (project != null)
            {
                project.IsWinner = awardDto.IsWinner;
                _dbContext.SaveChanges();
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public List<Project> GetWinningProjects(string awardName)
        {
            var awardProperty = awardName switch
            {
                "IsWinner" => nameof(Project.IsWinner),
                
                
                _ => null
            };

            if (awardProperty == null)
            {
                throw new ArgumentException("Invalid award name");
            }

            var winningProjects = _dbContext.Projects.Where(p => (bool)p.GetType().GetProperty(awardProperty).GetValue(p) == true).ToList();

            return winningProjects;
        }

    }
}

    

