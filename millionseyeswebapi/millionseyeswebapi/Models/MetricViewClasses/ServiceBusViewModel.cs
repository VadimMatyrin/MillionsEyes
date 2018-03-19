using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.MetricViewClasses
{
    public class ServiceBusViewModel
    {
        public string MetricName { get; set; }
        public List<ServiceBusMetricPoint> Points { get; set; }
    }
}