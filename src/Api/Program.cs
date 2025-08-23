using Microsoft.EntityFrameworkCore;
using InventoryReservation.Infrastructure.Persistence;
using InventoryReservation.Infrastructure.Repositories;
using InventoryReservation.Application.Interfaces;
using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using InventoryReservation.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

// Add FluentValidation (updated to non-deprecated approach)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<InventoryReservation.Application.Commands.CreateItem.CreateItemCommandValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? "Server=sqlserver;Database=InventoryDb;User=sa;Password=Password123!;TrustServerCertificate=true;";
builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(conn));

// DI for repository
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(InventoryReservation.Application.Commands.CreateItem.CreateItemCommandHandler).Assembly,
    typeof(InventoryReservation.Infrastructure.Persistence.AppDbContext).Assembly
));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:80", 
                "http://localhost:8080",  // Add this for the new port
                "http://localhost:4200",
                "http://frontend:80"      // Add this for container-to-container
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

// Add health checks
builder.Services.AddHealthChecks();

// Kestrel: Listen on all interfaces inside container
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

var app = builder.Build();

// Middleware
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add health endpoint
app.MapHealthChecks("/health");

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();