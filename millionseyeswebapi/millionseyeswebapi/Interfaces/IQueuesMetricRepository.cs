using MillionsEyesWebApi.Models.QueuesViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MillionsEyesWebApi.Models
{
    public interface IQueuesMetricRepository
    {
        Task<string> GetAllMetrics();

        QueueMetricViewModel CreateMetricModel(IncomingMetrics incomingMetric);

        IncomingMetrics DeserializeToObject(string message);

        Task<string> GetMetrics(GetMetricModel model);
    }
}