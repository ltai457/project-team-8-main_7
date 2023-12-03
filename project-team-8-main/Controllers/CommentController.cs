using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Project_Authentication.Data;
using Project_Authentication.Dto;
using Project_Authentication.Dtos;
using Project_Authentication.Model;

namespace Project_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IAuthenticaionRepo _authenticaionRepo; 
        private readonly ProjectDBContext _dbContext;   
        public CommentController(ICommentRepo commentRepo,IAuthenticaionRepo authenticaionRepo,ProjectDBContext dBContext)
        {
            _commentRepo = commentRepo; 
            _authenticaionRepo = authenticaionRepo;
            _dbContext = dBContext;
        }

        [HttpPost("WriteComment")]
        [Authorize(Roles = "Admin,Student,Visitor")]
        public CommentOutDto WriteComment([FromBody] CommentInDto commentInDto)
        {
            // Retrieve the UserID from the JWT token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            int userId = int.Parse(userIdClaim.Value);

            // Retrieve the user name from the user database
            var user = _dbContext.Users.FirstOrDefault(u => u.UserID == userId);
            string username = user?.UserName;

            Comment c = new() { ProjectID = commentInDto.ProjectID, CommentText = commentInDto.CommentText, UserID = userId };
            Comment addedComment = _commentRepo.WriteComment(c);
            CommentOutDto commentOutDto = new() { ProjectID = addedComment.ProjectID, CommentText = addedComment.CommentText, UserID = addedComment.UserID, UserName = username, OriginalTimestamp = addedComment.DateTimes};
            commentOutDto.OriginalTimestamp = DateTime.Now;
            return commentOutDto;
        }

        [HttpGet("GetProjectComments")]
        public ActionResult<IEnumerable<CommentOutDto>> GetProjectComments(int id)
        {
            IEnumerable<Comment> comments = _commentRepo.GetAllCommentsByID(id);
            IEnumerable<CommentOutDto> c = comments.Select(e => new CommentOutDto
            {
                ProjectID = e.ProjectID,
                CommentText = e.CommentText,
                UserName = e.UserName,
                OriginalTimestamp = e.DateTimes
            });
            return Ok(c);
        }

        [HttpGet("GetComments")]
        ////[Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<Project>> GetComments()
        {
            IEnumerable<Comment> comments = _commentRepo.GetComments();
            IEnumerable<CommentOutDto> c = comments.Select(e => new CommentOutDto {  ProjectID = e.ProjectID, CommentText = e.CommentText,UserName=e.UserName });
            return Ok(c);
        }
        // DELETE /webapi/DeleteComment/{id}
        [HttpDelete("DeleteComment")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteComment(int id)
        {
            Comment c = _commentRepo.GetCommentByID(id);
            if (c == null)
                return NotFound();
            else
            {
                _commentRepo.DeleteComment(c);
                return NoContent();
            }
        }
    }
}
