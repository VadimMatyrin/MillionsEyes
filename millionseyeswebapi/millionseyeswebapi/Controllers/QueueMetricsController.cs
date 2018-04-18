using MillionsEyesWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MillionsEyesWebApi.Controllers
{
    [RoutePrefix("api/queuemetrics")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "POST, PUT, DELETE, OPTIONS")]
    public class QueueMetricsController : ApiController
    {
        private readonly IQueuesMetricRepository _queuesMetricRepository;

        public QueueMetricsController(IQueuesMetricRepository queuesMetricRepository)
        {
            _queuesMetricRepository = queuesMetricRepository;
        }

        [HttpGet]
        [Route("getallmetrics")]
        public string GetAllMetrics()
        {
            Task<string> message = _queuesMetricRepository.GetAllMetrics();
            IncomingMetrics metric = _queuesMetricRepository.DeserializeToObject(message.Result);
            List<QueueMetric> metricModels = _queuesMetricRepository.CreateMetricModel(metric);
            string result = _queuesMetricRepository.SerializeToJson(metricModels);
            return result;
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