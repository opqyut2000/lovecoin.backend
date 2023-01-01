using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DbModels;
using WebApplication1.Models;
using WebApplication1.Models.Authentication;

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

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Login([FromBody] Login request)
        {
            if (request is null)
            {
                return BadRequest("Invalid user request!!!");
            }
            var dapperParam = new DynamicParameters();
            var SqlParam = new List<string>();
            if (request.Userid != null)
            {
                SqlParam.Add("userid = @userid");
                dapperParam.Add("userid", request.Userid);
            }
            if (request.Password != null)
            {
                SqlParam.Add("password = @password");
                dapperParam.Add("password", request.Password);
            }
            var conn = new SqlConnection(_configuration[Common.Station]);

            string where_condition = SqlParam.Any() ? $"where {string.Join(" and ", SqlParam)}" : string.Empty;
            var sql = $"SELECT * FROM tbUser {where_condition}";
            var results = conn.Query<TbUser>(sql, dapperParam).ToList();

            if (results.Count > 0)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new JWTTokenResponse { Token = tokenString });
            }
            return Unauthorized();
        }
    }
}
