using Project_Authentication.Dto;
using Project_Authentication.Dtos;
using Project_Authentication.Model;

namespace Project_Authentication.Data
{
    public interface ILikeRepo
    {
       void  LikeProject(Like like);
       int AwardProject (AwardDto awardDto);
        List<Project> GetWinningProjects(string awardName);
    }
}
