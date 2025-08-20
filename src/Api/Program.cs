using Microsoft.EntityFrameworkCore;
using InventoryReservation.Infrastructure.Persistence;
using InventoryReservation.Infrastructure.Repositories;
using InventoryReservation.Application.Interfaces;
using MediatR;
using FluentValidation.AspNetCore;

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
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<InventoryReservation.Application.Commands.CreateItem.CreateItemCommandHandler>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();






