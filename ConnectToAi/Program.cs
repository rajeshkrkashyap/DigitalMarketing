using Core.Shared;
using Core.Shared.Entities;
using ConnectToAi.Recharges;
using ConnectToAi.Services;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("/ModuleArea/{2}/Views/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/ModuleArea/{2}/Views/Shared/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

builder.Services.Configure<Settings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSession(options =>
{
    // Configure session options as needed
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<HttpContextAccessor>();

builder.Services.AddSingleton<BlobStorageService>();
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<OcrService>();
builder.Services.AddScoped<BaseService>();
builder.Services.AddScoped<AppService>();
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<OrderViewModel>();
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddSingleton<ServiceService>();
builder.Services.AddSingleton<RechargeService>();
builder.Services.AddSingleton<AppUserService>();
builder.Services.AddSingleton<ArticleTypeService>();
builder.Services.AddSingleton<BlogService>();
builder.Services.AddSingleton<CompetitorArticleService>();

var app = builder.Build();

var customConnectSources = new List<string>
{
    "https://*.signalr.net" // example
};
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    // make exceptions to allow hot reload to work
    customConnectSources.Add("ws://localhost:*");
    customConnectSources.Add("wss://localhost:*");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "ModuleArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.UseMvc(routes =>
{    //Other routes
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();