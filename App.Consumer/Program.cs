using App.Consumer.Models;
using App.Consumer.Services;
using App.Consumer.Services.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

 builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.Configure<RabbitMQSetting>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<RabbitMQSetting>>().Value);

builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<EmailConfiguration>>().Value);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IRabbitMQConsumer,RabbitMQConsumer>();

builder.Services.AddHostedService<ConsumerHostedService>();

builder.Services.AddTransient<IMessageHandler, MailService>();
builder.Services.AddSingleton<IEmailBodyBuilder,OrderCeateMessageHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.Run();

