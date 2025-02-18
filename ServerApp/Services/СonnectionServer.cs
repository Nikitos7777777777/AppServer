using Grpc.Core;
using Microsoft.EntityFrameworkCore;
//using NATS.Client.Internals;
using ServerApp.CRUDdb;
using ServerApp.Models;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StackExchange.Redis;

namespace ServerApp.Services
{
    public class СonnectionServer : AuthValid.AuthValidBase
    {
        private CRUDUser _CRUDUser;
        private string _issuer;
        private string _audience;
        private string _secretKey;
        private int _expireMinutes;

        public СonnectionServer(CRUDUser crUDUser)
        {
            _CRUDUser = crUDUser;
            JwtConfig();
        }
        private void JwtConfig()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var JWTConfig = config.GetSection("JwtSettings");
            _issuer = JWTConfig["Issuer"];
            _audience = JWTConfig["Audience"];
            _secretKey = JWTConfig["SecretKey"];
            _expireMinutes = int.Parse(JWTConfig["ExpireMinutes"]);
        }

        public override async Task<AuthorizationResponse> ValidAuthorization(AuthorizationRequest request, ServerCallContext context)
        {
            var users = await _CRUDUser.GetAllUsers();

            var responsError = new AuthorizationResponse
            {
                Permission = "false",
                Token = ""
            };

            foreach (var userItem in users)
            {
                if (request.Username == userItem.Username && request.Userpassword == userItem.PasswordHash)
                {
                    var respons = new AuthorizationResponse
                    {
                        Permission = "true",
                        Token = CreateToken(request.Username)
                    };
                    return respons;
                }
            }
            return responsError;
        }
        private string CreateToken(string username)
        {

            var key = Encoding.UTF8.GetBytes(_secretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
