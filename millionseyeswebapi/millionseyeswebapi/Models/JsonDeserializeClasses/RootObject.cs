using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class RootObject
    {
        public double cost { get; set; }
        public string timespan { get; set; }
        public string interval { get; set; }
        public List<Value> value { get; set; }
    }
}