using System;

namespace HSEApiTraining.Models.Currency
{
    public class CurrencyRequest
    {
        public CurrencyRequest(string symbol, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            DateStart = dateStart;
            DateEnd = dateEnd;
            Symbol = symbol;
        }
        private DateTime? dateStart, dateEnd;

        public string Symbol { get; set; }

        public DateTime? DateStart
        {
            get => dateStart;
            set
            {
               
                if (value == null)
                    dateStart = DateTime.Now.Date;
                else
                    dateStart = value;
            }

        }

        public DateTime? DateEnd
        {
            get => dateEnd;
            set
            {
                if (!value.HasValue)
                    dateEnd = DateStart;
                else
                    dateEnd = value;
            }
        } 
    }
}
