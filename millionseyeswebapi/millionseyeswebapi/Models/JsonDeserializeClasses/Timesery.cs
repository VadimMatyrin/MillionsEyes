using Newtonsoft.Json;
using System.Collections.Generic;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Timesery
    {
        [JsonProperty("metadatavalues")]
        public List<Metadatavalues> Metadatavalues { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }
}