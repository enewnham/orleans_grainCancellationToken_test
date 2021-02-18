using Domain.Subject;
using Orleans;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.Subject
{
    [Collection("Fixture")]
    public class TestGrainTest
    {
        private readonly SiloFactory _siloFactory;
        private readonly IClusterClient _clusterClient;

        public TestGrainTest(SiloFactory siloFactory)
        {
            _siloFactory = siloFactory;
            _clusterClient = siloFactory.CreateClientAsync().Result;
        }

        [Fact]
        public async Task TestLongOperationAsync()
        {
            ITestGrain grain = _clusterClient.GetGrain<ITestGrain>(123);

            GrainCancellationTokenSource cts = new GrainCancellationTokenSource();
            await cts.Cancel();

            await Assert.ThrowsAsync<TimeoutException>(() => grain.LongOperation(cts.Token, TimeSpan.FromSeconds(5)));
        }
    }
}
