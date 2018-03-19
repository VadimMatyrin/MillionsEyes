using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Datum
    {
        public DateTime timeStamp { get; set; }
        public object average { get; set; }
        public object minimum { get; set; }
        public object maximum { get; set; }
        public double total { get; set; }
        public object count { get; set; }
    }
}