using System.Diagnostics;
using System.Security.Cryptography;
using API.EventHandler.Consumers;
using API.EventHandler.Publishers;
using API.Features.Application;
using API.Features.Domain;
using API.Features.Infrastructure;
using API.Features.Infrastructure.Contexts;
using API.Features.Infrastructure.Repositories;
using API.Middleware;
using CodeContracts.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Serilog;
using Serilog.Context;
using Serilog.Events;

//I would like to add a configuration object

// Initial bootstrap logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
  
    // Removing System Detail of log outputs
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
  
    .Enrich.FromLogContext() // Adds context-specific properties automatically
    .Enrich.WithProperty("Application", "AgreementOffer") // Adds a fixed property for filtering
    
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u4}] {Message:lj} (CorrelationId={CorrelationId}){NewLine}{Exception}")
    
    .WriteTo.Seq("http://localhost:5341")
    
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    
    builder.Services.AddDbContext<AccountContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("AccountDbContext")));
    
    // Register RabbitMQ service
    builder.Services.AddSingleton<RabbitMQService>(serviceProvider => {
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        var logger = serviceProvider.GetRequiredService<ILogger<RabbitMQService>>();
        var rabbitMqConfig = config.GetSection("RabbitMQ");
        var factory = new ConnectionFactory() {
            HostName = rabbitMqConfig.GetValue<string>("HostName"),
            Port = rabbitMqConfig.GetValue<int>("Port"),
            UserName = rabbitMqConfig.GetValue<string>("UserName"),
            Password = rabbitMqConfig.GetValue<string>("Password")
        };
        return new RabbitMQService(factory, logger);
    });
    
    builder.Services.AddHostedService<QueueSetupHostedService>();
    builder.Services.AddHostedService<RabbitMQConsumerService>();
    builder.Services.AddHostedService<TransactionMessageConsumer>();
    
    // Adding Mediator
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
    
    // Domain Event Dispatcher handled by RabbitMQ
    builder.Services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
    
    builder.Services.AddTransient<AccountCreatedPublisher>();
    
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<IAccountSecurityDomainService, AccountSecurityService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IAccountFactory, AccountFactory>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("MyCorsPolicy", corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyOrigin() // or specify the allowed origins
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var app = builder.Build();
    
    // Swagger Docs, Database Setup & Delete 

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    if (app.Environment.IsDevelopment())
    {
        // Ensure databases are created at startup during development
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        CreateDatabases(serviceProvider);
    }
    
    if (app.Environment.IsDevelopment())
    {
        app.Lifetime.ApplicationStopping.Register(() =>
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            DeleteDatabases(serviceProvider);
        });
    }
    
    void CreateDatabases(IServiceProvider serviceProvider)
    {
        var contexts = new List<DbContext> {
            serviceProvider.GetRequiredService<AccountContext>()
        };
    
        foreach (var context in contexts)
        {
            context.Database.EnsureCreated();
        }
    }
    
    void DeleteDatabases(IServiceProvider serviceProvider)
    {
        var contexts = new List<DbContext> {
            serviceProvider.GetRequiredService<AccountContext>()
        };
    
        foreach (var context in contexts)
        {
            context.Database.EnsureDeleted();
        }
    }
    
    // Custom Middleware
    
    app.UseMiddleware<CorrelationMiddleware>();
    
    // Built In Middleware
    
    app.UseCors("MyCorsPolicy");
    app.MapControllers();
    app.UseRouting();
    app.UseHttpsRedirection();

    app.Run();  
}
catch (Exception e)
{
    Log.Fatal(e, "Application crashed");
}
finally
{
    Log.CloseAndFlush();
}