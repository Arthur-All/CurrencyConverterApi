using CurrencyConversion.Application;
using CurrencyConversion.Application.Extensions;
using CurrencyConversion.Application.Helper;
using CurrencyConversion.Application.Interface;
using CurrencyConversion.Application.Services;
using CurrencyConversion.Infra.Context;
using CurrencyConversion.Infra.Interface;
using CurrencyConversion.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using static CurrencyConversion.Application.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.\
builder.Services.AddScoped<Options>();
builder.Services.AddScoped<CurrencyRateService>();
builder.Services.AddScoped<CurrencyFallbackHelper>();
builder.Services.AddScoped<CurrencyConverterHelper>();
builder.Services.AddScoped<CachingCurrencyRateService>();
builder.Services.AddScoped<ICurrencyRateService, CachingCurrencyRateService>();

builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

builder.Services.Configure<WebApiOptions>(configuration.GetSection("WebApi"));
builder.Services.Configure<CachingOptions>(configuration.GetSection("Caching"));
builder.Services.Configure<FallbackOptions>(configuration.GetSection("Fallback"));
builder.Services.AddMemoryCache();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<AppDbContext>(opt => opt
                              .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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
