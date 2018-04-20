using MillionsEyesWebApi.Models.MetricViewClasses;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using static MillionsEyesWebApi.Models.ServiceBusLogic;

namespace MillionsEyesWebApi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "POST, PUT, DELETE, OPTIONS")]
    public class ServiceBusMetricsController : ApiController
    {
        [HttpGet]
        [ResponseType(responseType: typeof(List<ServiceBusViewModel>))]
        public IHttpActionResult Get(int hoursCount, double interval)
        {
            return Ok(content: GetMetricsResult(startTime: DateTime.UtcNow.AddHours(value: -hoursCount),
                finishTime: DateTime.UtcNow, interval: interval));
        }

        //[HttpGet]
        //[ResponseType(responseType: typeof(List<ServiceBusViewModel>))]
        //public IHttpActionResult Get(DateTime startTime, DateTime finishTime)
        //{
        //    return Ok(content: GetMetricsResult(startTime: startTime, finishTime: finishTime, interval: 1));
        //}

        //[HttpGet]
        //[ResponseType(responseType: typeof(List<ServiceBusViewModel>))]
        //public IHttpActionResult Get(DateTime startTime, DateTime finishTime, int interval)
        //{
        //    return Ok(content: GetMetricsResult(startTime: startTime, finishTime: finishTime, interval: interval));
        //}
    }
}
