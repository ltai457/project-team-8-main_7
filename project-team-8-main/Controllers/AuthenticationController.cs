using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Project_Authentication.Data;
using Project_Authentication.Dto;
using Project_Authentication.Dtos;
using Project_Authentication.JWT;
using Project_Authentication.Model;
using System.Drawing.Text;
using System.Security.Policy;
using Google.Apis.Auth;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Project_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticaionRepo _authenticationRepo;
        private readonly ITokenManager _tokenManager;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticaionRepo authenticationRepo, ITokenManager tokenManager, IMapper mapper)
        {
            _authenticationRepo = authenticationRepo;
            _tokenManager = tokenManager;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the user already exists
            var existingUser = _authenticationRepo.GetUserByUsername(registerDto.Email);
            if (existingUser != null)
            {
                return Conflict("User with this email already exists.");
            }

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create a new user object
            var user = new User
            {
                UserName = registerDto.UserName,
                Password = hashedPassword,

                Email = registerDto.Email,
                RoleID = registerDto.RoleID
            };

            // Add the user to the database
            var result = _authenticationRepo.RegisterUser(user);
            if (result > 0)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Failed to register user.");
            }
        }






        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AuthResponse> GetDetail(Login user)
        {
            var authUser = _authenticationRepo.CheckCredential(user);
            if (authUser == null)
            {
                return NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, authUser.Password))
            {
                return BadRequest("Incorrect Password");
            }

            var roleName = _authenticationRepo.GetUserRole(authUser.RoleID);
            var authResponse = new AuthResponse()
            {
                IsAuthenticated = true,
                Role = roleName,
                Token = _tokenManager.GenerateToken(authUser, roleName),
                UserName = authUser.UserName,
                UserID = authUser.UserID
            };

            return Ok(authResponse);
        }

        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            // Check if the user exists
            var user = _authenticationRepo.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            // Delete the user
            _authenticationRepo.DeleteUser(user);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("GoogleLogin")]
        public IActionResult GoogleLogin([FromBody] GoogleAuthDto googleAuthDto)
        {
            try
            {
                // Verify the Google authentication token
                GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(googleAuthDto.Code, new GoogleJsonWebSignature.ValidationSettings()).Result;

                // Extract user information from the payload
                string email = payload.Email;
                string name = payload.Name;

                // Check if the user already exists in your database
                var existingUser = _authenticationRepo.GetUserByEmail(email);
                if (existingUser == null)
                {
                    // User does not exist, create a new user account
                    var temporaryPassword = GenerateTemporaryPassword(); // Generate a temporary password

                    var newUser = new User
                    {
                        UserName = name,
                        Email = email,
                        Password = temporaryPassword,
                        RoleID = email.EndsWith("@aucklanduni.ac.nz") ? 2 : 3
                        // Set other properties as needed
                    };

                    // Add the new user to the database
                    var result = _authenticationRepo.RegisterUser(newUser);
                    if (result <= 0)
                    {
                        return BadRequest("Failed to register user.");
                    }

                    var roleName = _authenticationRepo.GetUserRole(newUser.RoleID);
                    var authResponse = new AuthResponse
                    {
                        IsAuthenticated = true,
                        Role = roleName,
                        Token = _tokenManager.GenerateToken(newUser, roleName),
                        UserName = newUser.UserName,
                        UserID = newUser.UserID
                    };

                    // Return the authentication response for the new user
                    return Ok(authResponse);
                }
                else
                {
                    var existingUserRole = _authenticationRepo.GetUserRole(existingUser.RoleID);
                    var existingUserAuthResponse = new AuthResponse
                    {
                        IsAuthenticated = true,
                        Role = existingUserRole,
                        Token = _tokenManager.GenerateToken(existingUser, existingUserRole),
                        UserName = existingUser.UserName,
                        UserID = existingUser.UserID
                    };

                    // Return the authentication response for the existing user
                    return Ok(existingUserAuthResponse);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid Google authentication token.");
            }
        }



        private string GenerateTemporaryPassword()
        {
            // Generate a temporary password using any desired logic
            // For example, you can use a combination of random characters or a GUID
            string temporaryPassword = "Temp1234"; // Replace with your own logic
            return temporaryPassword;
        }
    }
}

