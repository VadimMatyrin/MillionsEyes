using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.MetricModels
{
    public class MetricData
    {
        public DateTime Time { get; set; }
        public long Count { get; set; }
    }
}