using DeliveryScheduleSolution.Services;
using Microsoft.AspNetCore.Mvc;


namespace DeliveryScheduleSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly MenuService _menuService;

        public MenusController(MenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMenus()
        {
            var menus = await _menuService.GetMenusAsync();
            return Ok(menus);
        }
    }
}
