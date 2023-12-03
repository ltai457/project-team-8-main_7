using Project_Authentication.Dto;
using Project_Authentication.Model;

namespace Project_Authentication.Data
{
    public interface IAuthenticaionRepo
    {
        int RegisterUser (User user);   
        User? CheckCredential (Login user);
        string GetUserRole(int roleID);
        User? GetUserByUsername(string username);
        
        User GetUserById(int id);
        void DeleteUser(User user);
        User GetUserByEmail(string email);




    }
}
