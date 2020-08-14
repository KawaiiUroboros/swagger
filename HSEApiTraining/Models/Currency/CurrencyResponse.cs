using System.Collections.Generic;

namespace HSEApiTraining.Models.Currency
{
    public class CurrencyResponse
    {
        public IEnumerable<double> Rates { get; set; }

        public string Error { get; set; }
    }
}
