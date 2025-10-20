using Dapper;
using Microsoft.Data.SqlClient;

namespace DeliveryScheduleSolution.Services
{
    public class RoleService
    {
        private readonly string _connectionString;
        public RoleService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<string>> GetRolesAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT RoleName FROM Roles ORDER BY RoleName";
            return (await conn.QueryAsync<string>(sql)).ToList();
        }
    }
}
