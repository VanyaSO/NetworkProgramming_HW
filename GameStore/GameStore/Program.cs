using GameStore.Interfaces;
using GameStore.Models;
using GameStore.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IProduct, ProductRepository>();
builder.Services.AddTransient<ICategory, CategoryRepository>();
builder.Services.AddTransient<IOrder, OrderRepository>();
builder.Services.AddTransient<IUser, UserRepository>();

IConfigurationRoot confString = new ConfigurationBuilder().
    SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();


builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(confString.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "GameStore.Session";
    options.IdleTimeout = System.TimeSpan.FromHours(48);
    options.Cookie.HttpOnly = false;
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
