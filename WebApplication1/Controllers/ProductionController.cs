using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
using NuGet.Common;
using System.Data;
using System.Text;
using System.Text.Json;
using WebApplication1.Models;
using WebApplication1.Models.Production;

namespace WebApplication1.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionController : ControllerBase
    {
        private readonly ILogger<ProductionController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        public ProductionController(
            ILogger<ProductionController> logger,
            IConfiguration configuration,
            IDistributedCache cache
        )
        {
            _logger = logger;
            _configuration = configuration;
            _cache = cache;
        }

       
        [HttpGet]
        [Route("GetUser")]
        [Authorize]
        public ActionResult GetUsers()
        {
            var test = User.Identity.Name;

            var redisKey = "Production_GetUser";
            try
            {
                var redisData = _cache.GetString(redisKey);
                if (redisData == null)
                {

                    var conn = new SqlConnection(_configuration[Common.Station]);

                    var sql = "SELECT * FROM tbUser";
                    var results = conn.Query<dynamic>(sql).ToList();

                    var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
                    _cache.SetStringAsync(redisKey, JsonSerializer.Serialize(results), options);

                    return Ok(new { Code = 200, Data = results });
                }
                else
                {
                    var results = JsonSerializer.Deserialize<object>(redisData);
                    return Ok(new { Code = 200, Data = results });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Ok(new { Code = 200, Data = "Error" });
            }
        }

        [HttpGet]
        [Route("GetList")]
        public ActionResult GetList()
        {
            var redisKey = "Production_GetList";
            try
            {
                var redisData = _cache.GetString(redisKey);
                if (redisData == null)
                {

                    var conn = new SqlConnection(_configuration[Common.Station]);

                    var sql = "SELECT * FROM tbProduction";
                    var results = conn.Query<dynamic>(sql).ToList();

                    var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
                    _cache.SetStringAsync(redisKey, JsonSerializer.Serialize(results), options);

                    return Ok(new { Code = 200, Data = results });
                }
                else
                {
                    var results = JsonSerializer.Deserialize<object>(redisData);
                    return Ok(new { Code = 200, Data = results });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Ok(new { Code = 200, Data = "Error" });
            }
        }

        [HttpPost]
        [Route("GetProductionList")]
        public ActionResult GetProductionList(ProductionApiRuqest Request)
        {
            var redisKey = "Production_GetProductionList";
            try
            {
                var redisData = _cache.GetString(redisKey);
                if (redisData == null)
                {
                    var conn = new SqlConnection(_configuration[Common.Station]);

                    var sql = "SELECT * FROM tbProduction";
                    var results = conn.Query<dynamic>(sql).ToList();

                    var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
                    _cache.SetStringAsync(redisKey, JsonSerializer.Serialize(results), options);

                    return Ok(new { Code = 200, Data = results });
                }
                else
                {
                    var results = JsonSerializer.Deserialize<object>(redisData);
                    return Ok(new { Code = 200, Data = results });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Ok(new { Code = 200, Data = "Error" });
            }
        }

        [HttpPost]
        [Route("Update")]
        public ActionResult Update(Production_PModel post)
        {
            var redisKey = "Production_GetList";
            try
            {
                var redisData = _cache.GetString(redisKey);
                if (redisData == null)
                {

                    var conn = new SqlConnection(_configuration[Common.Station]);

                    var sql = "SELECT * FROM tbProduction";
                    var results = conn.Query<dynamic>(sql).ToList();

                    var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
                    _cache.SetStringAsync(redisKey, JsonSerializer.Serialize(results), options);

                    return Ok(new { Code = 200, Data = results });
                }
                else
                {
                    var results = JsonSerializer.Deserialize<object>(redisData);
                    return Ok(new { Code = 200, Data = results });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LogError:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Ok(new { Code = 200, Data = "Error" });
            }
        }
    }
}