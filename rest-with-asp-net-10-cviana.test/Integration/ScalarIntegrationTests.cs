using rest_with_asp_net_10_cviana.test.Integration.Tools;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

namespace rest_with_asp_net_10_cviana.test.Integration
{
    public class ScalarIntegrationTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;

        public ScalarIntegrationTests(SqlServerFixture fixture)
        {
            var factory = new CustomWebApplicationFactory<Program>(fixture.ConnectionString);
            _client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost")
                }
            );
        }

        [Fact]
        public async Task Get_Scalar_ShouldReturnScalarUi()
        {
            var response = await _client.GetAsync("/scalar");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNull();
            content.Should().Contain("<script src=\"scalar.js\"></script>");
        }
    }
}
