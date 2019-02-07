using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.MetricModels
{
    public abstract class MetricModel
    {
        public string MetricName { get; set; }

        public List<MetricData> Metrics { get; set; }
    }
}