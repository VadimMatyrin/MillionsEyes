using Microsoft.Azure.Management.Monitor;
using MillionsEyesWebApi.Helpers;
using MillionsEyesWebApi.Interfaces;
using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.QueuesModels;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<QueueMetricModel>> GetMetricsAsync(int interval, DateTime startTime, DateTime endTime, string metricName = null)
        {
            var model = await GetMetricsModels(interval, startTime, endTime, metricName);
            return model;
        }

        private async Task<IEnumerable<QueueMetricModel>> GetMetricsModels(int interval, DateTime startTime, DateTime finishTime, string metricName = null)
        {
            string resourceUri = _helper.ResourseUri;
            
            var models = new List<QueueMetricModel>();

            var monitorClient = await _helper.GetMonitorClient();

            foreach (var queue in Default.QueueNames)
            {
                var response = await monitorClient.Metrics.ListAsync(
                resourceUri,
                timespan: $"{startTime:yyyy-MM-ddTHH:mmZ}/{finishTime:yyyy-MM-ddTHH:mmZ}",
                interval: TimeSpan.FromMinutes(interval),
                metric: metricName,
                aggregation: Default.Aggregation,
                odataQuery: $"EntityName eq '{queue}'"
                );

                var model = _helper.AzureResponseToViewModel<QueueMetricModel>(response);
                model.QueueName = queue;
                models.Add(model);
            }

            return models;
        }

    }
}