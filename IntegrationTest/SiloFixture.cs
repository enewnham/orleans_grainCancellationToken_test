using Domain.Subject;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    public class SiloFactory : IDisposable
    {
        private readonly IHost _host;

        public SiloFactory()
        {
            _host = Silo.Program.CreateHostBuilder(Array.Empty<string>()).Build();
            _host.StartAsync().Wait();
        }

        public async Task<IClusterClient> CreateClientAsync()
        {
            IClientBuilder clientBuilder = new ClientBuilder();

            clientBuilder = clientBuilder
                .UseLocalhostClustering();

            IClusterClient client = clientBuilder.Build();
            await client.Connect();

            return client;
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }

    [CollectionDefinition("Fixture")]
    public class EveryFixture : ICollectionFixture<SiloFactory>
    {
    }
}
