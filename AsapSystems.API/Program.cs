using System.Text;
using AsapSystems.BLL.Dtos.Settings;
using AsapSystems.BLL.Helpers.Security;
using AsapSystems.BLL.Services.Addresses;
using AsapSystems.BLL.Services.Auth;
using AsapSystems.BLL.Services.Lookups;
using AsapSystems.BLL.Services.Persons;
using AsapSystems.Core;
using AsapSystems.Core.Repositories;
using AsapSystems.Infrastructure;
using AsapSystems.Infrastructure.Context;
using AsapSystems.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IGenderRepository, GenderRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IAddressService, AddressService>();

#region Authentication
builder.Services.Configure<AuthSetting>(builder.Configuration.GetSection("AuthSetting"));

var key = Encoding.ASCII.GetBytes(builder.Configuration[$"{nameof(AuthSetting)}:{nameof(AuthSetting.Jwt)}:{nameof(AuthSetting.Jwt.Secret)}"]);
var issuer = builder.Configuration[$"{nameof(AuthSetting)}:{nameof(AuthSetting.Jwt)}:{nameof(AuthSetting.Jwt.Issuer)}"];

var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = true,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidIssuer = issuer,
    RequireExpirationTime = true,
    ClockSkew = TimeSpan.Zero // remove delay of token when expire
};

builder.Services.AddSingleton(tokenValidationParams);

builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = tokenValidationParams;
        });
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Asap API",
        Description = "Person managment."
    });

    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header.",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});

builder.Services.AddDbContext<AsapContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AsapConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.Run();
