using Microsoft.EntityFrameworkCore;
using InventoryReservation.Infrastructure.Persistence;
using InventoryReservation.Infrastructure.Repositories;
using InventoryReservation.Application.Interfaces;
using MediatR;
using FluentValidation.AspNetCore;
using InventoryReservation.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);


// Add services
builder.Services.AddControllers()
    .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<InventoryReservation.Application.Commands.CreateItem.CreateItemCommandValidator>());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext - use environment variable for connection string
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? "Server=localhost;Database=Inventory;Trusted_Connection=True;";
builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(conn));



// DI for repository
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// MediatR: scans Application assembly for handlers
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(InventoryReservation.Application.Commands.CreateItem.CreateItemCommandHandler).Assembly,
    typeof(InventoryReservation.Infrastructure.Persistence.AppDbContext).Assembly
));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();


app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();

app.Run();






