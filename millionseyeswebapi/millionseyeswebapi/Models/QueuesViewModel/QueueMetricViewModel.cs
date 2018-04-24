using Newtonsoft.Json;
using System.Collections.Generic;

namespace MillionsEyesWebApi.Models.QueuesViewModel
{
    public class QueueMetricViewModel
    {
        public string MetricName { get; set; }

        public List<QueueMetric> QueueMetrics { get; set; }
    }
}