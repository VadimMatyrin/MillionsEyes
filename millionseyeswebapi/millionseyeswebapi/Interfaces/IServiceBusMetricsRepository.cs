using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MillionsEyesWebApi.Models.MetricViewClasses;

namespace MillionsEyesWebApi
{
    public interface IServiceBusMetricsRepository
    {
        ServiceBusViewModel GetSingleMetricResult(string metricName, DateTime startTime, DateTime finishTime, double interval);

        List<ServiceBusViewModel> GetMetricsResult(DateTime startTime, DateTime finishTime, double interval);
    }
}
