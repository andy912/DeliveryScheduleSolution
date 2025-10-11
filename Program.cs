using DeliveryScheduleSolution.Services;

var builder = WebApplication.CreateBuilder(args);

// �[�J MVC Controller �P View
builder.Services.AddControllersWithViews();

// �[�J Razor Pages
builder.Services.AddRazorPages();

// �[�J HttpContextAccessor�]�ѨM _Layout.cshtml �L�k�ϥ� @inject �����D�^
builder.Services.AddHttpContextAccessor();

// �ҥ� Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
});

// ���U Service (DI)
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

// Session �n�b Routing �e�᳣�i�ΡA��ĳ��b�o��
app.UseSession();


app.UseAuthorization();

// �w�]�ɦV�n�J����
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();
