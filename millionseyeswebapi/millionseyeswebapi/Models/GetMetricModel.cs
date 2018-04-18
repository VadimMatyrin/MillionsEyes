using System;

namespace MillionsEyesWebApi.Models
{
    public class GetMetricModel
    {
        public string MetricName { get; set; }

        public string Interval { get; set; }

        public string Timestamp { get; set; }
    }
}