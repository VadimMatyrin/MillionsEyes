using Newtonsoft.Json;
using System;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Datum
    {
        [JsonProperty("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }
}