using MillionsEyesWebApi.Models.JsonDeserializeClasses;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MillionsEyesWebApi.Models
{
    public class QueueMetric
    {
        [JsonProperty("name")]
        string Name { get; set; }

        [JsonProperty("MetricsForPeriod")]
        List<Datum> MetricsForPeriod { get; set; }


        public QueueMetric(string name, List<Datum> metricsForPeriod)
        {
            Name = name;
            MetricsForPeriod = metricsForPeriod;
        }
    }
}