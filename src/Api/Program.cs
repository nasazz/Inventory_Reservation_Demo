using Microsoft.EntityFrameworkCore;
using InventoryReservation.Infrastructure.Persistence;
using InventoryReservation.Infrastructure.Repositories;
using InventoryReservation.Application.Interfaces;
using MediatR;
using FluentValidation.AspNetCore;
using InventoryReservation.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers()
    .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<InventoryReservation.Application.Commands.CreateItem.CreateItemCommandValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? "Server=localhost;Database=Inventory;Trusted_Connection=True;";
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
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Kestrel: Listen on all interfaces inside container
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

var app = builder.Build();

// Middleware
app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Disable HTTPS redirection in Docker/dev
// app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
