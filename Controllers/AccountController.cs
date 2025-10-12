using DeliveryScheduleSolution.Models;
using DeliveryScheduleSolution.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryScheduleSolution.Controllers
{
    public class AccountController : Controller
    {
        private readonly MemberService _memberService;
        private readonly RoleService _roleService;

        public AccountController(RoleService roleService, MemberService memberService)
        {
            _roleService = roleService;
            _memberService = memberService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // 對應 Views/Account/Login.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var member = await _memberService.ValidateLoginAsync(username, password);
            if (member != null)
            {
                HttpContext.Session.SetString("Username", member.Username);
                HttpContext.Session.SetString("Role", member.Role);
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

        // 角色管理首頁
        [HttpGet]
        public async Task<IActionResult> ManageRoles(string searchName)
        {
            var members = await _memberService.GetMembersAsync(searchName);
            var roles = await _roleService.GetRolesAsync();
            ViewBag.Roles = roles;

            if (string.IsNullOrEmpty(searchName))
            {
                // 沒有搜尋，不回傳任何會員
                ViewBag.HasSearched = false;
                return View(new List<Member>());
            }
            else {
                ViewBag.HasSearched = true;
                return View(members);
            }
                
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(int id, string role)
        {
            // 呼叫 Service 更新資料庫
            await _memberService.UpdateRoleAsync(id, role);

            TempData["Message"] = "角色更新成功";
            return RedirectToAction("ManageRoles");
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string username, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(role))
            {
                TempData["Message"] = "帳號與角色不可為空";
                return RedirectToAction("ManageRoles");
            }

            // 隨機產生密碼
            string password = GenerateRandomPassword(8);

            // 建立會員物件
            var newMember = new Member
            {
                Username = username,
                Role = role,
                Password = password // 假設 Member 有 Password 欄位
            };

            await _memberService.AddMemberAsync(newMember);

            // 將密碼存到 TempData 顯示
            TempData["NewPassword"] = password;
            TempData["Message"] = $"新增使用者 {username} 成功";

            return RedirectToAction("ManageRoles");
        }

        // 隨機產生密碼方法
        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
