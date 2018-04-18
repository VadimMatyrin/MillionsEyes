using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Timesery
    {
        [JsonProperty("metadatavalues")]
        public List<object> Metadatavalues { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }
}