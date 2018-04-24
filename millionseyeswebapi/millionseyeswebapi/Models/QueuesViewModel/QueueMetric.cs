using Newtonsoft.Json;
using System;

namespace MillionsEyesWebApi.Models
{
    public class QueueMetric
    {
        [JsonProperty("Time")]
        public DateTime Time { get; set; }

        [JsonProperty("Count")]
        public long Count { get; set; }


        public QueueMetric(DateTime time, long count)
        {
            Time = time;
            Count = count;
        }
    }
}