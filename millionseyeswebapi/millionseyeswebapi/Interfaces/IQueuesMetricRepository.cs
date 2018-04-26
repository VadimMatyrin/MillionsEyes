using MillionsEyesWebApi.Models.QueuesViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MillionsEyesWebApi.Models
{
    public interface IQueuesMetricRepository
    {
        List<string> GetAllMetrics();

        QueueMetricViewModel CreateMetricModel(List<IncomingMetrics> incomingMetric);

        List<IncomingMetrics> DeserializeToObject(List<string> messages);

        Task<string> GetMetrics(GetMetricModel model);
    }
}