using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Project_Authentication.Data;
using Project_Authentication.Dto;
using Project_Authentication.Dtos;
using Project_Authentication.Model;
using Project = Project_Authentication.Model.Project;

namespace Project_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectDBContext _context;
        private readonly IProjectRepo _projectRepo;


        public ProjectsController(ProjectDBContext context, IProjectRepo projectRepo)
        {
            _context = context;
            _projectRepo = projectRepo;
        }
        [HttpGet("GetLogo")]
        [Authorize(Roles = "Admin")]

        public ActionResult GetLogo()
        {
            string path = Directory.GetCurrentDirectory();
            string imgDir = Path.Combine(path, "img\\Logo");
            string fileName1 = Path.Combine(imgDir, "Logo" + ".png");

            string respHeader = "";
            string fileName = "";
            respHeader = "image/png";


            return PhysicalFile(fileName1, respHeader);
        }

        // GET: api/Projects
        [HttpGet("GetAllProject")]
        ////[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var approvedProjects = await _context.Projects.Where(p => p.IsApproved).ToListAsync();
            return approvedProjects;
        }

        // GET: api/Projects/5
        [HttpGet("Project{id}")]

        ////[Authorize(Roles = "Admin,Student")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }
        [HttpGet("semester/{semester}")]
        /////[Authorize(Roles = "Admin")]
        public IActionResult GetProjectsBySemester(string semester)
        {
            var projects = _projectRepo.GetProjectBySemester(semester);

            if (projects == null)
            {
                return NotFound();
            }

            return Ok(projects);
        }
        [HttpGet("GetTop5NewestProject")]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetProjectsTop5()
        {
            var projects = _projectRepo.GetProjectTop5();

            if (projects == null)
            {
                return NotFound();
            }


            return Ok(projects);
        }
        [HttpGet("GetTop5PopularProject")]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetTop5PopularProject()
        {
            var projects = _projectRepo.GetProjectsLike5();

            if (projects == null)
            {
                return NotFound();
            }

            return Ok(projects);
        }




        [HttpGet("ProjectByName/{Name}")]
        /////[Authorize(Roles = "Admin,Student")]
        public IActionResult GetProjectsByName(string Name)
        {
            var projects = _projectRepo.GetProjectByName(Name);

            if (projects == null)
            {
                return NotFound();
            }

            return Ok(projects);
        }
        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateProjectBy/{projectId}")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> PutProject(int projectId, [FromBody] ProjectUpdateDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest();
            }

            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            // Update the properties of the project entity with the values from projectDto
            ///project.TeamName = projectDto.TeamName;
            ////project.TeamNumber = projectDto.TeamNumber;
            project.TeamMember = projectDto.TeamMember;
            project.ProjectName = projectDto.ProjectName;
            project.ProjectOverView = projectDto.ProjectOverView;
            project.LanguageUsed = projectDto.LanguageUsed;
            project.Semester = projectDto.Semester;
            project.TeamName = projectDto.TeamName;
            project.TeamNumber = projectDto.TeamNumber; 
            // Update other properties as needed

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(projectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Project Updated");
        }



        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost("AddNewProject")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<ActionResult<Project>> PostProject([FromBody]ProjectDto projectDto)
        {
          if (_context.Projects == null)
          {
              return Problem("Entity set 'ProjectDBContext.Projects'  is null.");
          }
            var project = new Project
            {
                TeamName = projectDto.TeamName,
                TeamNumber = projectDto.TeamNumber,
                TeamMember = projectDto.TeamMember,
                ProjectName = projectDto.ProjectName,
                Introduction = projectDto.Introduction,
                Description = projectDto.Description,
                Semester = projectDto.Semester,
                Img = projectDto.Img,
                LinktoGit = projectDto.LinktoGit,
                LinktoYoutube = projectDto.LinktoYoutube,
                LanguageUsed = projectDto.LanguageUsed,
                DateCreated = DateTime.Now
            };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ProjectID }, project);
        }*/

        // DELETE: api/Projects/5
        [HttpDelete("DeleteProjectBy{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound("No Project Found");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound("No Project Found");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok("Successfully Deleted");
        }

        /* [HttpGet("GetImage/{id}")]
         // [Authorize(Roles = "Admin")]
         public async Task<IActionResult> GetImage(int id)
         {
             // retrieve the project object from the database
             var project = await _context.Projects.FindAsync(id);

             if (project == null)
             {
                 return NotFound();
             }

             // check if the img property of the project object is not null or empty
             if (string.IsNullOrEmpty(project.Img))
             {
                 return NotFound();
             }

             // get the file path of the image
             var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", project.Img);

             // check if the file exists
             if (!System.IO.File.Exists(filePath))
             {
                 return NotFound();
             }

             // read the file as a byte array
             var fileBytes = System.IO.File.ReadAllBytes(filePath);

             // return the file as a FileStreamResult
             return new FileStreamResult(new MemoryStream(fileBytes), "image/png");
         }


         [HttpPost("UploadImage/{id}")]
         [Authorize(Roles = "Admin,Student")]
         public async Task<IActionResult> UploadImage(int id, IFormFile file)
         {
             // full path to file in temp location
             var currentDir = Directory.GetCurrentDirectory();
             string imgPhysicalDir = Path.Combine(currentDir, "wwwroot/img/Projects");
             string imgDBDir = "img/Projects/";

             if (file.Length > 0)
             {
                 //Getting FileName
                 var fileName = id + ".png";
                 var filePath = Path.Combine(imgPhysicalDir, fileName);
                 using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                 {
                     await file.CopyToAsync(fileStream);
                 }

                 // retrieve the project object from the database
                 var project = await _context.Projects.FindAsync(id);

                 // update the img property of the project object
                 project.Img = imgDBDir + fileName;

                 // save the updated project object to the database
                 _context.Entry(project).State = EntityState.Modified;
                 await _context.SaveChangesAsync();

                 return Ok(project);
             }

             return StatusCode(200);
         }*/
        [HttpGet("GetProjectImage/{id}")]
        public ActionResult GetItemPhoto(int id)
        {
            string path = Directory.GetCurrentDirectory();
            string imgDir = Path.Combine(path, "wwwroot/img/Projects");
            string fileName1 = Path.Combine(imgDir, id + ".png");
            string fileName2 = Path.Combine(imgDir, id + ".jpg");
            string fileName3 = Path.Combine(imgDir, id + ".gif");
            string respHeader = "";
            string fileName = "";
            if (System.IO.File.Exists(fileName1))
            {
                respHeader = "image/png";
                fileName = fileName1;
            }
            else if (System.IO.File.Exists(fileName2))
            {
                respHeader = "image/jpeg";
                fileName = fileName2;
            }
            else if (System.IO.File.Exists(fileName3))
            {
                respHeader = "image/gif";
                fileName = fileName3;
            }
            else
            {
                respHeader = "image/png";
                fileName = Path.Combine(imgDir, "default.png"); ;
            }
            return PhysicalFile(fileName, respHeader);
        }
        /* [HttpPost("UploadImageProject/{id}")]
         public async Task<IActionResult> UploadImage(string id, IFormFile file)
         {



             // full path to file in temp location
             var filePath = Directory.GetCurrentDirectory();
             string imgDir = Path.Combine(filePath, "img\\Projects");

             if (file.Length > 0)
             {
                 //Getting FileName
                 var fileName = Path.Combine(imgDir, id + ".png");
                 using (Stream fileStream = new FileStream(fileName, FileMode.Create))
                 {
                     await file.CopyToAsync(fileStream);
                 }

                 return Ok(file);

             }

             return StatusCode(200);
         }*/




        /*[HttpDelete("DeleteImage/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            // retrieve the project object from the database
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            // delete the file from the server
            var filePath = project.Img;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // update the img property of the project object
            project.Img = null;

            // save the updated project object to the database
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(project);
        }*/
        [HttpGet("likescount/{projectID}")]
        ///[Authorize(Roles = "Admin")]
        public IActionResult GetLikesCount(int projectID)
        {
            try
            {
                int count = _projectRepo.GetLikesCount(projectID);
                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while getting the likes count.");
            }
        }



        [HttpGet("WinningProjects")]
        ///[Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<Project>> GetWinningProjects()
        {
            var winningProjects = _context.Projects
                .Where(p => p.IsWinner)
                .ToList();

            return Ok(winningProjects);
        }

        /* [HttpPost("AddNewProject")]
         public async Task<ActionResult<Project>> AddNewProject([FromForm] ProjectDto projectDto, IFormFile file)
         {
             if (_context.Projects == null)
             {
                 return Problem("Entity set 'ProjectDBContext.Projects' is null.");
             }

             var project = new Project
             {
                 TeamName = projectDto.TeamName,
                 TeamNumber = projectDto.TeamNumber,
                 TeamMember = projectDto.TeamMember,
                 ProjectName = projectDto.ProjectName,
                 Introduction = projectDto.Introduction,
                 Description = projectDto.Description,
                 Semester = projectDto.Semester,
                 Img = projectDto.Img,
                 LinktoGit = projectDto.LinktoGit,
                 LinktoYoutube = projectDto.LinktoYoutube,
                 LanguageUsed = projectDto.LanguageUsed,
                 DateCreated = DateTime.Now
             };

             if (file != null && file.Length > 0)
             {
                 //Getting FileName
                 var fileName = $"{project.ProjectID}.png";
                 var imgDir = Path.Combine(Directory.GetCurrentDirectory(), "img/Projects");

                 // Saving the image file
                 using (var fileStream = new FileStream(Path.Combine(imgDir, fileName), FileMode.Create))
                 {
                     await file.CopyToAsync(fileStream);
                 }

                 // Updating the project object with the image path
                 project.Img = $"img/Projects/{fileName}";
             }

             _context.Projects.Add(project);
             await _context.SaveChangesAsync();

             return CreatedAtAction("GetProject", new { id = project.ProjectID }, project);
         }*/


        [HttpPost("AddNewProjectWithImage")]
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> AddNewProjectWithImage([FromForm] ProjectDto projectDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            int ownerId = int.Parse(userIdClaim.Value);
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ProjectDBContext.Projects' is null.");
            }

            var project = new Project
            {
                TeamName = projectDto.TeamName,
                TeamNumber = projectDto.TeamNumber,
                TeamMember = projectDto.TeamMember,
                ProjectName = projectDto.ProjectName,
                ProjectOverView = projectDto.ProjectOverView,
                Semester = projectDto.Semester,
                LinktoGit = projectDto.LinktoGit,
                LinktoYoutube = projectDto.LinktoYoutube,
                LanguageUsed = projectDto.LanguageUsed,
                DateCreated = DateTime.Now,
                OwnerID = ownerId,
            };

            if (User.IsInRole("Admin"))
            {
                project.IsApproved = true; // Set the approval status to true for admin-submitted projects
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            if (projectDto.Img != null && projectDto.Img.Length > 0)
            {
                // full path to file in temp location
                var filePath = Directory.GetCurrentDirectory();
                string imgDir = Path.Combine(filePath, "wwwroot/img/Projects");

                // Getting FileName
                var fileName = Path.Combine(imgDir, project.ProjectID.ToString() + ".png");
                using (Stream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await projectDto.Img.CopyToAsync(fileStream);
                }
            }

            if (User.IsInRole("Admin"))
            {
                return CreatedAtAction("GetProject", new { id = project.ProjectID }, project);
            }
            else
            {
                return Ok("Your project has been submitted and is pending approval from the admin.");
            }
        }


        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUserProjects()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized(); // No UserId claim found
            }

            if (!int.TryParse(userIdClaim.Value, out int ownerId))
            {
                return Unauthorized(); // Invalid UserId claim format
            }

            // Retrieve the projects for the specified user
            var projects = _projectRepo.GetProjectByUserID(ownerId);

            if (projects == null)
            {
                // User not found or no projects found for the user
                return NotFound();
            }

            return Ok(projects);
        }
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            project.IsApproved = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}/decline")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeclineProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("PendingApprovalProjects")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Project>>> GetPendingApprovalProjects()
        {
            var pendingProjects = await _context.Projects.Where(p => !p.IsApproved).ToListAsync();
            return pendingProjects;
        }







        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.ProjectID == id)).GetValueOrDefault();
        }
    }
}

