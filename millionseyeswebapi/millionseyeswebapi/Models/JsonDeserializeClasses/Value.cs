using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Value
    {
        public string id { get; set; }
        public string type { get; set; }
        public Name name { get; set; }
        public string unit { get; set; }
        public List<Timesery> timeseries { get; set; }
    }
}