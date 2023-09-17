using DatabaseConfigClassLibrary;
using DatabaseConfigClassLibrary.DTO;
using DatabaseConfigClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CustomerDetailsManagementApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

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

// API versioning configurations
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
});

//Defining services related to APIs
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<EditUserService>();
builder.Services.AddScoped<GetDistanceService>();
builder.Services.AddScoped<SearchUserService>();
builder.Services.AddScoped<GetCustomerListByZipCodeService>();
builder.Services.AddScoped<GetAllCustomerListService>();

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<UserUpdateDTO, UserData>()
       .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    cfg.CreateMap<UserDTO, UserData>();
    cfg.CreateMap<UserData, UserDTO>();
    cfg.CreateMap<AddressDetails, AddressData>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

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