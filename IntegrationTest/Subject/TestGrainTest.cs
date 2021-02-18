using Domain.Subject;
using Orleans;
using System;
using System.Linq;
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
        public async Task TestCancel()
        {
            GrainCancellationTokenSource cts = new GrainCancellationTokenSource();
            ITestGrain grain = _clusterClient.GetGrain<ITestGrain>(123);

            await cts.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => grain.LongOperation(cts.Token, TimeSpan.FromSeconds(5)));
        }

        [Fact]
        public async Task TestCancel_WhenQueued()
        {
            ITestGrain grain = _clusterClient.GetGrain<ITestGrain>(123);

            var tasks = Enumerable.Range(0, 5).Select(i => RunWaitCancel(grain));

            var exs = await Task.WhenAll(tasks);

            Assert.All(exs, ex => Assert.NotNull(ex));
        }

        private async Task<TaskCanceledException> RunWaitCancel(ITestGrain grain)
        {
            try
            {
                GrainCancellationTokenSource cts = new GrainCancellationTokenSource();
                var task = grain.LongOperation(cts.Token, TimeSpan.FromMilliseconds(200));

                // Let running task run seccond with backpressure
                await Task.WhenAny(task, Task.Delay(TimeSpan.FromMilliseconds(100)));

                await cts.Cancel();

                await task;

                return null;
            }
            catch( TaskCanceledException ex )
            {
                return ex;
            }
        }
    }
}
