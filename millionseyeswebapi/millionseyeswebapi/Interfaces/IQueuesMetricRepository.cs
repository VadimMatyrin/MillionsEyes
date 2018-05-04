using MillionsEyesWebApi.Models.QueuesViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MillionsEyesWebApi.Models
{
    public interface IQueuesMetricRepository
    {
        QueueMetricViewModel GetMetricsForHours(int hour, int interval);

        QueueMetricViewModel GetMetricsForPeriod(DateTime startTime, DateTime endTimem, int interval);

        QueueMetricViewModel GetMetricsByName(int interval, string metricName);

        QueueMetricViewModel GetAllMetrics();
    }
}