using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop_ex.Models;
using Shop_ex.Repositories.CartRepository;
using Shop_ex.Repositories.DBUpdate;
using Shop_ex.Repositories.Home;
using Shop_ex.Repositories.LoginRepository;
using Shop_ex.Repositories.OrderRepository;
using Shop_ex.Repositories.Registration;
using Shop_ex.Repositories.UserRepository;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
//string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
string? connection = builder.Configuration.GetConnectionString("WebConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IDBUpdateRepository, DBUpdateRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/Home/Index");
});



builder.Services.AddMemoryCache();
builder.Services.AddSession();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Cart",
    pattern: "{controller=Cart}/{action=Index}");
app.Run();
