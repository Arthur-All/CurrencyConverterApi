using CurrencyConversion.Application.Helper;
using CurrencyConversion.Application.Interface;
using CurrencyConversion.Domain.DTOs;
using CurrencyConversion.Infra.Interface;

namespace CurrencyConversion.Application.Services
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly ICurrencyRepository _currencyRepo;
        private readonly CurrencyConverterHelper _converterHelper;

        public CurrencyRateService(ICurrencyRepository currencyRepo, CurrencyConverterHelper converterHelper)
        {
            _currencyRepo = currencyRepo;
            _converterHelper = converterHelper;
            
        }

        public async Task<IEnumerable<ExchangeRatesDto>> GetAllCurrencyRateAsync()
        {
            return await _currencyRepo.GetAllCurrencyRateAsync();
        }
        public async Task<RatesDto> GetCurrencieRate(string currencyFrom, string currencyTo)
        {
           return  await _currencyRepo.GetCurrencieRate(currencyFrom, currencyTo);
        }

        /// <summary>
        /// Converts an input amount from one currency to another using exchange rates.
        /// </summary>
        /// <param name="value">Amount to be converted</param>
        /// <param name="InputToBaseRate">Exchange rate from the input currency to the base currency</param>
        /// <param name="BaseToOutputRate">Exchange rate from the base currency to the output currency</param>
        /// <returns>Converted amount in the output currency</returns>
        public async Task<decimal> ConvertCurrency(decimal value, string currencyFrom, string currencyTo)
        {
            try
            {
                var currenciesRate = await GetCurrencieRate(currencyFrom, currencyTo);

                decimal outputAmount = await _converterHelper.ConvertCurrency(value, currenciesRate.InputToBaseRate, currenciesRate.BaseToOutputRate);

                await saveCalculation(value, currencyFrom, currencyTo, outputAmount);

                return outputAmount;

            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<bool> saveCalculation(decimal valueFrom, string currencyFrom, string currencyTo, decimal outputAmount)
        {
            var TempExchangeRateCalculation = await _currencyRepo.ExchangeRateCalculationTemp(valueFrom, currencyFrom, currencyTo);

            if (!IsSameCurrency(TempExchangeRateCalculation, valueFrom, currencyFrom, currencyTo))
                return true;

            var tempRate = new tempExchangeRatesDto
            {
                ValueFrom = valueFrom,
                CurrencyFrom = currencyFrom,
                CurrencyTo = currencyTo,
                ValueOutPut = outputAmount,
            };

            return await _currencyRepo.saveCalculation(tempRate);
        }
        private bool IsSameCurrency(ConvertCurrencyDto tempExchangeRateCalculation, decimal valueFrom, string currencyFrom, string currencyTo)
        {
            return tempExchangeRateCalculation.Value == valueFrom
                && tempExchangeRateCalculation.CurrencyFrom.ToString() == currencyFrom
                && tempExchangeRateCalculation.CurrencyTo.ToString() == currencyTo;
        }
    }
}
