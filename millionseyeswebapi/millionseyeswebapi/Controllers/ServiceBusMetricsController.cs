using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.MetricViewClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace MillionsEyesWebApi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "POST, PUT, DELETE, OPTIONS")]
    public class ServiceBusMetricsController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(List<ServiceBusViewModel>))]
        public IHttpActionResult Get(int hoursCount, double interval)
        {
            return Ok(ServiceBusLogic.GetMetricsResult(DateTime.UtcNow.AddHours(-hoursCount), DateTime.UtcNow, interval));
        }

        [HttpGet]
        [ResponseType(typeof(List<ServiceBusViewModel>))]
        public IHttpActionResult Get(DateTime startTime, DateTime finishTime)
        {
            return Ok(ServiceBusLogic.GetMetricsResult(startTime, finishTime, 1));
        }

        [HttpGet]
        [ResponseType(typeof(List<ServiceBusViewModel>))]
        public IHttpActionResult Get(DateTime startTime, DateTime finishTime, int interval)
        {
            return Ok(ServiceBusLogic.GetMetricsResult(startTime, finishTime, interval));
        }
    }
}
