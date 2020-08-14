using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HSEApiTraining.Models.BannedPhone
{
    public class AddBannedPhoneRequest
    {
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
