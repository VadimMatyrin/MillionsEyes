using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor;
using Microsoft.Rest.Azure.Authentication;
using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.JsonDeserializeClasses;
using MillionsEyesWebApi.Models.MetricViewClasses;
using Newtonsoft.Json;

using static MillionsEyesWebApi.Properties.Settings;

namespace MillionsEyesWebApi.Repositories
{
    public class ServiceBusMetricsRepository : IServiceBusMetricsRepository
    {
        public ServiceBusViewModel GetSingleMetricResult(string metricName, DateTime startTime, DateTime finishTime, double interval)
        {
            var jsonResult = getJsonResult(metricName, startTime, finishTime, interval);
            var viewModel = convertJsonToViewModel(jsonResult);
            return viewModel;
        }

        public List<ServiceBusViewModel> GetMetricsResult(DateTime startTime, DateTime finishTime, double interval)
        {
            List<ServiceBusViewModel> result = new List<ServiceBusViewModel>();

            Parallel.For(0, Default.ServiceBusMetricsList.Count, (i) =>
            {
                result.Add(GetSingleMetricResult(Default.ServiceBusMetricsList[i], startTime, finishTime, interval));
            });

            return result;
        }

        private string getJsonResult(string metricName, DateTime startTime, DateTime finishTime, double interval)
        {
            var resourceId = $"subscriptions/{Default.SubscriptionId}/resourceGroups/{Default.ResourseGroupName}/providers/Microsoft.ServiceBus/namespaces/{Default.ServiceBusName}";

            var metricsClient = authenticate(Default.TenantId, Default.ClientId, Default.Secret, Default.SubscriptionId).Result;

            //var metricDefinitions = metricsClient.MetricDefinitions.ListAsync(resourceId).Result;
            //metricDefinitions.Select(x => new { x.Name.Value, x.Unit }).Dump();

            var metrics = metricsClient.Metrics.ListAsync(
                resourceId,
                timespan: $"{startTime:yyyy-MM-ddTHH:mmZ}/{finishTime:yyyy-MM-ddTHH:mmZ}",
                interval: TimeSpan.FromMinutes(interval),
                metric: metricName,
                aggregation: "Total").Result;

            var jsonResult = JsonConvert.SerializeObject(metrics, Formatting.Indented);

            return jsonResult;
        }

        private async Task<MonitorClient> authenticate(string tenantId, string clientId, string secret, string subscriptionId)
        {
            var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, secret);
            var monitorClient = new MonitorClient(serviceCreds)
            {
                SubscriptionId = subscriptionId
            };

            return monitorClient;
        }

        private ServiceBusViewModel convertJsonToViewModel(string json)
        {
            var root = JsonConvert.DeserializeObject<IncomingMetrics>(json);

            ServiceBusViewModel viewModel = new ServiceBusViewModel
            {
                MetricName = root.Value[0].Name.Value,
                Points = new List<ServiceBusMetricPoint>()
            };

            foreach (var t in root.Value[0].Timeseries[0].Data)
            {
                viewModel.Points.Add(new ServiceBusMetricPoint()
                {
                    Time = t.TimeStamp.AddHours(3).ToUniversalTime(),
                    Count = Convert.ToInt32(t.Total)
                });
            }

            return viewModel;
        }
    }
}