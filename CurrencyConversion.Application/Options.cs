namespace CurrencyConversion.Application
{
    public class Options
    {
        public class CachingOptions
        {
            public bool Enabled { get; set; }
            public TimeSpan Duration { get; set; }
        }
        public class FallbackOptions
        {
            public string backupFilePath { get; set; }
        }
        public class WebApiOptions
        {
            public string WebApiUrl { get; set; }
        }
    }
}
