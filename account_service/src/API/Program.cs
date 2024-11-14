using System.Diagnostics;
using API.EventHandler.Consumers;
using API.EventHandler.Publishers;
using API.Features.Application;
using API.Features.Domain;
using API.Features.Infrastructure;
using API.Features.Infrastructure.Contexts;
using API.Features.Infrastructure.Repositories;
using CodeContracts.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Serilog;
using Serilog.Context;
using Serilog.Events;

// Initial bootstrap logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
  
    // Removing System Detail of log outputs
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
  
    .Enrich.FromLogContext() // Adds context-specific properties automatically
    .Enrich.WithProperty("Application", "AgreementOffer") // Adds a fixed property for filtering
    
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u4}] {Message:lj} (TraceId={TraceId}){NewLine}{Exception}")
    
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
    
    // Middleware to handle Correlation ID, log incoming requests, and measure response time
    app.Use(async (context, next) =>
    {
  
        // Skip middleware for Swagger or base path
        if (context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase) || 
            context.Request.Path == "/" || 
            string.IsNullOrEmpty(context.Request.Path.Value))
        {
            await next.Invoke();
            return;
        }
  
        var stopwatch = Stopwatch.StartNew(); // Start timing the request

        // Generate or retrieve the correlation ID
        var traceId = context.Request.Headers["x-correlation-id"].FirstOrDefault() ?? Guid.NewGuid().ToString();
        context.Items["TraceId"] = traceId;
        LogContext.PushProperty("TraceId", traceId);

        // Construct the request URI
        var requestUri = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        
        try
        {
            
            Log.Information("Received {Method} request for {RequestUri}", 
                context.Request.Method, requestUri);

            await next(); // Process the next middleware
            
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unhandled exception occurred.");
        }
        finally
        {
            // Stop timing and log the response time
            stopwatch.Stop();
            var responseTime = stopwatch.ElapsedMilliseconds;
            
            Log.Information("Processed {Method} request for {RequestUri} in {ResponseTime} ms", 
                context.Request.Method, requestUri, responseTime);
        }
    });
    
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