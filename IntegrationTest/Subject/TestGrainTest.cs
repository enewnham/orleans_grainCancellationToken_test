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

            // start the grain up just to make a cleaner test
            using (var cts = new GrainCancellationTokenSource())
            {
                await grain.LongOperation(cts.Token, TimeSpan.FromMilliseconds(1));
            }

            var tasks = Enumerable.Range(0, 5).Select(i => RunWaitCancel(grain));

            await Task.WhenAll(tasks);
        }

        private async Task RunWaitCancel(ITestGrain grain)
        {
            using var tcs = new GrainCancellationTokenSource();
            var task = grain.LongOperation(tcs.Token, TimeSpan.FromMilliseconds(200));

            // Let running task run for a bit with backpressure
            await Task.WhenAny(task, Task.Delay(TimeSpan.FromMilliseconds(100)));

            await tcs.Cancel();

            try
            {
                await task;

                Assert.True(false, "Expected TaskCancelledException, but message completed");
            }
            catch ( TaskCanceledException ) { }
        }
    }
}
