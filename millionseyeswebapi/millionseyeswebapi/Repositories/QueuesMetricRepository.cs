﻿using MillionsEyesWebApi.Helpers;
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
            List<QueueMetric> metrics = new List<QueueMetric>();
            List<QueueMetricModel> metricModels = new List<QueueMetricModel>();
            QueueMetricModel metricModel = new QueueMetricModel();
            QueueMetricViewModel model = new QueueMetricViewModel();
            foreach (var incomingMetric in incomingMetrics)
            {
                foreach (var value in incomingMetric.Value)
                {
                    foreach (var time in value.Timeseries)
                    {
                       foreach (var metadata in time.Metadatavalues)
                       {

                            foreach (var data in time.Data)
                            {
                                QueueMetric metric = new QueueMetric(data.TimeStamp,
                                                                     data.Total);
                                metrics.Add(metric);
                                metricModel.MetricName = value.Name.Value;
                                metricModel.QueueMetrics = metrics;
                                metricModel.QueueName = metadata.Value;
                                metricModels.Add(metricModel);
                            }
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
    }
}