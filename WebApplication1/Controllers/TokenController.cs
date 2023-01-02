using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using WebApplication1.DbModels;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.Models.Login;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly JwtHelpers jwt;
        private readonly ValidateHelpers _validateHelpers;

        public TokenController(JwtHelpers jwt
            ,ValidateHelpers validateHelpers)
        {
            this.jwt = jwt;
            _validateHelpers = validateHelpers;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public ActionResult<object> SignIn(LoginRequest request)
        {
            if (request is null)
            {
                return BadRequest("Invalid user request!!!");
            }
            var userModel = _validateHelpers.ValidateUser(request);
            if (userModel != null)
            {
                return jwt.GenerateToken(userModel);
            }
            else
            {
                return BadRequest("Invalid user request!!!");
            }
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
            return Ok(new { Code = 200, Data = Userid });
        }

        [HttpGet("jwtid")]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }
    }
}
