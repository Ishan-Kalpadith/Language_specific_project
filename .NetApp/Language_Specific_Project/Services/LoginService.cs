using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Language_Specific_Project.Services
{
    public class LoginService
    {
        private readonly IConfiguration _configuration;

        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string token, string role) AuthenticateUser(string Username, string Password)
        {
            var admin = _configuration["Authentication:AdminUsername"];
            var adminpw = _configuration["Authentication:AdminPassword"];
            var user = _configuration["Authentication:UserUsername"];
            var userpw = _configuration["Authentication:UserPassword"];
            string validAdminUsername = admin;
            string validAdminPassword = adminpw;
            string validUsername = user;
            string validPassword = userpw;

            if (
                (Username == validAdminUsername && Password == validAdminPassword)
                || (Username == validUsername && Password == validPassword)
            )
            {
                string role = (Username == validAdminUsername) ? "admin" : "user";
                var token = GenerateJwtToken(Username, role);

                return (token, role);
            }

            return (null, null);
        }

        private string GenerateJwtToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            };
            var jwtKey = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var audience = _configuration["Jwt:Audience"];
            var issuer = _configuration["Jwt:Issuer"];
            var expires = _configuration["Jwt:ExpirationHours"];

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToDouble(expires)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
