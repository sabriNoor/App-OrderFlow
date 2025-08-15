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
using App.MVC.DTOs.Order;

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
    builder.Services.Configure<RabbitMQSetting>(builder.Configuration.GetSection("RabbitMQ"));


    builder.Services.AddSingleton(sp =>
        sp.GetRequiredService<IOptions<JwtSettings>>().Value);
    builder.Services.AddSingleton(resolver =>
        resolver.GetRequiredService<IOptions<RabbitMQSetting>>().Value);

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

    // RabbitMq
    builder.Services.AddSingleton(typeof(IRabbitMQPublisher<>), typeof(RabbitMQPublisher<>));

    // Auth DI
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IJwt, Jwt>();
    // Product DI
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();
    // Order DI
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IOrderService, OrderService>();



    builder.Services.AddAuthorization();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();

    builder.Services.AddDbContext<ApplicationDBContext>(options =>
         options
         .UseSqlServer("Server=sqlserver,1433;Database=AppMVCDb;User Id=sa;Password=Abc@123456;TrustServerCertificate=true;"
         , sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)));

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
    var publisher = app.Services.GetRequiredService<IRabbitMQPublisher<OrderCreatedMessageDTO>>();
    await publisher.InitializeAsync();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        db.Database.Migrate();
        SeedData.SeedAdmin(db);
    }
    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);

}
