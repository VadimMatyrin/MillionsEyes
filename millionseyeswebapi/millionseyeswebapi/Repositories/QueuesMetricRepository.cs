using MillionsEyesWebApi.Helpers;
using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.QueuesViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace MillionsEyesWebApi.Repository
{
    public class QueuesMetricRepository : IQueuesMetricRepository
    {
        private readonly HttpClientHelper _helper;

        public QueuesMetricRepository(HttpClientHelper helper)
        {
            _helper = helper;
        }

        public QueueMetricViewModel GetAllMetrics()
        {
            string[] queues = ConfigurationManager.AppSettings["QueueNames"].Split(',');
            List<string> messages = new List<string>();
            Request.Request.Timestamp = GetDefaultTimestamp();
            Request.Request.Interval = GetDefaultInterval();
            Request.Request.MetricName = GetDefaultMetricName();
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
                               $"&metric={Request.Request.MetricName}" +
                               $"&aggregation={Settings.Aggregation}" +
                               $"&$filter=EntityName eq '{Request.Request.EntityName}'" +
                               $"&{Settings.ApiVersion}";

                var message = _helper.GetMethodAsync(url);
                messages.Add(message.Result);
            }
            var metrics = DeserializeToObject(messages);
            var model = CreateMetricModel(metrics);
            return model;
        }

        public QueueMetricViewModel GetMetricsForHours(int hour, int interval)
        {
            GetMetricsForPeriod(DateTime.Now.AddDays(-30), DateTime.Now, 1);
            string timestamp = GetTimestampForHour(hour);
            string timeInterval = GetIntervalForHour(interval);
            if (interval == 60)
            {
                timeInterval = GetInterval(1);
            }
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
                               $"&metric={Request.Request.MetricName}" +
                               $"&aggregation={Settings.Aggregation}" +
                               $"&$filter=EntityName eq '{Request.Request.EntityName}'" +
                               $"&{Settings.ApiVersion}";

                var message = _helper.GetMethodAsync(url);
                messages.Add(message.Result);
            }
            var metrics = DeserializeToObject(messages);
            var model = CreateMetricModel(metrics);
            return model;
        }

        public QueueMetricViewModel GetMetricsForPeriod(DateTime startTime, DateTime endTime, int hour)
        {
            GetMetricsForPeriod(DateTime.Now.AddDays(-30), DateTime.Now, 1);
            string[] queues = ConfigurationManager.AppSettings["QueueNames"].Split(',');
            List<string> messages = new List<string>();
            string timestamp = GetTimestamp(startTime, endTime);
            string interval = GetIntervalForHour(hour);
            if (hour == 60)
            {
                interval = GetInterval(1);
            }
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
                               $"&metric={Request.Request.MetricName}" +
                               $"&aggregation={Settings.Aggregation}" +
                               $"&$filter=EntityName eq '{Request.Request.EntityName}'" +
                               $"&{Settings.ApiVersion}";

                var message = _helper.GetMethodAsync(url);
                messages.Add(message.Result);
            }
            var metrics = DeserializeToObject(messages);
            var model = CreateMetricModel(metrics);
            return model;
        }

        public QueueMetricViewModel GetMetricsByName(int hour, string metricName)
        {
            GetMetricsForPeriod(DateTime.Now.AddDays(-30), DateTime.Now, 1);
            string[] queues = ConfigurationManager.AppSettings["QueueNames"].Split(',');
            List<string> messages = new List<string>();
            string timestamp = GetDefaultTimestamp();
            string interval = GetIntervalForHour(hour);
            if (hour == 60)
            {
                interval = GetInterval(1);
            }
            Request.Request.Timestamp = timestamp;
            Request.Request.Interval = interval;
            Request.Request.MetricName = metricName;
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
                               $"&metric={Request.Request.MetricName}" +
                               $"&aggregation={Settings.Aggregation}" +
                               $"&$filter=EntityName eq '{Request.Request.EntityName}'" +
                               $"&{Settings.ApiVersion}";

                var message = _helper.GetMethodAsync(url);
                messages.Add(message.Result);
            }
            var metrics = DeserializeToObject(messages);
            var model = CreateMetricModel(metrics);
            return model;
        }

        private QueueMetricViewModel CreateMetricModel(List<IncomingMetrics> incomingMetrics)
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

        private List<IncomingMetrics> DeserializeToObject(List<string> messages)
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

        private string GetTimestampForHour(int hour)
        {
            DateTime startDate = DateTime.Now.AddHours(-hour);
            DateTime endDate = DateTime.Now;
            string timestamp = GetTimestamp(startDate, endDate);
            return timestamp;
        }
        private string GetDefaultTimestamp()
        {
            DateTime startDate = DateTime.Now.AddDays(-1);
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

        private string GetDefaultInterval() => "PT30M";

        private string GetInterval(int interval) => $"PT{interval}H";

        private string GetIntervalForHour(int interval) => $"PT{interval}M";

        private string GetDefaultMetricName() => "IncomingRequests";
    }
}