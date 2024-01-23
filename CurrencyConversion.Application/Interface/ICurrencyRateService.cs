using CurrencyConversion.Domain.DTOs;

namespace CurrencyConversion.Application.Interface
{
    public interface ICurrencyRateService
    {
        Task<IEnumerable<ExchangeRatesDto>> GetAllCurrencyRateAsync();
        Task<decimal> ConvertCurrency(decimal value, string currencyFrom, string currencyTo);
    }
}
