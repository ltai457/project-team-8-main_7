using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Authentication.Data;
using Project_Authentication.Dtos;
using Project_Authentication.Model;
using System.Diagnostics.Contracts;

namespace Project_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticaionRepo _authenticationRepo;
        private readonly ProjectDBContext _dbContext;

        public UserController(IAuthenticaionRepo authenticaion, ProjectDBContext dBContext)
        {
            _authenticationRepo = authenticaion;
            _dbContext = dBContext;
        }
        [HttpGet("AllUser")]

        ////[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserListDto>>> GetUser()
        {
            var users = await _dbContext.Users.ToListAsync();
            if (users == null || users.Count == 0)
            {
                return NotFound();
            }

            var userListDtos = users.Select(user => new UserListDto
            {
                UserName = user.UserName,
                Email = user.Email,
                RoleID = user.RoleID,
                UserID = user.UserID
            }).ToList();

            return userListDtos;

        }
    }
}

