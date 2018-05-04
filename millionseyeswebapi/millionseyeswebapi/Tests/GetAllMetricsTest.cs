using MillionsEyesWebApi.Helpers;
using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Repository;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Configuration;

namespace MillionsEyesWebApi.Tests
{
    [TestFixture]
    public class GetAllMetricsTest
    {
        IQueuesMetricRepository _queueRepo;
        HttpClientHelper _helper;
        string tenantId = WebConfigurationManager.AppSettings["tenantId"];
        string clientId = WebConfigurationManager.AppSettings["clientId"];
        string secret = WebConfigurationManager.AppSettings["secret"];

        [SetUp]
        public void SetUp()
        {
            _helper = new HttpClientHelper(tenantId, clientId, secret);
            _queueRepo = new QueuesMetricRepository(_helper);

            /*
                var queueMock = new Mock<IQueuesMetricRepository>();
                queueMock.Setup(m => m.GetAllMetrics()).Returns(new QueueMetricViewModel().QueueMetrics;
                _queueRepo = queueMock.Object;
                */
        }


        [Test]
        public void GetAllMetrics()
        {
            List<string> test = new List<string> { "timespan", "puller", "optimizer" };
            var result = _queueRepo.GetAllMetrics();
            Assert.AreEqual(result.QueueMetrics[0].QueueName, "adminservice");
        }

        [Test]
        public void GetMetricsByName()
        {
            var result = _queueRepo.GetMetricsByName(15, "IncomingMessages");
            Assert.AreEqual(result.QueueMetrics[0].QueueName, "puller");
        }

        [Test]
        public void GetMetricsForHours()
        {
            var result = _queueRepo.GetMetricsForHours(1, 15);
            Assert.AreEqual(result.QueueMetrics[0].QueueName, "adminservice");
        }
    }
}