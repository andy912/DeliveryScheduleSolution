using DeliveryScheduleSolution.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DeliveryScheduleSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly MenuService _menuService;

        public TestController(MenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// 測試讀取 Menus 資料
        /// </summary>
        [HttpGet("menus")]
        public async Task<IActionResult> GetMenus()
        {
            try
            {
                var menus = await _menuService.GetMenusAsync();
                if (menus == null || !menus.Any())
                {
                    return NotFound("⚠️ 資料表 Menus 沒有資料");
                }
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ 錯誤：{ex.Message}");
            }
        }

    }
}
