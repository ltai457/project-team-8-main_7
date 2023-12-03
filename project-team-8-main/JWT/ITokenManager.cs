using Project_Authentication.Model;

namespace Project_Authentication.JWT
{
    public interface ITokenManager
    {
        string GenerateToken(User user,string roleName);
    }
}
