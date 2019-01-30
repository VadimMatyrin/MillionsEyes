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

        public async Task<IEnumerable<ServiceBusModel>> GetMetricsAsync(int interval, DateTime startTime, DateTime endTime, string metricName)
        {
            var models = new List<ServiceBusModel>();
            if (metricName is null)
            {
                var tasks = Default.ServiceBusMetricsList.Select(m => GetMetricsModelAsync(interval, startTime, endTime, m));
                models = (await Task.WhenAll(tasks)).ToList();
            }
            else
                models.Add(await GetMetricsModelAsync(interval, startTime, endTime, metricName));

            return models;
        }

        private async Task<ServiceBusModel> GetMetricsModelAsync(int interval, DateTime startTime, DateTime endTime, string metricName)
        {
            var monitorClient = await _helper.GetMonitorClient();

            var response = await  monitorClient.Metrics.ListAsync(
                _helper.ResourseUri,
                timespan: $"{startTime:yyyy-MM-ddTHH:mmZ}/{endTime:yyyy-MM-ddTHH:mmZ}",
                interval: TimeSpan.FromMinutes(interval),
                metric: metricName,
                aggregation: Default.Aggregation);

            var model = _helper.AzureResponseToModel<ServiceBusModel>(response);
            return model;
        }
    }
}