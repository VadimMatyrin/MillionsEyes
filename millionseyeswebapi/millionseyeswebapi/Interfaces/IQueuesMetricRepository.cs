using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MillionsEyesWebApi.Models
{
    public interface IQueuesMetricRepository
    {
        Task<string> GetAllMetrics();

        List<QueueMetric> CreateMetricModel(IncomingMetrics incomingMetric);

        IncomingMetrics DeserializeToObject(string message);

        string SerializeToJson(List<QueueMetric> metrics);

        Task<string> GetMetrics(GetMetricModel model);
    }
}