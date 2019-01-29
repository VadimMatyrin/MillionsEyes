using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MillionsEyesWebApi.Models
{
    public interface IMetricsRepository<T>
    {
        Task<IEnumerable<T>> GetMetricsAsync(int interval, DateTime startTime, DateTime endTime, string metricName = null);
    }
}