using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pri.Ca.Core.Entities;
using Pri.Ca.Core.Interfaces.Repositories;
using Pri.Ca.Core.Interfaces.Services;
using Pri.Ca.Core.Services;
using Pri.Ca.Infrastructure.Data;
using Pri.Ca.Infrastructure.Repositories;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Register dbContext
builder.Services.AddDbContext<ApplicationDbcontext>
    (options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("GamesDb")));
builder.Services.AddCors();
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options =>
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
//add JWT bearer service
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer
(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateAudience = true,
    ValidateIssuer = true,
    RequireExpirationTime = true,
    ValidAudience = builder.Configuration.GetValue<string>("JWTConfiguration:Audience"),
    ValidIssuer = builder.Configuration.GetValue<string>("JWTConfiguration:Issuer"),
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
    .GetBytes(builder.Configuration.GetValue<string>("JWTConfiguration:Secret")))
});
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
            if (DateTime.Now.Year - year >= 18)
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
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IImageService, ImageService>();
//register automapper service
builder.Services.AddAutoMapper(typeof(Program));
//register HttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
