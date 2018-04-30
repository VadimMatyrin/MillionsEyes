using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.QueuesViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            List<string> messages = _queuesMetricRepository.GetAllMetrics();
            List<IncomingMetrics> metrics = _queuesMetricRepository.DeserializeToObject(messages);
            QueueMetricViewModel metricModels = _queuesMetricRepository.CreateMetricModel(metrics);
            return Ok(metricModels);
        }

        [HttpGet]
        [Route("getmetricsforhours")]
        public IHttpActionResult GetMetricsForHours(int hour, int interval)
        {
            List<string> messages = _queuesMetricRepository.GetMetricsForHours(hour, interval);
            List<IncomingMetrics> metrics = _queuesMetricRepository.DeserializeToObject(messages);
            QueueMetricViewModel metricModels = _queuesMetricRepository.CreateMetricModel(metrics);
            return Ok(metricModels);
        }

        [HttpGet]
        [Route("getmetricsforperiod")]
        public IHttpActionResult GetMetricsForPeriod(DateTime startTime, DateTime endTime, int interval)
        {
            List<string> messages = _queuesMetricRepository.GetMetricsForPeriod(startTime, endTime, interval);
            List<IncomingMetrics> metrics = _queuesMetricRepository.DeserializeToObject(messages);
            QueueMetricViewModel metricModels = _queuesMetricRepository.CreateMetricModel(metrics);
            return Ok(metricModels);
        }
    }
}