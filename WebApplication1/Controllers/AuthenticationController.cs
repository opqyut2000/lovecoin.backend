using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthenticationController(
           IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        [HttpPost("token")]
        public IActionResult Login([FromBody] User user)
        {
            if (user is null)
            {
                return BadRequest("Invalid user request!!!");
            }
            var dapperParam = new DynamicParameters();
            var SqlParam = new List<string>();
            if (user.Userid != null)
            {
                SqlParam.Add("userid = @userid");
                dapperParam.Add("userid", user.Userid);
            }
            if (user.Password != null)
            {
                SqlParam.Add("password = @password");
                dapperParam.Add("password", user.Password);
            }
            var conn = new SqlConnection(_configuration[Common.Station]);

            string where_condition = SqlParam.Any() ? $"where {string.Join(" and ", SqlParam)}" : string.Empty;
            var sql = $"SELECT * FROM tbUser {where_condition}";
            var results = conn.Query<User>(sql, dapperParam).ToList();

            if (user.Userid == "Jaydeep" && user.Password == "Pass@777")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new JWTTokenResponse { Token = tokenString });
            }
            return Unauthorized();
        }
    }
}
