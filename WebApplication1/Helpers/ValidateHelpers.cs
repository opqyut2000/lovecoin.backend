using Dapper;
using System.Data.SqlClient;
using WebApplication1.DbModels;
using WebApplication1.Models;
using WebApplication1.Models.Login;

namespace WebApplication1.Helpers
{
    public class ValidateHelpers
    {
        private readonly IConfiguration _configuration;
        public ValidateHelpers(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TbUser ValidateUser(LoginRequest request)
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
            var conn = new SqlConnection(_configuration[Common.LoveCoin]);

            string where_condition = SqlParam.Any() ? $"where {string.Join(" and ", SqlParam)}" : string.Empty;
            var sql = $"SELECT * FROM tbUser {where_condition}";
            var results = conn.QuerySingleOrDefault<TbUser>(sql, dapperParam);

            return results;
        }
    }
}
