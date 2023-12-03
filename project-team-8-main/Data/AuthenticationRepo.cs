using Microsoft.EntityFrameworkCore;
using Project_Authentication.Dto;
using Project_Authentication.Model;

namespace Project_Authentication.Data
{
    public class AuthenticationRepo : IAuthenticaionRepo
    {
        private readonly ProjectDBContext _dbcontext;
        public AuthenticationRepo(ProjectDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public int RegisterUser(User user)
        {
            try
            {
                _dbcontext.Users.Add(user);
                return _dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
                return -1;
            }
        }

        public User CheckCredential(Login user)
        {
            var credential = _dbcontext.Users.FirstOrDefault(p => p.Email == user.Email);
            return credential;
        }
        public string GetUserRole(int roleID)
        {
            return _dbcontext.Roles.SingleOrDefault(role => role.RoleID == roleID).RoleName;
        }
        public User? GetUserByUsername(string username)
        {
            return _dbcontext.Set<User>().SingleOrDefault(u => u.UserName == username);
        }
        public User GetUserById(int id)
        {
            return _dbcontext.Users.FirstOrDefault(u => u.UserID == id);
        }

        public void DeleteUser(User user)
        {
            _dbcontext.Users.Remove(user);
            _dbcontext.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            return _dbcontext.Users.FirstOrDefault(u => u.Email == email);
        }



    }
}
