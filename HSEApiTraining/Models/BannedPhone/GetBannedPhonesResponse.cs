using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HSEApiTraining.Models.BannedPhone
{
    public class GetBannedPhonesResponse
    {
        public IEnumerable<BannedPhone> BannedPhones { get; set; }
        public string Error
        {
            get; set;
        }
    }
}
