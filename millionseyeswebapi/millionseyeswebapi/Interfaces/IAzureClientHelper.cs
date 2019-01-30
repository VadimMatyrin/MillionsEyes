using Microsoft.Azure.Management.Monitor;
using Microsoft.Azure.Management.Monitor.Models;
using MillionsEyesWebApi.Models.MetricModels;
using System.Threading.Tasks;

namespace MillionsEyesWebApi.Interfaces
{
    public interface IAzureClientHelper
    {
        string ResourseUri { get; }

        Task<MonitorClient> GetMonitorClient();

        T AzureResponseToModel<T>(Response response) where T : MetricModel, new();

    }
}