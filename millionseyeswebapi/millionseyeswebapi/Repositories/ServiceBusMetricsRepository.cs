using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Monitor;
using Microsoft.Rest.Azure.Authentication;
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
            var jsonResult = GetJsonResult(metricName: metricName, startTime: startTime, finishTime: finishTime, interval: interval);
            var viewModel = ConvertJsonToViewModel(json: jsonResult);
            return viewModel;
        }

        public List<ServiceBusViewModel> GetMetricsResult(DateTime startTime, DateTime finishTime, double interval)
        {
            List<ServiceBusViewModel> result = new List<ServiceBusViewModel>();

            Parallel.For(fromInclusive: 0, toExclusive: Default.ServiceBusMetricsList.Count, body: (i) =>
            {
                result.Add(item: GetSingleMetricResult(metricName: Default.ServiceBusMetricsList[index: i], startTime: startTime, finishTime: finishTime, interval: interval));
            });

            return result;
        }

        private string GetJsonResult(string metricName, DateTime startTime, DateTime finishTime, double interval)
        {
            var resourceId = $"subscriptions/{Default.SubscriptionId}/resourceGroups/{Default.ResourseGroupName}/providers/Microsoft.ServiceBus/namespaces/{Default.ServiceBusName}";

            var metricsClient = Authenticate(tenantId: Default.TenantId, clientId: Default.ClientId, secret: Default.Secret, subscriptionId: Default.SubscriptionId).Result;

            //var metricDefinitions = metricsClient.MetricDefinitions.ListAsync(resourceId).Result;
            //metricDefinitions.Select(x => new { x.Name.Value, x.Unit }).Dump();

            var metrics = metricsClient.Metrics.ListAsync(
                resourceUri: resourceId,
                timespan: $"{startTime:yyyy-MM-ddTHH:mmZ}/{finishTime:yyyy-MM-ddTHH:mmZ}",
                interval: TimeSpan.FromMinutes(value: interval),
                metric: metricName,
                aggregation: "Total").Result;

            var jsonResult = JsonConvert.SerializeObject(value: metrics, formatting: Formatting.Indented);

            return jsonResult;
        }

        private async Task<MonitorClient> Authenticate(string tenantId, string clientId, string secret, string subscriptionId)
        {
            var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(domain: tenantId, clientId: clientId, secret: secret);
            var monitorClient = new MonitorClient(credentials: serviceCreds)
            {
                SubscriptionId = subscriptionId
            };

            return monitorClient;
        }

        private ServiceBusViewModel ConvertJsonToViewModel(string json)
        {
            var root = JsonConvert.DeserializeObject<RootObject>(value: json);

            ServiceBusViewModel viewModel = new ServiceBusViewModel
            {
                MetricName = root.Value[index: 0].Name.Value,
                Points = new List<ServiceBusMetricPoint>()
            };

            foreach (var t in root.Value[index: 0].Timeseries[index: 0].Data)
            {
                viewModel.Points.Add(item: new ServiceBusMetricPoint()
                {
                    Time = t.TimeStamp.ToUniversalTime(),
                    Count = Convert.ToInt32(value: t.Total)
                });
            }

            return viewModel;
        }
    }
}