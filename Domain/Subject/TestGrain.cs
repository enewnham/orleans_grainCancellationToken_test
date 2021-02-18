using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Domain.Subject
{
    public class TestGrain : Grain, ITestGrain
    {
        private readonly ILogger<TestGrain> _logger;

        public TestGrain(ILogger<TestGrain> logger)
        {
            _logger = logger;
        }
        public async Task LongOperation(GrainCancellationToken ct, TimeSpan delay)
        {
            _logger.LogInformation("Start LongOperation {delay}", delay);

            await Task.Delay(delay, ct.CancellationToken);

            _logger.LogInformation("End LongOperation");
        }
    }
}
