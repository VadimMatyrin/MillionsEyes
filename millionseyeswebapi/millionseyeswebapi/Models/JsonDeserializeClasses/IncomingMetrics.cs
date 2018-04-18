using MillionsEyesWebApi.Models.JsonDeserializeClasses;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MillionsEyesWebApi.Models
{
    public class IncomingMetrics
    {
        [JsonProperty("cost")]
        public long Cost { get; set; }

        [JsonProperty("timespan")]
        public string Timespan { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("value")]
        public List<Value> Value { get; set; }
    }
}