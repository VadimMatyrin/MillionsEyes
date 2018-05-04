using MillionsEyesWebApi.Models;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MillionsEyesWebApi.Controllers
{
    [RoutePrefix("api/queuemetrics")]
    [EnableCors("http://localhost:4200", "*", "POST, PUT, DELETE, OPTIONS")]
    public class QueueMetricsController : ApiController
    {
        private readonly IQueuesMetricRepository _queuesMetricRepository;

        public QueueMetricsController(IQueuesMetricRepository queuesMetricRepository)
        {
            _queuesMetricRepository = queuesMetricRepository;
        }

        [HttpGet]
        [Route("getallmetrics")]
        public IHttpActionResult GetAllMetrics()
        {
            return Ok(_queuesMetricRepository.GetAllMetrics());
        }

        [HttpGet]
        [Route("getmetricsforhours")]
        public IHttpActionResult GetMetricsForHours(int hour, int interval)
        {
            return Ok(_queuesMetricRepository.GetMetricsForHours(hour, interval));
        }

        [HttpGet]
        [Route("getmetricsbyname")]
        public IHttpActionResult GetMetricsByName(int interval, string metricName)
        {
            return Ok(_queuesMetricRepository.GetMetricsByName(interval, metricName));
        }

        [HttpGet]
        [Route("getmetricsforperiod")]
        public IHttpActionResult GetMetricsForPeriod(DateTime startTime, DateTime endTime, int interval)
        {
            return Ok(_queuesMetricRepository.GetMetricsForPeriod(startTime, endTime, interval));
        }
    }
}