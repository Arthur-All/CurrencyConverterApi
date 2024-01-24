using CurrencyConversion.Domain.Enum;

namespace CurrencyConversion.Domain.DTOs
{
    public class ConvertCurrencyDto
    {
        public decimal Value { get; set; }
        public CurrenciesEnum CurrencyFrom { get; set; }
        public CurrenciesEnum CurrencyTo { get; set; }
    }
}
