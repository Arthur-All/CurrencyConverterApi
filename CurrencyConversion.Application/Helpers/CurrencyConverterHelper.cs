namespace CurrencyConversion.Application.Helper
{
    public class CurrencyConverterHelper
    {
        /// <summary>
        /// Converts an input amount from one currency to another using exchange rates.
        /// </summary>
        /// <param name="ValueFrom">Amount to be converted</param>
        /// <param name="InputToBaseRate">Exchange rate from the input currency to the base currency</param>
        /// <param name="BaseToOutputRate">Exchange rate from the base currency to the output currency</param>
        /// <returns>Converted amount in the output currency</returns>
        public async Task<decimal> ConvertCurrency(decimal ValueFrom, decimal InputToBaseRate, decimal BaseToOutputRate)
        {

            decimal baseAmount = ValueFrom / InputToBaseRate;
            decimal outputAmount = Math.Round(baseAmount * BaseToOutputRate, 2);

            return outputAmount;
        }
    }
}
