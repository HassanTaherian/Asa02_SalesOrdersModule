using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using Services.Abstractions;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IUnitOfWork , RepositoryDbContext>(options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("Asa02SalesOrdersModule")
        );
    }
);

//builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
//builder.Services.AddScoped<IInvoiceItemRepository, InvoiceItemRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();