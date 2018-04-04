using Microsoft.Azure.Management.Monitor;
using Microsoft.Rest.Azure.Authentication;
using MillionsEyesWebApi.Models.JsonDeserializeClasses;
using MillionsEyesWebApi.Models.MetricViewClasses;
using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static MillionsEyesWebApi.Properties.Settings;

namespace MillionsEyesWebApi.Models
{
    public class ServiceBusLogic
    {
        public static ServiceBusViewModel GetSingleMetricResult(string metricName, DateTime startTime, DateTime finishTime, double interval)
        {
            string jsonResult = GetJsonResult(metricName, startTime, finishTime, interval);
            var viewModel = ConvertJsonToViewModel(jsonResult);
            return viewModel;
        }

        public static List<ServiceBusViewModel> GetMetricsResult(DateTime startTime, DateTime finishTime, double interval)
        {
            List<ServiceBusViewModel> result = new List<ServiceBusViewModel>();

            Parallel.For(0, Default.ServiceBusMetricsList.Count, (i) =>
            {
                result.Add(ServiceBusLogic.GetSingleMetricResult(Default.ServiceBusMetricsList[i], startTime, finishTime, interval));
            });

            return result;
        }

        private static string GetJsonResult(string metricName, DateTime startTime, DateTime finishTime, double interval)
        {
            var resourceId = $"subscriptions/{Default.SubscriptionId}/resourceGroups/{Default.ResourseGroupName}/providers/Microsoft.ServiceBus/namespaces/{Default.ServiceBusName}";

            var metricsClient = Authenticate(Default.TenantId, Default.ClientId, Default.Secret, Default.SubscriptionId).Result;

            var metricDefinitions = metricsClient.MetricDefinitions.ListAsync(resourceId).Result;
            var selectedMetrics = metricDefinitions.Select(x => new { x.Name.Value, x.Unit }).Dump();

            var metrics = metricsClient.Metrics.ListAsync(
                resourceUri: resourceId,
                timespan: $"{startTime:yyyy-MM-ddTHH:mmZ}/{finishTime:yyyy-MM-ddTHH:mmZ}",
                interval: TimeSpan.FromMinutes(interval),
                metric: metricName,
                aggregation: "Total").Result;

            string jsonResult = JsonConvert.SerializeObject(metrics, Formatting.Indented);

            return jsonResult;
        }

        public static async Task<MonitorClient> Authenticate(string tenantId, string clientId, string secret, string subscriptionId)
        {
            var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, secret);
            var monitorClient = new MonitorClient(serviceCreds)
            {
                SubscriptionId = subscriptionId
            };

            return monitorClient;
        }

        private static ServiceBusViewModel ConvertJsonToViewModel(string json)
        {
            var root = JsonConvert.DeserializeObject<RootObject>(json);

            ServiceBusViewModel viewModel = new ServiceBusViewModel();
            viewModel.MetricName = root.value[0].name.value;
            viewModel.Points = new List<ServiceBusMetricPoint>();
            for (int i = 0; i < root.value[0].timeseries[0].data.Count; i++)
            {
                viewModel.Points.Add(new ServiceBusMetricPoint()
                {
                    Time = root.value[0].timeseries[0].data[i].timeStamp.ToUniversalTime(),
                    Count = Convert.ToInt32(root.value[0].timeseries[0].data[i].total)
                });
            }

            return viewModel;
        }
    }
}