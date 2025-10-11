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


        /// <summary>
        /// 根據角色取得對應可見選單（含階層）
        /// </summary>
        public async Task<List<MenuItem>> GetMenusAsync(string role)
        {
            using var conn = new SqlConnection(_connectionString);
            // 這裡同時查詢 Menus 與 Permissions，讓只有該角色有權限的選單才會被載入
            var sql = @"
                SELECT DISTINCT m.Id, m.Title, m.Icon, m.Url, m.ParentId
                FROM Menus m
                INNER JOIN Permissions p ON 
                    (m.Title = p.MenuTitle OR (m.Url IS NOT NULL AND m.Url = p.MenuUrl))
                WHERE p.Role = @Role
                ORDER BY m.ParentId, m.Id;
            ";
            var flatList = (await conn.QueryAsync<MenuItem>(sql, new { Role = role })).ToList();

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
