using Dapper;
using Microsoft.Data.SqlClient;
using DeliveryScheduleSolution.Models;

namespace DeliveryScheduleSolution.Services
{
    public class MenuService
    {
        private readonly string _connectionString;

        public MenuService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<MenuItem>> GetMenusAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            var sql = "SELECT Id, Title, Icon, Url, ParentId FROM Menus ORDER BY ParentId, Id";
            var flatList = (await conn.QueryAsync<MenuItem>(sql)).ToList();

            // 組裝樹狀
            var lookup = flatList.ToDictionary(m => m.Id, m => m);
            var roots = new List<MenuItem>();

            foreach (var item in flatList)
            {
                if (item.ParentId.HasValue && lookup.ContainsKey(item.ParentId.Value))
                {
                    lookup[item.ParentId.Value].Children.Add(item);
                }
                else
                {
                    roots.Add(item);
                }
            }

            return roots;
        }
    }
}
