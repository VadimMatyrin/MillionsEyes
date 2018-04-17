using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Value
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public Name Name { get; set; }
        public string Unit { get; set; }
        public List<Timesery> Timeseries { get; set; }
    }
}