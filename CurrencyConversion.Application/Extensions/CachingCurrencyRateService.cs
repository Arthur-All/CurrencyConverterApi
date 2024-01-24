using CurrencyConversion.Application.Interface;
using CurrencyConversion.Application.Services;
using CurrencyConversion.Domain.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyConversion.Application.Extensions
{
    public class CachingCurrencyRateService : ICurrencyRateService
    {
        private readonly CurrencyRateService _decorated;
        private readonly IMemoryCache _cache;

        public CachingCurrencyRateService(CurrencyRateService decorated, IMemoryCache cache) 
        { 
            _decorated = decorated; 
            _cache = cache; 
        }

        public async Task<decimal> ConvertCurrency(decimal value, string currencyFrom, string currencyTo)
        {
            var cacheKey = $"{currencyFrom}-{currencyTo}";
            decimal outputAmount;

            if(!_cache.TryGetValue(cacheKey, out outputAmount))
            {
                outputAmount = await CalculateConversion(value, currencyFrom, currencyTo);
                await saveCalculation(value, currencyFrom, currencyTo, outputAmount);
                _cache.Set(cacheKey, outputAmount);
            }
            return _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return _decorated.ConvertCurrency(value, currencyFrom, currencyTo);
            });
        }

        public Task<IEnumerable<ExchangeRatesDto>> GetAllCurrencyRateAsync() => _decorated.GetAllCurrencyRateAsync();
    }
}
