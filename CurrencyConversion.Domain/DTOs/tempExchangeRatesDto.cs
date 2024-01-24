namespace CurrencyConversion.Domain.DTOs
{
    public class tempExchangeRatesDto
    {
        public decimal ValueFrom { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal ValueOutPut { get; set; }
        public DateTime Date { get; set; }

    }
}
