﻿namespace CurrencyConversion.Domain.Entites
{
    public class Rates
    {
        public int Id { get; set; }
        public decimal ValueFrom { get; set; }
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal ValueOutPut { get; set; }
        public DateTime Date { get; set; }

    }
}
