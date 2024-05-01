using API.Features.CreateAccount.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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