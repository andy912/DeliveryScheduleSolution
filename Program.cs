using DeliveryScheduleSolution.Services;

var builder = WebApplication.CreateBuilder(args);

// 註冊 MenuService
builder.Services.AddScoped<MenuService>();

// Add services to the container.
builder.Services.AddRazorPages();

//啟用 Controllers (API)
builder.Services.AddControllers();

//啟用 Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromHours(1); });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
// 註冊 API Controllers
app.MapControllers();

//將 Razor Pages 路由註冊
app.MapRazorPages();

//將 API Controllers 路由註冊
app.MapControllers();

// Optional: 測試簡單 GET
app.MapGet("/api/ping", () => "pong");

//啟用 Session
app.UseSession();

app.Run();
