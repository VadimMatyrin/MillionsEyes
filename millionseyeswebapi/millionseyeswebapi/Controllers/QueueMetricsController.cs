using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.QueuesModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

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
            if (startTime is null || endTime is null)
            {
                var currentTime = DateTime.UtcNow;
                startTime = currentTime;
                endTime = currentTime;
            }

            if (startTime == endTime)
            {
                startTime = startTime.Value.AddHours(-1);
                endTime = endTime.Value.AddHours(23);
            }

            var models = await _queuesMetricRepository.GetMetricsAsync(interval, startTime.Value, endTime.Value, metricName);

            var viewModel = new QueueMetricViewModel
            {
                QueueMetrics = models.ToList()
            };

            return viewModel;
        }
    }
}