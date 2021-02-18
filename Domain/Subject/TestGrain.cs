using Orleans;
using System;
using System.Threading.Tasks;

namespace Domain.Subject
{
    public class TestGrain : Grain, ITestGrain
    {
        public async Task<int> LongOperation(GrainCancellationToken ct, TimeSpan delay)
        {
            await Task.Delay(delay, ct.CancellationToken);

            return 1;
        }
    }
}
