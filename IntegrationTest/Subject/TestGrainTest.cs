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
            GrainCancellationTokenSource cts1 = new GrainCancellationTokenSource();
            GrainCancellationTokenSource cts2 = new GrainCancellationTokenSource();
            ITestGrain grain = _clusterClient.GetGrain<ITestGrain>(123);

            // Start a running task. Don't cancel this one
            var runningTask = Task.Run(() => grain.LongOperation(cts1.Token, TimeSpan.FromSeconds(5)));

            // Let running task run seccond with backpressure
            await Task.WhenAny(runningTask, Task.Delay(TimeSpan.FromMilliseconds(100)));

            // Queue a task. This is the one to cancel
            var queuedTask = Task.Run(() => grain.LongOperation(cts2.Token, TimeSpan.FromSeconds(5)));

            // Let both tasks run seccond with backpressure
            await Task.WhenAny(runningTask, queuedTask, Task.Delay(TimeSpan.FromMilliseconds(100)));

            await cts2.Cancel();

            await Task.WhenAny( runningTask, Assert.ThrowsAsync<TaskCanceledException>(() => queuedTask) );
        }
    }
}
