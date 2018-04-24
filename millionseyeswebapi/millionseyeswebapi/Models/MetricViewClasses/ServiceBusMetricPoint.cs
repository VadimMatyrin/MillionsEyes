using System;

namespace MillionsEyesWebApi.Models.MetricViewClasses
{
    public class ServiceBusMetricPoint
    {
        public DateTime Time { get; set; }

        public int Count { get; set; }
    }
}