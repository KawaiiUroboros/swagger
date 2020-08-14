using HSEApiTraining.Models.Currency;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HSEApiTraining.Controllers
{
    //Тут все методы-хендлеры вам нужно реализовать самим
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;

        //В конструкторе контроллера происходит инъекция сервисов через их интерфейсы
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public IActionResult DummyMethod()
        {
            return View();
        }

        [HttpPost]
        public CurrencyResponse Post([FromBody] CurrencyRequest Request)
        {
            try
            {
                // Проверяем непротиворечивость дат.
                if (Request.DateStart > DateTime.Now || Request.DateEnd > DateTime.Now
                            || Request.DateStart > Request.DateEnd)
                    throw new ArgumentException("Incorrect Date.");

                // Проверяем валидность символа.
                if(!CheckValidSymbol(Request.Symbol))
                    throw new ArgumentException("Invalid Symbol.");

                var resp = _currencyService.GetRates(Request);

                return new CurrencyResponse() { Rates = resp.Result };
            }
            catch(Exception ex)
            {
                return new CurrencyResponse() { Rates = null, Error = ex.Message };
            }
        }

        /// <summary>
        /// Проверяет сивол на минимальные условия валидности.
        /// </summary>
        /// <param name="symb"> Символ. </param>
        /// <returns> Истина, если валиден, иначе - ложь. </returns>
        private bool CheckValidSymbol(string symb)
        {
            if (symb.Length != 3)
                return false;

            foreach(char c in symb)
            {
                if (c < 'A' || c > 'Z')
                    return false;
            }

            return true;
        }
    }
}