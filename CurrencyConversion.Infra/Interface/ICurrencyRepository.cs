using CurrencyConversion.Domain.DTOs;

namespace CurrencyConversion.Infra.Interface
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<ExchangeRatesDto>> GetAllCurrencyRateAsync();
        Task<RatesDto> GetCurrencieRate(string currencyFrom, string currencyTo);
        Task<bool> saveCalculation(tempExchangeRatesDto tempExchange);
        Task<tempExchangeRatesDto?> checkExchangeRateValidation(string currencyFrom, string currencyTo);
    }
}
