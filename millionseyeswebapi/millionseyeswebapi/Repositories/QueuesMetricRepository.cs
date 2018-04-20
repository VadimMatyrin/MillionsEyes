using MillionsEyesWebApi.Helpers;
using MillionsEyesWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MillionsEyesWebApi.Repository
{
    public class QueuesMetricRepository : IQueuesMetricRepository
    {
        private readonly HttpClientHelper _helper;

        public QueuesMetricRepository(HttpClientHelper helper)
        {
            _helper = helper;
        }

        public QueuesMetricRepository()
        {
        }

        public List<QueueMetric> CreateMetricModel(IncomingMetrics incomingMetric)
        {
            List<QueueMetric> metrics = new List<QueueMetric>();
            foreach (var value in incomingMetric.Value)
            {
                foreach (var time in value.Timeseries)
                {
                    QueueMetric metric = new QueueMetric(value.Name.Value,
                                                         time.Data);
                    metrics.Add(metric);
                }
            }
            return metrics;
        }

        public Task<string> GetAllMetrics()
        {
            Request.Request.Timestamp = GetDefaultTimestamp();

            string url = $"{Settings.BaseUrl}/subscriptions/{Settings.SubscriptionId}/" +
                               $"resourceGroups/{Request.Request.ResourceGroup}/" +
                               $"providers/{Request.Request.Provider}/" +
                               $"namespaces/{Request.Request.ServiceBusNameSpace}/" +
                               $"providers/{Request.Request.InsightProvider}/" +
                               $"metrics?timespan={Request.Request.Timestamp}" +
                               $"&interval={Request.Request.Interval}" +
                               $"&metric={Request.Request.Metrics}" +
                               $"&aggregation={Settings.Aggregation}" +
                               $"&$filter=EntityName eq '{Request.Request.EntityName}'" +
                               $"&{Settings.ApiVersion}";

            var message = _helper.GetMethodAsync(url);
            return message;
        }

        public Task<string> GetMetrics(GetMetricModel model)
        {
            Request.Request.Timestamp = model.Timestamp;
            Request.Request.Interval = model.Interval;
            Request.Request.Metrics = model.MetricName;

            string url = $"{Settings.BaseUrl}/subscriptions/{Settings.SubscriptionId}/" +
                               $"resourceGroups/{Request.Request.ResourceGroup}/" +
                               $"providers/{Request.Request.Provider}/" +
                               $"namespaces/{Request.Request.ServiceBusNameSpace}/" +
                               $"providers/{Request.Request.InsightProvider}/" +
                               $"metrics?timespan={Request.Request.Timestamp}" +
                               $"&interval={Request.Request.Interval}" +
                               $"&metric={Request.Request.Metrics}" +
                               $"&aggregation={Settings.Aggregation}" +
                               $"&$filter=EntityName eq '{Request.Request.EntityName}'" +
                               $"&{Settings.ApiVersion}";

            return _helper.GetMethodAsync(url);
        }

        public IncomingMetrics DeserializeToObject(string message)
        {
            IncomingMetrics metric = new IncomingMetrics();
            metric = JsonConvert.DeserializeObject<IncomingMetrics>(message);
            return metric;
        }

        public string SerializeToJson(List<QueueMetric> metrics)
        {
            string json = JsonConvert.SerializeObject(metrics);
            return json;
        }

        private string GetDefaultTimestamp()
        {
            DateTime startDate = DateTime.Now.AddDays(-15);
            DateTime endDate = DateTime.Now;
            string timestamp = GetTimestamp(startDate, endDate);
            return timestamp;
        }

        private string GetTimestamp(DateTime startDate, DateTime endDate)
        {
            string format = ("yyyy-MM-ddTHH:mmZ");
            string sDate = startDate.ToString(format);
            string eDate = endDate.ToString(format);
            string timestamp = String.Format("{0}/{1}", sDate, eDate);
            return timestamp;
        }
    }
}