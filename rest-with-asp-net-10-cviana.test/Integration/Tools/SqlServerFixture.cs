using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net_10_cviana.Configurations;
using Testcontainers.MsSql;

namespace rest_with_asp_net_10_cviana.test.Integration.Tools
{
    public class SqlServerFixture : IAsyncLifetime
    {
        public MsSqlContainer Container { get; }
        public string ConnectionString => Container.GetConnectionString();

        public SqlServerFixture()
        {
            Container = new MsSqlBuilder()
                .WithPassword("@Admin123$")
                //.WithPortBinding(0, 1433) //Cria uma porta randômica para o acesso do host e fixa a porta 1433 no container
                .Build();
        }

        public async Task InitializeAsync()
        {
            await Container.StartAsync();
            EvolveConfig.ExecuteMigrations(ConnectionString);
        }

        public async Task DisposeAsync()
        {
            await Container.DisposeAsync();
        }

        public static HttpClient GetDefaultTestingClient(SqlServerFixture sqlFixture)
        {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            return factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost")
                }
            );
        }

    }
}
