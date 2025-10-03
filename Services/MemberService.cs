using Dapper;
using Microsoft.Data.SqlClient;
using DeliveryScheduleSolution.Models;

namespace DeliveryScheduleSolution.Services
{
    public class MemberService
    {
        private readonly string _connectionString;

        public MemberService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<Member?> ValidateLoginAsync(string username, string password)
        {
            using var conn = new SqlConnection(_connectionString);
            string passwordHash = ComputeSha256Hash(password);

            var sql = @"SELECT MemberId, Username, FullName, Role
                        FROM Members
                        WHERE Username=@Username AND PasswordHash=@PasswordHash";

            return await conn.QueryFirstOrDefaultAsync<Member>(sql, new { Username = username, PasswordHash = passwordHash });
        }

        public async Task<List<Permission>> GetPermissionsByRoleAsync(string role)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = @"SELECT MenuTitle, MenuUrl
                        FROM Permissions
                        WHERE Role=@Role";

            return (await conn.QueryAsync<Permission>(sql, new { Role = role })).ToList();
        }

        private string ComputeSha256Hash(string rawData)
        {
            using var sha256Hash = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));
            var builder = new System.Text.StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }

    }
}
