namespace CurrencyConversion.Application
{
    public class Options
    {
        public class CachingOptions
        {
            public bool Enabled { get; set; }
            public TimeSpan Duration { get; set; }
        }
    }
}
