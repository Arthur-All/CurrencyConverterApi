namespace CurrencyConversion.Domain.Entites
{
    public class ExchangeRates
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }

    }
}
