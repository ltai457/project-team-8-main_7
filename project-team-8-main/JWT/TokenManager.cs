using Microsoft.IdentityModel.Tokens;
using Project_Authentication.Model;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Project_Authentication.JWT
{
    public class TokenManager : ITokenManager
    {
        private readonly SymmetricSecurityKey _key;
        public TokenManager(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTKEY:Secret"]));
        }
        /// here we want to read our secret key that we set 
        public string GenerateToken(User user, string roleName )
        {
            var claims = new List<Claim>
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name, user.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("UserId", user.UserID.ToString()),
                new Claim("UserName", user.UserName),



            };/// for checking with our authentication controller 
            var credential = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);// use to generate digital signature
            var Tokendes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(60),/// expiry time of token 
                SigningCredentials = credential,
            };
            var TokenHandler = new JwtSecurityTokenHandler();/// provide two method 1. CreateToken and WriteToken(Return a string value)
            var token = TokenHandler.CreateToken(Tokendes);
            return TokenHandler.WriteToken(token);
        }
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }


}

