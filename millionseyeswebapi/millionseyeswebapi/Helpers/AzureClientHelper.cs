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
        public string ResourseUri => $"subscriptions/{Default.SubscriptionId}/" +
                               $"resourceGroups/{Default.ResourseGroupName}/" +
                               $"providers/{Default.Provider}/" +
                               $"namespaces/{Default.ServiceBusName}";

        public async Task<MonitorClient> GetMonitorClient()
        {
            var serviceCreds = await ApplicationTokenProvider.LoginSilentAsync(Default.TenantId, Default.ClientId, Default.Secret);
            var monitorClient = new MonitorClient(serviceCreds)
            {
                SubscriptionId = Default.SubscriptionId
            };

            return monitorClient;
        }

        public T AzureResponseToModel<T>(Response response) where T : MetricModel, new()
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            if (response.Value is null)
                throw new ArgumentNullException(nameof(response.Value));

            var model = response.Value.Select(r =>
            new T
            {
                MetricName = r.Name?.Value ?? throw new ArgumentNullException(nameof(r.Name)),
                Metrics = r.Timeseries?.SelectMany(rt => rt.Data.Select(d =>
                new MetricData
                {
                    Time = d.TimeStamp,
                    Count = (long)(d.Total ?? 0)
                }).ToList()).ToList() ?? throw new ArgumentNullException(nameof(r.Timeseries))

            });

            return model.FirstOrDefault();
        }

    }

}