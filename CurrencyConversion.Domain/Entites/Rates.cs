namespace CurrencyConversion.Domain.Entites
{
    public class Rates
    {
        public int Id { get; set; }
        public decimal CurrencyFrom { get; set; }
        public decimal CurrencyTo { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

    }
}
