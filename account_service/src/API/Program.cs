using API.Features.Application;
using API.Features.Application.Eventhandlers;
using API.Features.Domain;
using API.Features.Infrastructure;
using API.Features.Infrastructure.Contexts;
using API.Features.Infrastructure.Repositories;
using CodeContracts.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AccountContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("AccountDbContext")));

// Register RabbitMQ service
builder.Services.AddSingleton<RabbitMQService>(serviceProvider => 
    new RabbitMQService(serviceProvider.GetRequiredService<IConfiguration>()));

// Adding Mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

// Domain Event Dispatcher handled by RabbitMQ
builder.Services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();

builder.Services.AddTransient<AccountCreatedHandler>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountSecurityDomainService, AccountSecurityService>();
builder.Services.AddScoped<IAccountService, AccountService>();

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


// Settings for RabbitMQ
var rabbitMqService = app.Services.GetRequiredService<RabbitMQService>();

rabbitMqService.SetupQueue("accountEvents");

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

app.UseHttpsRedirection();

app.Run();  