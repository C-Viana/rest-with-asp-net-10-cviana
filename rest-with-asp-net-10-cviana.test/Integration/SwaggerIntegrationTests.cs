using rest_with_asp_net_10_cviana.test.Integration.Tools;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

namespace rest_with_asp_net_10_cviana.test.Integration
{
    public class SwaggerIntegrationTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;

        public SwaggerIntegrationTests(SqlServerFixture fixture)
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
        public async Task Get_Swagger_ShouldReturnSwaggerJson()
        {
            var response = await _client.GetAsync("/swagger/v1/swagger.json");
            
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNull();
            content.Should().Contain("\"openapi\": \"3.0.4\"");
            content.Should().Contain("\"title\": \"Curso Erudio ASP.NET 2026 por Leandro Costa\"");
        }

        [Fact]
        public async Task Get_Swagger_ShouldReturnSwaggerUi()
        {
            var response = await _client.GetAsync("/swagger-ui/index.html");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNull();
            content.Should().Contain("<div id=\"swagger-ui\">");
        }
    }
}
