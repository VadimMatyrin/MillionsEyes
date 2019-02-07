using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.QueuesModels;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using static MillionsEyesWebApi.Properties.Settings;

namespace MillionsEyesWebApi.Controllers
{
    [RoutePrefix("api/queueMetrics")]
    public class QueueMetricsController : ApiController
    {
        private readonly IMetricsRepository<QueueMetricModel> _queuesMetricRepository;

        public QueueMetricsController(IMetricsRepository<QueueMetricModel> queuesMetricRepository)
        {
            _queuesMetricRepository = queuesMetricRepository;
        }

        [HttpGet]
        [Route("getMetricsForHours")]
        public async Task<QueueMetricViewModel> GetMetricsForHours(int hour, int interval)
        {
            if (hour <= 0 || interval <= 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var currentTime = DateTime.UtcNow;
            var models = await _queuesMetricRepository.GetMetricsAsync(interval, currentTime.AddHours(-hour), currentTime);

            var viewModel = new QueueMetricViewModel
            {
                QueueMetrics = models.ToList()
            };

            return viewModel;
        }

        [HttpGet]
        [Route("getMetrics")]
        public async Task<QueueMetricViewModel> GetMetrics(int interval, DateTime? startTime, DateTime? endTime, string metricName = null)
        {
            if (startTime is null || endTime is null || interval <= 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (!(metricName is null) && !Default.ServiceBusMetricsList.Contains(metricName))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (startTime.Value == endTime.Value)
                endTime = endTime.Value.AddHours(24);

            var models = await _queuesMetricRepository.GetMetricsAsync(interval, startTime.Value, endTime.Value, metricName);

            var viewModel = new QueueMetricViewModel
            {
                QueueMetrics = models.ToList()
            };

            return viewModel;
        }
    }
}