using MillionsEyesWebApi.Helpers;
using MillionsEyesWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonExtractor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var helper = new HttpClientHelper(Settings.TenantId, Settings.ClientId, Settings.Secret);
            var repository = new QueuesMetricRepository(helper);
            Console.WriteLine(await repository.GetMetricsAsync());
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
