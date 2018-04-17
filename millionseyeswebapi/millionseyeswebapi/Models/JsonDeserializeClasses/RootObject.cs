using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class RootObject
    {
        public double Cost { get; set; }
        public string Timespan { get; set; }
        public string Interval { get; set; }
        public List<Value> Value { get; set; }
    }
}