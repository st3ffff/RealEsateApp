using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Data;
using RealEstateApp.Models;
using RealEstateApp.Services;

var builder = WebApplication.CreateBuilder(args);

// SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HouseRentingDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity с Guid
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<HouseRentingDbContext>()
.AddDefaultTokenProviders();

// Services
builder.Services.AddScoped<HouseService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

var app = builder.Build();

// Middleware
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Houses}/{action=All}/{id?}");
app.MapRazorPages();

// Seed roles & users
await SeedRolesAsync(app);
app.Run();


// Създаване на роли и потребители
async Task SeedRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Admin", "Broker", "PrivateUser" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = role });
        }
    }

    // Admin user
    var adminEmail = "admin@site.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    // Broker user
    var brokerEmail = "broker@site.com";
    var brokerUser = await userManager.FindByEmailAsync(brokerEmail);
    if (brokerUser == null)
    {
        brokerUser = new ApplicationUser { UserName = brokerEmail, Email = brokerEmail };
        await userManager.CreateAsync(brokerUser, "Broker123!");
        await userManager.AddToRoleAsync(brokerUser, "Broker");
    }
}
