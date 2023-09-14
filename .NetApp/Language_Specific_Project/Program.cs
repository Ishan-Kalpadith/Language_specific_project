using Db_Manipulate;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Language_Specific_Project.Services;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

//add the appsettings.json as configuration file
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<DataImporter>();
builder.Services.AddScoped<DataAccessService>();

//Allow cross origin resource sharing for the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowLocalhost",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

//validating the JWT token
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddSingleton<IAuthorizationHandler, UserOrAdminHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "AdminPolicy",
        policy => policy.RequireRole(configuration["Authentication:AdminRole"])
    );
    options.AddPolicy(
        "UserPolicy",
        policy => policy.RequireRole(configuration["Authentication:UserRole"])
    );
    options.AddPolicy(
        "UserOrAdminPolicy",
        policy =>
        {
            policy.Requirements.Add(new UserOrAdminRequirement());
        }
    );
});

//Defining services related to APIs
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<EditUserService>();
builder.Services.AddScoped<GetDistanceService>();
builder.Services.AddScoped<SearchUserService>();
builder.Services.AddScoped<GetCustomerListByZipCodeService>();
builder.Services.AddScoped<GetAllCustomerListService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowLocalhost");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
