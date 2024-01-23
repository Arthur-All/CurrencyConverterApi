using CurrencyConversion.Domain.DTOs;
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
        private const string GetAllCurrency = @"SELECT Rate FROM ExchangeRates";
        private const string GetCurrenciesRates = @"
                                                SELECT * FROM ExchangeRates 
                                                WHERE Currency in (@currencyFrom, @currencyTo) 
                                                ORDER BY CASE Currency
                                                    WHEN @currencyFrom THEN 1
                                                    WHEN @currencyTo THEN 2
                                                    ELSE 3
                                                END;"; 
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
            var t = (RatesDto)await _connection.QueryAsync<RatesDto>(GetCurrenciesRates, new { currencyFrom, currencyTo });
            return t;    
        }
    }
}
