using CurrencyConversion.Domain.DTOs;
using CurrencyConversion.Domain.Entites;
using CurrencyConversion.Infra.Context;
using CurrencyConversion.Infra.Interface;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CurrencyConversion.Infra.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        #region Dapper
        private const string GetAllCurrency = @"SELECT Rate FROM ExchangeRates;";
        private const string GetCurrenciesRates = @"
                                                    SELECT Rate
                                                    FROM ExchangeRates
                                                    WHERE Currency in (@currencyFrom, @currencyTo)
                                                    ORDER BY CASE Currency
                                                        WHEN @currencyFrom THEN 1
                                                        WHEN @currencyTo THEN 2
                                                        ELSE 3
                                                    END;";
        private readonly string ExchangeRateValidation = @"SELECT  ValueFrom, CurrencyFrom, CurrencyTo, ValueOutPut, Date FROM Rates WHERE CurrencyFrom = @CurrencyFrom AND CurrencyTo = @CurrencyTo;";
        #endregion

        private readonly IDbConnection _connection;
        private readonly AppDbContext _context;
        public CurrencyRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connection = _connection.AddConnection(configuration);
        }

        public async Task<IEnumerable<ExchangeRatesDto>> GetAllCurrencyRateAsync()
        {
            return await _connection.QueryAsync<ExchangeRatesDto>(GetAllCurrency);
        }
        public async Task<RatesDto> GetCurrencieRate(string currencyFrom, string currencyTo)
        {
            var ratesList = await _connection.QueryAsync<decimal>(GetCurrenciesRates, new { currencyFrom, currencyTo });

            // Assuming ratesList contains two decimal values
            var ratesDto = new RatesDto
            {
                InputToBaseRate = ratesList.ElementAtOrDefault(0),
                BaseToOutputRate = ratesList.ElementAtOrDefault(1)
            };

            return ratesDto;
        }
        public async Task<bool> saveCalculation(tempExchangeRatesDto tempExchange)
        {
            var tempRate = new Rates
            {
                ValueFrom = tempExchange.ValueFrom,
                CurrencyFrom = tempExchange.CurrencyFrom,
                CurrencyTo = tempExchange.CurrencyTo,
                ValueOutPut = tempExchange.ValueOutPut,
                Date = DateTime.Now
            };
            _context.Rates.Add(tempRate);
            var success = await _context.SaveChangesAsync();
            return success > 0;
        }
        public async Task<tempExchangeRatesDto?> checkExchangeRateValidation(string currencyFrom, string currencyTo)
        {

            var success =  await _connection.QueryFirstOrDefaultAsync<tempExchangeRatesDto?>(ExchangeRateValidation, new { currencyFrom, currencyTo });
            return success;

        }
    }
}
