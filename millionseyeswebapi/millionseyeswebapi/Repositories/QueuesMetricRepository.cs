using Microsoft.Azure.Management.Monitor;
using MillionsEyesWebApi.Helpers;
using MillionsEyesWebApi.Interfaces;
using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.QueuesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static MillionsEyesWebApi.Properties.Settings;

namespace MillionsEyesWebApi.Repository
{
    public class QueuesMetricRepository : IMetricsRepository<QueueMetricModel>
    {
        private readonly IAzureClientHelper _helper;

        public QueuesMetricRepository(IAzureClientHelper helper)
        {
            _helper = helper;
        }

        public async Task<IEnumerable<QueueMetricModel>> GetMetricsAsync(int interval, DateTime startTime, DateTime endTime, string metricName)
        {
            var models = new List<QueueMetricModel>();

            var tasks = Default.QueueNames.Select(queue => GetMetricsModelAsync(interval, startTime, endTime, metricName, queue));
            models = (await Task.WhenAll(tasks)).ToList();

            return models;
        }

        private async Task<QueueMetricModel> GetMetricsModelAsync(int interval, DateTime startTime, DateTime endTime, string metricName, string queueName)
        {
            var monitorClient = await _helper.GetMonitorClient();

            var response = await monitorClient.Metrics.ListAsync(
            _helper.ResourseUri,
            timespan: $"{startTime:yyyy-MM-ddTHH:mmZ}/{endTime:yyyy-MM-ddTHH:mmZ}",
            interval: TimeSpan.FromMinutes(interval),
            metric: metricName,
            aggregation: Default.Aggregation,
            odataQuery: $"EntityName eq '{queueName}'"
            );

            var model = _helper.AzureResponseToModel<QueueMetricModel>(response);
            model.QueueName = queueName;

            return model;
        }

    }
}