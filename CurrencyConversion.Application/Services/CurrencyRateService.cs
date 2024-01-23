using CurrencyConversion.Application.Interface;
using CurrencyConversion.Domain.DTOs;
using CurrencyConversion.Infra.Interface;
using System.Collections.Generic;

namespace CurrencyConversion.Application.Services
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly ICurrencyRepository _currencyRepo;

        public CurrencyRateService(ICurrencyRepository currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }

        public CurrencyRateService()
        {
        }

        public async Task<IEnumerable<ExchangeRatesDto>> GetAllCurrencyRateAsync()
        {
            return await _currencyRepo.GetAllCurrencyRateAsync();
        }


        //public async Task<IEnumerable<ExchangeRatesDto>> TEST(string currencyFrom, string currencyTo)
        //{
        //    return await _currencyRepo.GetCurrencieRate(currencyFrom, currencyTo);
        //}

        /// <summary>
        /// Converts an input amount from one currency to another using exchange rates.
        /// </summary>
        /// <param name="value">Amount to be converted</param>
        /// <param name="InputToBaseRate">Exchange rate from the input currency to the base currency</param>
        /// <param name="BaseToOutputRate">Exchange rate from the base currency to the output currency</param>
        /// <returns>Converted amount in the output currency</returns>
        public async Task<decimal> ConvertCurrency(decimal value, string currencyFrom, string currencyTo)
        {
            var currenciesRate =  _currencyRepo.GetCurrencieRate(currencyFrom, currencyTo);
            var InputToBaseRate = 1;
            var BaseToOutputRate = 1;
            decimal baseAmount = value / InputToBaseRate;
            decimal outputAmount = baseAmount * BaseToOutputRate;
            return outputAmount;
        }
    }
}
