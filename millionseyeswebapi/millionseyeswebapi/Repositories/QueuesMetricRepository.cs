using MillionsEyesWebApi.Helpers;
using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.QueuesViewModel;
using Newtonsoft.Json;
using ServiceStack;
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

        public QueueMetricViewModel CreateMetricModel(IncomingMetrics incomingMetric)
        {
            List<QueueMetric> metrics = new List<QueueMetric>();
            QueueMetricViewModel model = new QueueMetricViewModel();
            foreach (var value in incomingMetric.Value)
            {
                foreach (var time in value.Timeseries)
                {
                    foreach (var data in time.Data)
                    {
                        QueueMetric metric = new QueueMetric(data.TimeStamp,
                                                             data.Total);
                        model.MetricName = value.Name.Value;
                        metrics.Add(metric);
                    }
                       
                }
            }
            model.QueueMetrics = metrics;
            return model;
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

        private string GetDefaultTimestamp()
        {
            DateTime startDate = DateTime.Now.AddDays(-7);
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