using App.MVC.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDBContext>(options =>
        options
        .UseSqlServer("Server=DESKTOP-VPBMQ65;Database=MVCDb;Trusted_connection=true;TrustServerCertificate=true;")
        );
builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

