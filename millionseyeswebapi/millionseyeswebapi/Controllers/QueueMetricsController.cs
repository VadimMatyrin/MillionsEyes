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
            var currentDate = DateTime.UtcNow;
            var models = await _queuesMetricRepository.GetMetricsAsync(interval, currentDate.AddHours(-hour), currentDate);

            var viewModel = new QueueMetricViewModel
            {
                QueueMetrics = models.ToList()
            };

            return viewModel;
        }

        [HttpGet]
        [Route("getMetrics")]
        public async Task<QueueMetricViewModel> GetMetrics(int interval = 1, DateTime? startTime = null, DateTime? endTime = null, string metricName = null)
        {
            if (startTime == endTime)
            {
                startTime = startTime?.AddHours(-1);
                endTime = endTime?.AddHours(23);
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