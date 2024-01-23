﻿using CurrencyConversion.Application.Interface;
using CurrencyConversion.Domain.DTOs;
using CurrencyConversion.Domain.Enum;
using CurrencyConversion.Infra.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConversion.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        #region Properties
        private readonly ICurrencyRateService _currencyrate;
        #endregion Properties

        #region Contructor
        public CurrencyConverterController(ICurrencyRateService currencyrate)
        {
            _currencyrate = currencyrate;
        }
        #endregion Contructor

        [HttpGet("GetAllCurrencyRate")]

        public async Task<ActionResult<IEnumerable<ExchangeRatesDto>>> GetAllCurrencyRateAsync()
        {
            try
            {
                return Ok(await _currencyrate.GetAllCurrencyRateAsync());
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet("CurrencyConverter")]

        public ActionResult<decimal> ConvertCurrency(decimal value, CurrenciesEnum currencyFrom, CurrenciesEnum currencyTo)
        {
            try
            {
                return Ok( _currencyrate.ConvertCurrency(value,  currencyFrom.ToString(),  currencyTo.ToString()));
            }
            catch (Exception ex) { throw ex; }
        }

        //[HttpGet("GetCurrencyRate")]
        //public async Task<IEnumerable<RatesDto>> GetCurrencyRate(CurrenciesEnum currencyFrom , CurrenciesEnum currencyTo)
        //{
        //    return await _currencyrate.TEST(currencyFrom.ToString(), currencyTo.ToString());
        //}

    }
}