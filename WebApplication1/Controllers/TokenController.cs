using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using WebApplication1.DAO;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly JwtHelpers jwt;
        private readonly IConfiguration _configuration;

        public TokenController(JwtHelpers jwt, IConfiguration configuration)
        {
            this.jwt = jwt;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public ActionResult<object> SignIn(LoginModel request)
        {
            if (request is null)
            {
                return BadRequest("Invalid user request!!!");
            }
            var userModel = ValidateUser(request);
            if (userModel!=null)
            {
                return jwt.GenerateToken(userModel);
            }
            else
            {
                return BadRequest("Invalid user request!!!");
            }
        }

        private tbUser ValidateUser(LoginModel request)
        {
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
            var results = conn.QuerySingleOrDefault<tbUser>(sql, dapperParam);

            return results;
        }

        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        [HttpGet("username")]
        public IActionResult GetUserName()
        {
            var Userid = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value ?? "";
            var Username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value ?? "";
            return Ok(Userid);
        }

        [HttpGet("jwtid")]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }
    }
    public class LoginModel
    {
        public string Userid { get; set; }
        public string Password { get; set; }
    }
}
