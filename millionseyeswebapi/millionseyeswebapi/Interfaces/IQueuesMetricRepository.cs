using MillionsEyesWebApi.Models.QueuesViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MillionsEyesWebApi.Models
{
    public interface IQueuesMetricRepository
    {
        List<string> GetMetricsForHours(int hour, int interval);

        List<string> GetMetricsForPeriod(DateTime startTime, DateTime endTimem, int interval);

        List<string> GetAllMetrics();

        QueueMetricViewModel CreateMetricModel(List<IncomingMetrics> incomingMetric);

        List<IncomingMetrics> DeserializeToObject(List<string> messages);
    }
}