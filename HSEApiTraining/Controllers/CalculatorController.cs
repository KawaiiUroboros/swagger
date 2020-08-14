using HSEApiTraining.Models.Calculator;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HSEApiTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;

        //В конструкторе контроллера происходит инъекция сервисов через их интерфейсы
        public CalculatorController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        /// <summary>
        /// Обрабатывает запрос-выражение.
        /// </summary>
        /// <param name="expression"> Выражение. </param>
        /// <returns> Ответ - результат вычисления выражения. </returns>
        [HttpGet]
        public CalculatorResponse Calculate([FromQuery] string expression)
        {
            //Тут нужно подключить реализованную в сервисе calculatorService логику вычисления выражений
            //В нижнем методе - аналогично
            try
            {
                var result = _calculatorService.CalculateExpression(expression);

                return new CalculatorResponse
                {
                    Value = result
                };
            }
            catch
            {
                return new CalculatorResponse
                {
                    Value = 0,
                    Error = "Incorrect input"
                };
            }
        }

        /// <summary>
        /// Обрабатывает запрос - ряд выражений.
        /// </summary>
        /// <param name="Request"> Запрос, содержащий выражения. </param>
        /// <returns> Ответ, содержащий посчитанные выражения. </returns>
        [HttpPost]
        public CalculatorBatchResponse CalculateBatch([FromBody] CalculatorBatchRequest Request)
        {
            try
            {
                var result = _calculatorService.CalculateBatchExpressions(Request.Expressions);

                return new CalculatorBatchResponse
                {
                    Values = result.Select(x => new CalculatorResponse { Value = x })
                };
            }
            catch
            {
                return new CalculatorBatchResponse
                {
                    Values = null,
                    Error = "Incorrect input"
                };
            }
        }

        //Примеры-пустышки
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
