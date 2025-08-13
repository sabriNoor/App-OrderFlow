using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using App.MVC.Data;
using App.MVC.Entities;
using App.MVC.Services.Interfaces;
using App.MVC.Services;
using App.MVC.Helpers;
using App.MVC.Helpers.Interfaces;
using App.MVC.Repositories;
using App.MVC.Repositories.Interfaces;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

    var secretKey = builder.Configuration.GetSection("JWT")["Key"];
    if (string.IsNullOrWhiteSpace(secretKey))
    {
        throw new Exception("JWT SecretKey is not configured.");
    }

    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

    builder.Services.AddSingleton(sp =>
        sp.GetRequiredService<IOptions<JwtSettings>>().Value);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    secretKey
                ))
            };
        });

    // Auth DI
    builder.Services.AddScoped<IAuthRepository,AuthRepository>();
    builder.Services.AddScoped<IAuthService,AuthService>();
    builder.Services.AddScoped<IJwt,Jwt>();
    // Product DI
    builder.Services.AddScoped<IProductRepository,ProductRepository>();
    builder.Services.AddScoped<IProductService,ProductService>();

    builder.Services.AddAuthorization();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();

    builder.Services.AddDbContext<ApplicationDBContext>(options =>
         options
         .UseSqlServer("Server=DESKTOP-VPBMQ65;Database=StudentManagerDb;Trusted_connection=true;TrustServerCertificate=true;"));

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();


    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);

}
