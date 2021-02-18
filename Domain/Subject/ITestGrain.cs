using Orleans;
using System;
using System.Threading.Tasks;

namespace Domain.Subject
{
    public interface ITestGrain : IGrainWithIntegerKey
    {
        Task<int> LongOperation( GrainCancellationToken ct, TimeSpan delay );
    }
}