using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Project_Authentication.Data;
using Project_Authentication.Dtos;
using System.Threading.Tasks;

namespace Project_Authentication.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepo _likeService;

        private readonly ProjectDBContext _dBContext;
        private readonly IProjectRepo _projectRepo;

        public LikeController(ILikeRepo likeService,ProjectDBContext dBContext,IProjectRepo projectRepo)
        {
            _likeService = likeService;
            _dBContext = dBContext; 
            _projectRepo = projectRepo;

        }

        ///[HttpPost("{projectId,userId}")]
        ////public async Task<IActionResult> AddLikeToProject(int projectId,int userID)
         [HttpPost("LikeProject/{projectId}")]
          [Authorize(Roles = "Admin,Student,Visitor")]
          public IActionResult LikeProject([FromRoute] int projectId)
          {
              try
              {
                  var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                  int userId = int.Parse(userIdClaim.Value);
                  var user = _dBContext.Users
                      .Include(u => u.LikedProjects)
                      .ThenInclude(lp => lp.Project)
                      .FirstOrDefault(u => u.UserID == userId);

                  if (user == null)
                  {
                      return NotFound($"User with ID {userId} not found.");
                  }

                  var project = _dBContext.Projects
                      .FirstOrDefault(p => p.ProjectID == projectId);

                  if (project == null)
                  {
                      return NotFound($"Project with ID {projectId} not found.");
                  }

                  if (user.LikedProjects.Any(lp => lp.ProjectID == projectId))
                  {
                      return BadRequest("The user has already liked the project.");
                  }

                  var like = new Like
                  {
                      ProjectID = projectId,
                      UserID = userId,
                      DateLiked = DateTime.Now,
                      UserName = user.UserName
                  };

                  user.LikedProjects.Add(like);
                  _dBContext.Likes.Add(like);
                  _dBContext.SaveChanges();

                  return Ok();
              }
              catch (Exception ex)
              {
                  // Log the exception
                  return StatusCode(500, "An error occurred while liking the project.");
              }
          }
      /*  [HttpPost("LikeProject")]
        [Authorize(Roles = "Admin,Student,Visitor")]
        public LikeOutDto LikeProject([FromBody] LikeInDto likeInDto)
        {
            // Retrieve the UserID and UserName from the JWT token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            int userId = int.Parse(userIdClaim.Value);

            var userNameClaim = User.Claims.FirstOrDefault(c => c.Type == "UserName");
            string userName = userNameClaim.Value;

            Like like = new Like(likeInDto.ProjectID, userId, userName);
            _likeService.LikeProject(like);

            Project_Authentication.Model.Project project = _projectRepo.GetProjectByID(likeInDto.ProjectID);

            return new LikeOutDto
            {
                ProjectLikeID = like.ProjectLikeID,
                ProjectID = like.ProjectID,
                UserID = like.UserID,
                UserName = like.UserName,
                DateLiked = like.DateLiked,
            };
        }*/








        /// still need to debug 
        //[HttpDelete("UnlikeProject{projectId}/{userId}")]
        /* public IActionResult UnlikeProject(int projectId, int userId)
         {
             try
             {
                 var user = _dBContext.Users
                     .Include(u => u.LikedProjects)
                     .ThenInclude(lp => lp.Project)
                     .FirstOrDefault(u => u.UserID == userId);

                 if (user == null)
                 {
                     return NotFound($"User with ID {userId} not found.");
                 }

                 var project = _dBContext.Projects
                     .FirstOrDefault(p => p.ProjectID == projectId);

                 if (project == null)
                 {
                     return NotFound($"Project with ID {projectId} not found.");
                 }

                 if (user.LikedProjects.Any(lp => lp.ProjectID == projectId))
                 {
                     return BadRequest("The user has not liked the project.");
                 }

                 user.LikedProjects.Remove(new Like
                 {
                     ProjectID = projectId,
                     UserID = userId,
                     DateLiked = DateTime.Now
                 });

                 _dBContext.SaveChanges();

                 return Ok();
             }
             catch (Exception ex)
             {
                 // Log the exception
                 return StatusCode(500, "An error occurred while liking the project.");
             }

         }*/
        [HttpPost("AwardProject")]
        [Authorize(Roles = "Admin")]
        public ActionResult AwardProject([FromBody]AwardDto award)
        {
            if (_likeService.AwardProject(award) == 0)
            {
                return Ok("Successfully Awarded");
            }
            else
            {
                return NotFound("No Project Found.");
            }
        }



        /*[HttpGet("WinningProjects/{awardName}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<Project>> GetWinningProjects(string awardName)
        {
            try
            {
                var winningProjects = _likeService.GetWinningProjects(awardName);
                return Ok(winningProjects);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("WinningProjects")]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<Project>> GetWinningProjects()
        {
            var winningProjects = _dBContext.Projects
                .Where(p => p.ExCellenceAwardWinner || p.ExCellenceAwardRunnerUp || p.CommunityImpactAwardWinner || p.CommunityImpactAwardRunneruup || p.PeopleChoiceAwardWinner || p.PeopleChoiceAwardRunnerUp)
                .ToList();

            return Ok(winningProjects);
        }*/

    }
}








