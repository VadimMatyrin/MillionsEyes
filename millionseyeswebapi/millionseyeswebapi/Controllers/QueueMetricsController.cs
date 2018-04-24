using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.QueuesViewModel;
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
            Task<string> message = _queuesMetricRepository.GetAllMetrics();
            IncomingMetrics metric = _queuesMetricRepository.DeserializeToObject(message.Result);
            QueueMetricViewModel metricModels = _queuesMetricRepository.CreateMetricModel(metric);
            return Ok(metricModels);
        }

        [HttpGet]
        [Route("(getmetrics")]
        public Task<string> GetMetrics([FromBody] GetMetricModel model)
        {
            Task<string> message = _queuesMetricRepository.GetMetrics(model);
            return message;
        }
    }
}