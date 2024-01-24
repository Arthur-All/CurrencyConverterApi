using CurrencyConversion.Domain.DTOs;

namespace CurrencyConversion.Infra.Interface
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<ExchangeRatesDto>> GetAllCurrencyRateAsync();
        Task<bool> saveCalculation(tempExchangeRatesDto tempExchange);
        Task<RatesDto?> GetCurrencieRate(string currencyFrom, string currencyTo);
        Task<ConvertCurrencyDto> ExchangeRateCalculationTemp(decimal valueFrom, string currencyFrom, string currencyTo);
    }
}
