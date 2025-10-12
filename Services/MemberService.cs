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

            var sql = @"SELECT MemberId, Username, Role
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

        public async Task<List<Member>> GetMembersAsync(string? name)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT MemberId, Username, Role FROM Members";
            if (!string.IsNullOrEmpty(name))
                sql += " WHERE Username LIKE @name";
            return (await conn.QueryAsync<Member>(sql, new { name = $"%{name}%" })).ToList();
        }

        public async Task UpdateRoleAsync(int id, string role)
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "UPDATE Members SET Role = @role WHERE MemberId = @id";
            await conn.ExecuteAsync(sql, new { id, role });
        }

        // 新增會員
        public async Task AddMemberAsync(Member member)
        {
            using var conn = new SqlConnection(_connectionString);

            if (member == null)
                throw new ArgumentNullException(nameof(member));

            // 將密碼加密成 SHA256
            member.PasswordHash = ComputeSha256Hash(member.Password); // 假設 Member 有 PasswordHash 欄位

            // 檢查帳號是否已存在
            var exists = await conn.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Members WHERE Username = @Username",
                new { member.Username });

            if (exists > 0)
                throw new InvalidOperationException("使用者帳號已存在");

            // 新增會員
            var sql = @"INSERT INTO Members (Username, PasswordHash, Role)
                    VALUES (@Username, @PasswordHash, @Role)";
            await conn.ExecuteAsync(sql, member);
        }

    }
}
