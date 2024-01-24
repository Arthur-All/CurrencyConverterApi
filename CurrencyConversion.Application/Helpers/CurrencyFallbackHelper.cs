using CurrencyConversion.Domain.DTOs;
using CurrencyConversion.Infra.Interface;

namespace CurrencyConversion.Application.Helper
{
    public class CurrencyFallbackHelper
    {
        private readonly ICurrencyRepository _currencyRepo;

        public CurrencyFallbackHelper(ICurrencyRepository currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }

        public async Task<RatesDto?> GetFallbackCurrencieRate(string currencyFrom, string currencyTo)
        {
            var currencieRate = await _currencyRepo.GetCurrencieRate(currencyFrom, currencyTo);

            if (currencieRate == null) return null;

            return currencieRate;
        }
    }
}
