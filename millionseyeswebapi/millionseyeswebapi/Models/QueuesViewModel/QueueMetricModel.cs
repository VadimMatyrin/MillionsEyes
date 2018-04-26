using System.Collections.Generic;

namespace MillionsEyesWebApi.Models.QueuesViewModel
{
    public class QueueMetricModel
    {
        public string QueueName { get; set; }

        public string MetricName { get; set; }

        public List<QueueMetric> QueueMetrics { get; set; }
    }
}