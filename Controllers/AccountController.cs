using Microsoft.AspNetCore.Mvc;
using DeliveryScheduleSolution.Services;
using DeliveryScheduleSolution.Models;

namespace DeliveryScheduleSolution.Controllers
{
    public class AccountController : Controller
    {
        private readonly MemberService _memberService;

        public AccountController(MemberService memberService)
        {
            _memberService = memberService;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var member = await _memberService.ValidateLoginAsync(username, password);
            if (member != null)
            {
                HttpContext.Session.SetString("Username", member.Username);
                HttpContext.Session.SetString("Role", member.Role);
                HttpContext.Session.SetString("FullName", member.FullName);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "帳號或密碼錯誤";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
