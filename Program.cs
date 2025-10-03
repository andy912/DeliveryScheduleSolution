using DeliveryScheduleSolution.Services;

var builder = WebApplication.CreateBuilder(args);

// ���U MenuService
builder.Services.AddScoped<MenuService>();

// Add services to the container.
builder.Services.AddRazorPages();

//�ҥ� Controllers (API)
builder.Services.AddControllers();

//�ҥ� Session
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
// ���U API Controllers
app.MapControllers();

//�N Razor Pages ���ѵ��U
app.MapRazorPages();

//�N API Controllers ���ѵ��U
app.MapControllers();

// Optional: ����²�� GET
app.MapGet("/api/ping", () => "pong");

//�ҥ� Session
app.UseSession();

app.Run();
