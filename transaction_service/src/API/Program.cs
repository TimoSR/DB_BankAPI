using API.EventHandlers.Consumers;
using API.EventHandlers.Publishers;
using API.Features.Application;
using API.Features.Domain;
using API.Features.Infrastructure;
using API.Features.Infrastructure.Repositories;
using CodeContracts.Infrastructure;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using TransactionContext = API.Features.Infrastructure.Contexts.TransactionContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransactionContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("AccountDbContext")));

// Register RabbitMQ service
builder.Services.AddSingleton<RabbitMQService>(serviceProvider => {
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    var logger = serviceProvider.GetRequiredService<ILogger<RabbitMQService>>();
    var rabbitMQConfig = config.GetSection("RabbitMQ");
    var factory = new ConnectionFactory() {
        HostName = rabbitMQConfig.GetValue<string>("HostName"),
        Port = rabbitMQConfig.GetValue<int>("Port"),
        UserName = rabbitMQConfig.GetValue<string>("UserName"),
        Password = rabbitMQConfig.GetValue<string>("Password")
    };
    return new RabbitMQService(factory, logger);
});

builder.Services.AddHostedService<QueueSetupHostedService>();
builder.Services.AddHostedService<TransactionMessageConsumer>();

builder.Services.AddHttpClient("AccountServiceClient", client =>
    {
        // This make it easy to change the version of the client everywhere
        client.BaseAddress = new Uri("https://localhost:7101/api/");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });


// Adding Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

// Domain Event Dispatcher handled by RabbitMQ
builder.Services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddSingleton<TransactionCreatedPublisher>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionFactory, TransactionFactory>();

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
        serviceProvider.GetRequiredService<TransactionContext>()
    };

    foreach (var context in contexts)
    {
        context.Database.EnsureCreated();
    }
}

void DeleteDatabases(IServiceProvider serviceProvider)
{
    var contexts = new List<DbContext> {
        serviceProvider.GetRequiredService<TransactionContext>()
    };

    foreach (var context in contexts)
    {
        context.Database.EnsureDeleted();
    }
}

app.UseCors("MyCorsPolicy");
app.MapControllers();

app.UseHttpsRedirection();

app.Run();  