using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pri.Ca.Core.Entities;
using Pri.Ca.Core.Interfaces.Repositories;
using Pri.Ca.Core.Interfaces.Services;
using Pri.Ca.Core.Services;
using Pri.Ca.Infrastructure.Data;
using Pri.Ca.Infrastructure.Repositories;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

//Register dbContext
builder.Services.AddDbContext<ApplicationDbcontext>
    (options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("GamesDb")));

//register IdentityContext
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(
    options =>
    {
        //for testing purposes only!!
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 3;
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    }
    )
    .AddEntityFrameworkStores<ApplicationDbcontext>();
builder.Services.ConfigureApplicationCookie(
    options =>
    {
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    }
    );
//Add authorization
builder.Services.AddAuthorization(options
    =>
{
    //define the policies based on claims
    //add an admin policy
    options.AddPolicy("Admin", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Admin");
    });
    //add user claim
    options.AddPolicy("User", policy =>
    {
        policy.RequireAssertion(context =>
        {
            if (context.User.HasClaim(ClaimTypes.Role, "User")
            || context.User.HasClaim(ClaimTypes.Role, "Admin"))
            {
                return true;
            }
            return false;
        });
    }
    );
    //18+ claim
    options.AddPolicy("18+", policy =>
    {
        policy.RequireAssertion(context => 
        {
            var dateOfbirth = context.User.Claims.FirstOrDefault(c =>
            c.Type.Equals(ClaimTypes.DateOfBirth)).Value;
            var year = DateTime.Parse(dateOfbirth).Year;
            //compare with current year
            if(DateTime.Now.Year - year >= 18)
            {
                return true;
            }
            return false;
        });
    });
});
// Add services to the container.
//register repositories
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//register services
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

//area route
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Account}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
