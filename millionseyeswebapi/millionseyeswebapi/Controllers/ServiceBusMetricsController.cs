using MillionsEyesWebApi.Models.MetricViewClasses;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace MillionsEyesWebApi.Controllers
{
    [EnableCors("http://localhost:4200", "*", "POST, PUT, DELETE, OPTIONS")]
    public class ServiceBusMetricsController : ApiController
    {
        private readonly IServiceBusMetricsRepository _serviceBusMetricsRepository;

        public ServiceBusMetricsController(IServiceBusMetricsRepository serviceBusMetricsRepository)
        {
            _serviceBusMetricsRepository = serviceBusMetricsRepository;
        }

        [HttpGet]
        [ResponseType(typeof(List<ServiceBusViewModel>))]
        public IHttpActionResult Get(int hoursCount, double interval)
        {
            return Ok(_serviceBusMetricsRepository.GetMetricsResult(DateTime.UtcNow.AddHours(-hoursCount),
                DateTime.UtcNow, interval));
        }

        //[HttpGet]
        //[ResponseType(responseType: typeof(List<ServiceBusViewModel>))]
        //public IHttpActionResult Get(DateTime startTime, DateTime finishTime)
        //{
        //    return Ok(_serviceBusMetricsRepository.GetMetricsResult(startTime: startTime, finishTime: finishTime, interval: 1));
        //}

        //[HttpGet]
        //[ResponseType(responseType: typeof(List<ServiceBusViewModel>))]
        //public IHttpActionResult Get(DateTime startTime, DateTime finishTime, int interval)
        //{
        //    return Ok(_serviceBusMetricsRepository.GetMetricsResult(startTime: startTime, finishTime: finishTime, interval: interval));
        //}
    }
}
