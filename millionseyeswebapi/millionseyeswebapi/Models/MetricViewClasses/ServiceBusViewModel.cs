using System.Collections.Generic;

namespace MillionsEyesWebApi.Models.MetricViewClasses
{
    public class ServiceBusViewModel
    {
        public string MetricName { get; set; }
        public List<ServiceBusMetricPoint> Points { get; set; }
    }
}