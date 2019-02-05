using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.MetricModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MillionsEyesWebApi.Controllers
{
    [RoutePrefix("api/serviceBusMetrics")]
    public class ServiceBusMetricsController : ApiController
    {
        private readonly IMetricsRepository<ServiceBusModel> _serviceBusMetricsRepository;

        public ServiceBusMetricsController(IMetricsRepository<ServiceBusModel> serviceBusMetricsRepository)
        {
            _serviceBusMetricsRepository = serviceBusMetricsRepository;
        }

        [HttpGet]
        [Route("getBusMetricsForHours")]
        public async Task<ServiceBusViewModel> GetBusMetricsForHours(int hour, int interval)
        {
            var currentTime = DateTime.UtcNow;
            var models = await _serviceBusMetricsRepository.GetMetricsAsync(interval, currentTime.AddHours(-hour), currentTime);

            var viewModel = new ServiceBusViewModel
            {
                ServiceBusModels = models.ToList()
            };

            return viewModel;
        }


        [HttpGet]
        [Route("getBusMetrics")]
        public async Task<ServiceBusViewModel> GetBusMetrics(int interval, DateTime? startTime, DateTime? endTime, string metricName = null)
        {
            if (startTime is null || endTime is null)
            {
                var currentTime = DateTime.UtcNow;
                startTime = currentTime;
                endTime = currentTime;
            }

            if (startTime.Value == endTime.Value)
            {
                startTime = startTime.Value.AddHours(-1);
                endTime = endTime.Value.AddHours(23);
            }

            var models = await _serviceBusMetricsRepository.GetMetricsAsync(interval, startTime.Value, endTime.Value, metricName);

            var viewModel = new ServiceBusViewModel
            {
                ServiceBusModels = models.ToList()
            };

            return viewModel;
        }

    }
}
