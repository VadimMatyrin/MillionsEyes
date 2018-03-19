using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.MetricViewClasses
{
    public class ServiceBusMetricPoint
    {
        public DateTime Time { get; set; }
        public int Count { get; set; }
    }
}