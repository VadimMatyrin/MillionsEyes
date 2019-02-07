using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Models.MetricModels;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using static MillionsEyesWebApi.Properties.Settings;

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
            if (hour <= 0 || interval <= 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

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
            if (startTime is null || endTime is null || interval <= 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            if(!(metricName is null) && !Default.ServiceBusMetricsList.Contains(metricName))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (startTime.Value == endTime.Value)
                endTime = endTime.Value.AddHours(24);

            var models = await _serviceBusMetricsRepository.GetMetricsAsync(interval, startTime.Value, endTime.Value, metricName);

            var viewModel = new ServiceBusViewModel
            {
                ServiceBusModels = models.ToList()
            };

            return viewModel;
        }

    }
}
