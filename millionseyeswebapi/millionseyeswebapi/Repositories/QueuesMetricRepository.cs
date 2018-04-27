using MillionsEyesWebApi.Helpers;
using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.QueuesViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        public QueueMetricViewModel CreateMetricModel(List<IncomingMetrics> incomingMetrics)
        {
            List<QueueMetricModel> metricModels = new List<QueueMetricModel>();
            QueueMetricViewModel model = new QueueMetricViewModel();

            foreach (var incomingMetric in incomingMetrics)
            {
                foreach (var value in incomingMetric.Value)
                {
                    foreach (var time in value.Timeseries)
                    {
                        foreach (var metadata in time.Metadatavalues)
                        {
                            QueueMetricModel metricModel = new QueueMetricModel();
                            List<QueueMetric> metrics = new List<QueueMetric>();
                            foreach (var data in time.Data)
                            {
                                QueueMetric metric = new QueueMetric(data.TimeStamp, data.Total);
                                metrics.Add(metric);
                            }
                            metricModel.MetricName = value.Name.Value;
                            metricModel.QueueMetrics = metrics;
                            metricModel.QueueName = metadata.Value;
                            metricModels.Add(metricModel);
                        }
                    }

                }
            }

            model.QueueMetrics = metricModels;
            return model;
        }

        public List<string> GetAllMetrics()
        {
            string[] queues = ConfigurationManager.AppSettings["QueueNames"].Split(',');
            List<string> messages = new List<string>();
            Request.Request.Timestamp = GetDefaultTimestamp();
            foreach (var queue in queues)
            {
                Request.Request.EntityName = queue;

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
                messages.Add(message.Result);
            }
            return messages;
        }

        public Task<string> GetMetrics(GetMetricModel model)
        {
            Request.Request.Timestamp = model.Timestamp;
            Request.Request.Interval = model.Interval;

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

        public List<IncomingMetrics> DeserializeToObject(List<string> messages)
        {
            List<IncomingMetrics> metrics = new List<IncomingMetrics>();
            foreach (string message in messages)
            {
                IncomingMetrics metric = new IncomingMetrics();
                metric = JsonConvert.DeserializeObject<IncomingMetrics>(message);
                metrics.Add(metric);
            }
            return metrics;
        }

        public List<string> GetMetricsForHours(int hour, int interval)
        {
            string timestamp = GetTimestampForHour(hour);
            string timeInterval = GetIntervalForHour(interval);
            string[] queues = ConfigurationManager.AppSettings["QueueNames"].Split(',');
            List<string> messages = new List<string>();
            Request.Request.Timestamp = timestamp;
            Request.Request.Interval = timeInterval;
            foreach (var queue in queues)
            {
                Request.Request.EntityName = queue;

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
                messages.Add(message.Result);
            }
            return messages;
        }

        public List<string> GetMetricsForPeriod(DateTime startTime, DateTime endTime, int hour)
        {
            string[] queues = ConfigurationManager.AppSettings["QueueNames"].Split(',');
            List<string> messages = new List<string>();
            string timestamp = GetTimestamp(startTime, endTime);
            string interval = GetInterval(hour);

            Request.Request.Timestamp = timestamp;
            Request.Request.Interval = interval;
            foreach (var queue in queues)
            {
                Request.Request.EntityName = queue;

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
                messages.Add(message.Result);
            }
            return messages;
        }

        private string GetTimestampForHour(int hour)
        {
            DateTime startDate = DateTime.Now.AddHours(-hour);
            DateTime endDate = DateTime.Now;
            string timestamp = GetTimestamp(startDate, endDate);
            return timestamp;
        }
        private string GetDefaultTimestamp()
        {
            DateTime startDate = DateTime.Now.AddHours(-6);
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
        private string GetInterval(int interval)
        {
            return $"PT{interval}H";
        }
        private string GetIntervalForHour(int interval)
        {
            return $"PT{interval}M";
        }
    }
}