using System;

namespace MillionsEyesWebApi.Models.JsonDeserializeClasses
{
    public class Datum
    {
        public DateTime TimeStamp { get; set; }
        public object Average { get; set; }
        public object Minimum { get; set; }
        public object Maximum { get; set; }
        public double Total { get; set; }
        public object Count { get; set; }
    }
}