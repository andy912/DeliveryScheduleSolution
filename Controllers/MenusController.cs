using DeliveryScheduleSolution.Services;
using Microsoft.AspNetCore.Mvc;


namespace DeliveryScheduleSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {        
        private readonly MenuService _menuService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenusController(MenuService menuService, IHttpContextAccessor httpContextAccessor)
        {
            _menuService = menuService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetMenus()
        {
            string? username = _httpContextAccessor.HttpContext?.Session.GetString("Username");
            string? role = _httpContextAccessor.HttpContext?.Session.GetString("Role");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(role))
                return Unauthorized();

            var menus = await _menuService.GetMenusAsync(role);
            return Ok(menus);
        }
    }
}
