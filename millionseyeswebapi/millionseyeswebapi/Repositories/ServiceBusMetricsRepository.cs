using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor;
using MillionsEyesWebApi.Interfaces;
using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.MetricModels;

using static MillionsEyesWebApi.Properties.Settings;

namespace MillionsEyesWebApi.Repositories
{
    public class ServiceBusMetricsRepository : IMetricsRepository<ServiceBusModel>
    {
        private readonly IAzureClientHelper _helper;

        public ServiceBusMetricsRepository(IAzureClientHelper helper)
        {
            _helper = helper;
        }

        public async Task<IEnumerable<ServiceBusModel>> GetMetricsAsync(int interval, DateTime startTime, DateTime entTime, string metricName = null)
        {
            var models = new List<ServiceBusModel>();
            if (metricName is null)
            {
                var tasks = Default.ServiceBusMetricsList.Select(e => GetMetricsViewModels(interval, startTime, entTime, e));
                models = (await Task.WhenAll(tasks)).ToList();
            }
            else
                models.Add(await GetMetricsViewModels(interval, startTime, entTime, metricName));

            return models;
        }

        private async Task<ServiceBusModel> GetMetricsViewModels(int interval, DateTime startTime, DateTime finishTime, string metricName)
        {
            string resourceUri = _helper.ResourseUri;

            var monitorClient = await _helper.GetMonitorClient();

            var response = await  monitorClient.Metrics.ListAsync(
                resourceUri,
                timespan: $"{startTime:yyyy-MM-ddTHH:mmZ}/{finishTime:yyyy-MM-ddTHH:mmZ}",
                interval: TimeSpan.FromMinutes(interval),
                metric: metricName,
                aggregation: Default.Aggregation);

            var model = _helper.AzureResponseToViewModel<ServiceBusModel>(response);
            return model;
        }
    }
}