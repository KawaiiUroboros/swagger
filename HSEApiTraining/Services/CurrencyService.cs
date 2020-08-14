using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using HSEApiTraining.Models.Currency;
using System.Web;
using Newtonsoft.Json.Linq;

namespace HSEApiTraining
{
    public interface ICurrencyService
    {
        Task<IEnumerable<double>> GetRates(CurrencyRequest request);
    }

    public class CurrencyService: ICurrencyService
    {
        private UriBuilder uriBuilder = new UriBuilder("https://api.ratesapi.io/api/");

        /// <summary>
        /// Отправляет запросы по нашим данным и возвращает ряд ответов.
        /// </summary>
        /// <param name="Request"> Запрос, который был отправлен нам. </param>
        /// <returns> Ответ на него. </returns>
        public async Task<IEnumerable<double>> GetRates(CurrencyRequest Request)
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["base"] = Request.Symbol;
            parameters["symbols"] = "RUB";

            // Лист наших ответов.
            List<double> rates = new List<double>();

            DateTime start = (DateTime)Request.DateStart;

            // Формируем url для запроса и делаем сам запрос.
            while (start <= Request.DateEnd)
            {
                uriBuilder.Path = start.ToString("yyyy-MM-dd");
                uriBuilder.Query = parameters.ToString();

                Uri finalUrl = uriBuilder.Uri;

                rates.Add(await GetAnswer(finalUrl));

                start = start.AddDays(1);
            }

            return rates;
        }

        /// <summary>
        /// Отправляет запрос и получает ответ, приводя его к нужному виду.
        /// </summary>
        /// <param name="url"> Адрес для запроса. </param>
        /// <returns> Ответ на запрос. </returns>
        private async Task<double> GetAnswer(Uri url)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(url);
                if (result.IsSuccessStatusCode)
                    response = await result.Content.ReadAsStringAsync();
            }

            return ResponseDeserialize(response);
        }

        /// <summary>
        /// Парсит ответ на запрос.
        /// </summary>
        /// <param name="json"> Строка в виде json. </param>
        /// <returns> Значение double - нужный нам параметр. </returns>
        private double ResponseDeserialize(string json)
        {
            JObject jObject = JObject.Parse(json);
            var token = jObject["rates"].ToObject<Dictionary<string, double>>();
            return token.First().Value;
        }
    }
}