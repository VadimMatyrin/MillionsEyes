using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Timesery
    {
        public List<object> Metadatavalues { get; set; }
        public List<Datum> Data { get; set; }
    }
}