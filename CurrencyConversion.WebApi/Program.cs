using CurrencyConversion.Application.Interface;
using CurrencyConversion.Application.Services;
using CurrencyConversion.Infra.Context;
using CurrencyConversion.Infra.Interface;
using CurrencyConversion.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<AppDbContext>(opt => opt
                              .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

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
