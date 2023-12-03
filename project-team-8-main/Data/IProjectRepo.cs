using Project_Authentication.Dto;
using Project_Authentication.Model;

namespace Project_Authentication.Data
{
    public interface IProjectRepo 
    {
        IEnumerable<Project> GetProjectBySemester(string semester);

        IEnumerable<Project> GetProjectByName(string Name);
        int GetLikesCount(int projectId);
        //GetProjectByID
        Project GetProjectByID(int ID);
        Project GetProjectByUserID(int ID);
        IEnumerable<Project> GetProjectTop5();
        IEnumerable<Project> GetProjectsLike5();
    }
}
