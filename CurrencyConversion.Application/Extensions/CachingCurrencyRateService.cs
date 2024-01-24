using CurrencyConversion.Application.Helper;
using CurrencyConversion.Application.Interface;
using CurrencyConversion.Application.Services;
using CurrencyConversion.Domain.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyConversion.Application.Extensions
{
    public class CachingCurrencyRateService : ICurrencyRateService
    {
        //private readonly
        private readonly IMemoryCache _cache;
        private readonly CurrencyRateService _decorated;
        private readonly CurrencyConverterHelper _converterHelper;
        private readonly CurrencyFallbackHelper _currencyFallbackHelper;

        public CachingCurrencyRateService(CurrencyRateService decorated, IMemoryCache cache, CurrencyConverterHelper converterHelper, CurrencyFallbackHelper currencyFallbackHelper)
        {
            _decorated = decorated;
            _cache = cache;
            _converterHelper = converterHelper;
            _currencyFallbackHelper = currencyFallbackHelper;

        }

        public async Task<RatesDto> GetCurrencieRate(string currencyFrom, string currencyTo)
        {
            var cacheKey = $"{currencyFrom}-{currencyTo}";
            RatesDto rates;
            if (!_cache.TryGetValue(cacheKey, out rates))
            {
                rates = await _decorated.GetCurrencieRate(currencyFrom, currencyTo);
                //I need to add the if(null) call=>FallBack
                _cache.Set(cacheKey, rates);
            }
            return await _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                var currenciesRate = _decorated.GetCurrencieRate(currencyFrom, currencyTo);

                return currenciesRate;
            });
        }
        public async Task<decimal> ConvertCurrency(decimal value, string currencyFrom, string currencyTo)
        {
            var cacheKey = $"{currencyFrom}-{currencyTo}";
            decimal outputAmount;

            if (!_cache.TryGetValue(cacheKey, out outputAmount))
            {
                var currenciesRate = await _decorated.GetCurrencieRate(currencyFrom, currencyTo); //old
                var currenciesRate2 = GetCurrencieRate(currencyFrom, currencyTo);
                // Get the exchange rates from the currenciesRate object
                decimal inputToBaseRate = currenciesRate.InputToBaseRate;
                decimal baseToOutputRate = currenciesRate.BaseToOutputRate;

                outputAmount = await _converterHelper.ConvertCurrency(value, inputToBaseRate, baseToOutputRate);
                await _decorated.saveCalculation(value, currencyFrom, currencyTo, outputAmount);
                _cache.Set(cacheKey, outputAmount);
            }
            return await _cache.GetOrCreateAsync(cacheKey, entry =>
            {
                  entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                  return  _decorated.ConvertCurrency(value, currencyFrom, currencyTo);
            });
        }


        public Task<IEnumerable<ExchangeRatesDto>> GetAllCurrencyRateAsync() => _decorated.GetAllCurrencyRateAsync();
    }
}
