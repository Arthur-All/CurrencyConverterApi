using CurrencyConversion.Domain.DTOs;
using CurrencyConversion.Infra.Interface;
using Microsoft.Extensions.Options;
using System.Text.Json;
using static CurrencyConversion.Application.Options;

namespace CurrencyConversion.Application.Helper
{
    public class CurrencyFallbackHelper
    {
        private readonly ICurrencyRepository _currencyRepo;
        private readonly FallbackOptions _fallbackOptions;
        private readonly WebApiOptions _webApiOptions;

        public CurrencyFallbackHelper(ICurrencyRepository currencyRepo, IOptions<FallbackOptions> fallbackOptions, IOptions<WebApiOptions> webApiOptions)
        {
            _currencyRepo = currencyRepo;
            _fallbackOptions = fallbackOptions.Value;
            _webApiOptions = webApiOptions.Value;
        }

        public async Task BackupCurrencyRates()
        {
            var allCurrencyRates = await _currencyRepo.GetAllCurrencyRateAsync();
            var json = JsonSerializer.Serialize(allCurrencyRates);
            await File.WriteAllTextAsync(_fallbackOptions.backupFilePath, json);
        }

        public async Task<RatesDto?> GetBackupCurrencyRate(string currencyFrom, string currencyTo)
        {

            var json = await File.ReadAllTextAsync(_fallbackOptions.backupFilePath);
            var allCurrencyRates = JsonSerializer.Deserialize<List<ExchangeRatesDto>>(json);

            var rateFrom = allCurrencyRates?.FirstOrDefault(r => r.Currency == currencyFrom)?.Rate;
            var rateTo = allCurrencyRates?.FirstOrDefault(r => r.Currency == currencyTo)?.Rate;
            return new RatesDto
            {
                InputToBaseRate = rateFrom.Value,
                BaseToOutputRate = rateTo.Value
            };
        }
    }
}
