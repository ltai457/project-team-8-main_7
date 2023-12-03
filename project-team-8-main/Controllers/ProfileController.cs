using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Authentication.Data;
using Project_Authentication.Dto;
using Project_Authentication.Dtos;
using Project_Authentication.Model;

namespace Project_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ProjectDBContext _context;
        private readonly IProfileRepo _profileRepo;

        public ProfileController(ProjectDBContext context,IProfileRepo profileRepo )
        {
            _context = context;
            _profileRepo = profileRepo;
        }

        // GET: api/Profile
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles()
        {
            // Get the currently authenticated user's ID from the token
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Forbid(); // Return 403 Forbidden if the user ID claim is missing or invalid
            }

            var profiles = await _context.Profiles.Where(p => p.UserID == authenticatedUserId).ToListAsync();

            if (profiles == null)
            {
                return NotFound();
            }

            return profiles;
        }



        // GET: api/Profile/5
        [HttpGet("ByUserId/{userId}")]
        [Authorize]
        public async Task<ActionResult<Profile>> GetProfileByUserId(int userId)
        {
            // Get the currently authenticated user's ID from the token
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Forbid(); // Return 403 Forbidden if the user ID claim is missing or invalid
            }

            // Check if the authenticated user matches the requested user ID
            if (authenticatedUserId != userId)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized to access the data
            }

            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserID == userId);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }


        [HttpGet("Interest/{Interest}")]
        [Authorize(Roles = "Admin")]
        public IActionResult SearchByInterest(string interest)
        {
            var profiles = _profileRepo.SearchByInterest(interest);

            if (profiles == null)
            {
                return NotFound();
            }

            return Ok(profiles);
        }
        [HttpGet("semester/{semester}")]
        [Authorize(Roles = "Admin")]
        public IActionResult SearchBySemester(string semester)
        {
            var profiles = _profileRepo.SearchBySemeseter(semester);

            if (profiles == null)
            {
                return NotFound();
            }

            return Ok(profiles);
        }

        [HttpGet("Language/{Language}")]
        [Authorize(Roles = "Admin")]
        public IActionResult SearchByMainCodingLanguage(string language)
        {
            var profiles = _profileRepo.SearchByMainCodingLanguage(language);

            if (profiles == null)
            {
                return NotFound();
            }

            return Ok(profiles);
        }


        // PUT: api/Profile/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(int id, [FromBody] ProfileInDto profileDto)
        {
            // Get the currently authenticated user's ID from the token
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int authenticatedUserId))
            {
                return Forbid(); // Return 403 Forbidden if the user ID claim is missing or invalid
            }

            // Check if the authenticated user matches the requested user ID
            if (authenticatedUserId != id)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized to update the profile
            }

            // Retrieve the existing profile from the database
            var existingProfile = await _context.Profiles.FindAsync(id);
            if (existingProfile == null)
            {
                return NotFound();
            }

            // Update the properties of the existing profile with the values from the incoming profileDto
            existingProfile.Semester = profileDto.Semester;
            existingProfile.Interest = profileDto.Interest;
            existingProfile.MainCoding_Language = profileDto.MainCoding_Language;
            existingProfile.Contact_Detail = profileDto.Contact_Detail;
            // Update other properties as needed

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Profile Update Successfully.");
        }


        // POST: api/Profile
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin,Student,Visitor")]
        public ProfileOutDto AddProfile ([FromBody]ProfileInDto profileInDto)
        {
            // Retrieve the UserID from the JWT token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            int userId = int.Parse(userIdClaim.Value);

            // Retrieve the user name from the user database
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            string username = user?.UserName;
            Profile p = new() { Interest = profileInDto.Interest, Contact_Detail = profileInDto.Contact_Detail, MainCoding_Language=profileInDto.MainCoding_Language ,Semester = profileInDto.Semester,UserID = userId };
            Profile addedProfile = _profileRepo.AddNewProfile(p);
            ProfileOutDto profileOutDto = new() { Interest = addedProfile.Interest, Contact_Detail = addedProfile.Contact_Detail,MainCoding_Language= addedProfile.MainCoding_Language,Semester= addedProfile.Semester, UserID = addedProfile.UserID, UserName = username };
            return profileOutDto;
        }
        

        // DELETE: api/Profile/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            if (_context.Profiles == null)
            {
                return NotFound();
            }
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        /*[HttpPost("UploadProfilePicture/{userId}")]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(int userId, IFormFile file)
        {
            // Get the currently authenticated user's ID from the token
            var authenticatedUserIdClaim = User.FindFirst("UserId");
            if (authenticatedUserIdClaim == null || !int.TryParse(authenticatedUserIdClaim.Value, out int authenticatedUserId))
            {
                return Forbid(); // Return 403 Forbidden if the user ID claim is missing or invalid
            }

            // Ensure that the authenticated user is the owner of the profile picture being uploaded
            if (authenticatedUserId != userId)
            {
                return Forbid(); // Return 403 Forbidden if the authenticated user is not the owner
            }

            // Retrieve the profile of the user
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserID == userId);
            if (profile == null)
            {
                return NotFound(); // Return 404 Not Found if the profile does not exist
            }

            // Upload and save the profile picture
            if (file != null)
            {
                // Process the uploaded file and save it to a storage location (e.g., file system or cloud storage)
                // Here, you can implement your logic to save the file and update the profile picture URL in the profile object

                // For example, you can save the file with a unique name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine("profile-pictures", fileName); // Specify the directory or storage location

                // Save the file to the storage location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Update the profile picture URL in the profile object
                profile.ProfilePictureUrl = filePath; // Update with the appropriate URL or file path
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Return a successful response
            return Ok("Profile picture uploaded successfully");
        }*/

        private bool ProfileExists(int id)
        {
            return (_context.Profiles?.Any(e => e.ProfileID == id)).GetValueOrDefault();
        }
    }
}
