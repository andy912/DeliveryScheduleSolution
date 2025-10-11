using DeliveryScheduleSolution.Services;

var builder = WebApplication.CreateBuilder(args);

// 加入 MVC Controller 與 View
builder.Services.AddControllersWithViews();

// 加入 Razor Pages
builder.Services.AddRazorPages();

// 加入 HttpContextAccessor（解決 _Layout.cshtml 無法使用 @inject 的問題）
builder.Services.AddHttpContextAccessor();

// 啟用 Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
});

// 註冊 Service (DI)
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<RoleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

// Session 要在 Routing 前後都可用，建議放在這裡
app.UseSession();


app.UseAuthorization();

// 預設導向登入頁面
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();
