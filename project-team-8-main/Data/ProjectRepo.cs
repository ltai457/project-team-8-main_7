using Microsoft.EntityFrameworkCore;
using Project_Authentication.Dto;
using Project_Authentication.Model;
using System.Xml.Linq;

namespace Project_Authentication.Data
{
    public class ProjectRepo : IProjectRepo
    {
        private readonly ProjectDBContext _dbcontext;
        public ProjectRepo(ProjectDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IEnumerable<Project> GetProjectBySemester(string semester)
        {
            return _dbcontext.Projects.Where(p =>EF.Functions.Like( p.Semester ,"%" + semester + "%"));
        }
        public Project_Authentication.Model.Project GetProjectByID(int ID)
        {
            return _dbcontext.Projects.FirstOrDefault(p => p.ProjectID == ID);
        }

        public IEnumerable<Project> GetProjectByName(string Name)
        {
            return _dbcontext.Projects.Where(p => EF.Functions.Like(p.ProjectName, "%" + Name + "%"));
        }/// This will help to find not need to actual match, result will contain partial match 

        public int GetLikesCount(int projectId)
        {
            return _dbcontext.Projects
                .Include(p => p.Likes)
                .Where(p => p.ProjectID == projectId)
                .Select(p => p.Likes.Count())
                .FirstOrDefault();
        }
          public IEnumerable<Project> GetProjectTop5()
        {
            IEnumerable<Project> cmt = _dbcontext.Projects.Where(p=>p.IsApproved).OrderByDescending(e => e.DateCreated).Take(5).ToList<Project>();
            return cmt;
        }/// Thi

        public IEnumerable<Project> GetProjectsLike5()
        {
            IEnumerable<Project> cmt = _dbcontext.Projects.Where(p => p.IsApproved).OrderByDescending(e => e.Likes.Count()).Take(5).ToList<Project>();
            return cmt;
        }
        public Project_Authentication.Model.Project GetProjectByUserID(int ID)
        {
            return _dbcontext.Projects.FirstOrDefault(p => p.OwnerID== ID);
        }




    }
}
