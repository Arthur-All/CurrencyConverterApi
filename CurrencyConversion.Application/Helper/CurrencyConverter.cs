namespace CurrencyConversion.Application.Helper
{
    public class CurrencyConverter
    {
        /// <summary>
        /// Converts an input amount from one currency to another using exchange rates.
        /// </summary>
        /// <param name="inputAmount">Amount to be converted</param>
        /// <param name="InputToBaseRate">Exchange rate from the input currency to the base currency</param>
        /// <param name="BaseToOutputRate">Exchange rate from the base currency to the output currency</param>
        /// <returns>Converted amount in the output currency</returns>
        public decimal ConvertCurrency(float value, decimal currencyFrom, decimal currencyTo)
        {
            decimal baseAmount = (decimal)value / currencyFrom;
            decimal outputAmount = baseAmount * currencyTo;
            return outputAmount;
        }
    }
}
