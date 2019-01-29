using Microsoft.Azure.Management.Monitor;
using Microsoft.Azure.Management.Monitor.Models;
using Microsoft.Rest.Azure.Authentication;
using MillionsEyesWebApi.Interfaces;
using MillionsEyesWebApi.Models.MetricModels;
using System;
using System.Linq;
using System.Threading.Tasks;

using static MillionsEyesWebApi.Properties.Settings;

namespace MillionsEyesWebApi.Helpers
{
    public class AzureClientHelper : IAzureClientHelper
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _subscriptionId;

        public string ResourseUri => $"subscriptions/{Default.SubscriptionId}/" +
                               $"resourceGroups/{Default.ResourseGroupName}/" +
                               $"providers/{Default.Provider}/" +
                               $"namespaces/{Default.ServiceBusName}";

        public AzureClientHelper(string tenantId, string clientId, string secret, string subscriptionId)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _secret = secret;
            _subscriptionId = subscriptionId;
        }

        public async Task<MonitorClient> GetMonitorClient()
        {
            var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(_tenantId, _clientId, _secret);
            var monitorClient = new MonitorClient(serviceCreds)
            {
                SubscriptionId = _subscriptionId
            };

            return monitorClient;
        }

        public T AzureResponseToViewModel<T>(Response response) where T : MetricModel, new()
        {
            var model = response.Value.Select(r =>
            new T
            {
                MetricName = r.Name.Value,
                Metrics = r.Timeseries.SelectMany(rt => rt.Data.Select(d =>
                new MetricData
                {
                    Time = d.TimeStamp,
                    Count = Convert.ToInt64(d.Total)
                }).ToList()).ToList()
            }
            );

            return model.FirstOrDefault();
        }

    }
}