﻿using CurrencyConversion.Application.Interface;
using CurrencyConversion.Domain.DTOs;
using CurrencyConversion.Infra.Interface;

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
            try
            {
                var checkExchangeValidation = await _currencyRepo.checkExchangeRateValidation(currencyFrom, currencyTo);

                if (checkExchangeValidation.CurrencyFrom == currencyFrom && checkExchangeValidation.CurrencyTo == currencyTo) return checkExchangeValidation.ValueOutPut;

                var currenciesRate = await _currencyRepo.GetCurrencieRate(currencyFrom, currencyTo);

                var InputToBaseRate = currenciesRate.InputToBaseRate;
                var BaseToOutputRate = currenciesRate.BaseToOutputRate;
                decimal baseAmount = value / InputToBaseRate;
                decimal outputAmount = baseAmount * BaseToOutputRate;

                await saveCalculation(value, currencyFrom, currencyTo, outputAmount);

                return outputAmount;

            }
            catch (Exception ex) { throw ex; }
        }

        private async Task<bool> saveCalculation(decimal valueFrom, string currencyFrom, string currencyTo, decimal outputAmount)
        {
            var tempRate = new tempExchangeRatesDto
            {
                ValueFrom = valueFrom,
                CurrencyFrom = currencyFrom,
                CurrencyTo = currencyTo,
                ValueOutPut = outputAmount,
            };

            return await _currencyRepo.saveCalculation(tempRate);
        }

        private async Task<tempExchangeRatesDto?> checkExchangeRateValidation(string currencyFrom, string currencyTo)
        {
            try
            {
                var success = await _currencyRepo.checkExchangeRateValidation(currencyFrom, currencyTo);

                var checkExchangeRate = new tempExchangeRatesDto
                {
                    ValueFrom = success.ValueFrom,
                    CurrencyFrom = success.CurrencyFrom,
                    CurrencyTo = success.CurrencyTo,
                    ValueOutPut = success.ValueOutPut,
                    Date = success.Date,
                };
                return checkExchangeRate;

            }
            catch (Exception ex) { throw ex; }
        }
    }
}
