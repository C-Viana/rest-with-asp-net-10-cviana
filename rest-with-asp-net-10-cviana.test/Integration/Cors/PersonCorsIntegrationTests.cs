using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.test.Integration.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace rest_with_asp_net_10_cviana.test.Integration.Cors
{
    [TestCaseOrderer(
        TestConfigs.TEST_CASE_ORDERER_FULLNAME,
        TestConfigs.TEST_CASE_ORDERER_ASSEMBLY)]
    public class PersonControllerJsonTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;
        private static PersonDTO? _person;

        public PersonControllerJsonTests(SqlServerFixture sqlFixture)
        {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost")
                }
            );
            string? token = AuthenticationHelper.RunSignInAndSetToken(_httpClient, AuthenticationHelper.SetValidUser()).Result?.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private void AddOriginHeader(string origin)
        {
            _httpClient.DefaultRequestHeaders.Remove("Origin");
            _httpClient.DefaultRequestHeaders.Add("Origin", origin);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        [Fact(DisplayName = "01 - Create Person With Allowed Origin")]
        [TestPriority(1)]
        public async Task CreatePerson_WithAllowedOrigin_ShouldReturnCreated()
        {
            // Arrange
            AddOriginHeader("https://erudio.com.br");

            // Act
            var request = PersonTestHelper.CreateRandomPerson();
            var response = await _httpClient.PostAsJsonAsync("api/person/v1", request);

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<PersonDTO>();
            created.Should().NotBeNull();
            created.Id.Should().BeGreaterThan(0);
            created.FirstName.Should().Be(request.FirstName);
            created.LastName.Should().Be(request.LastName);
            created.Gender.Should().Be(request.Gender);
            created.Address.Should().Be(request.Address);

            _person = created;
        }

        [Fact(DisplayName = "02 - Create Person With Disallowed Origin")]
        [TestPriority(2)]
        public async Task CreatePerson_WithDisallowedOrigin_ShouldReturnForbiden()
        {
            // Arrange
            AddOriginHeader("https://semeru.com.br");

            var request = PersonTestHelper.CreateRandomPerson();

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/person/v1", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("CORS policy not allowed");
        }

        [Fact(DisplayName = "03 - Get Person By ID With Allowed Origin")]
        [TestPriority(3)]
        public async Task FindPersonById_WithAllowedOrigin_ShouldReturnOk()
        {
            // Arrange
            AddOriginHeader("https://erudio.com.br");

            // Act
            if(_person == null)
            {
                var createdPerson = await _httpClient.PostAsJsonAsync("api/person/v1", PersonTestHelper.CreateRandomPerson());
                var created = await createdPerson.Content.ReadFromJsonAsync<PersonDTO>();
                created.Should().NotBeNull();
                _person = created;
            }

            var response = await _httpClient.GetAsync($"api/person/v1/{_person.Id}");

            // Assert
            response.EnsureSuccessStatusCode();

            var found = await response.Content.ReadFromJsonAsync<PersonDTO>();

            found.Should().NotBeNull();
            found.Id.Should().Be(_person.Id);
            found.FirstName.Should().Be(_person.FirstName);
            found.LastName.Should().Be(_person.LastName);
            found.Gender.Should().Be(_person.Gender);
            found.Address.Should().Be(_person.Address);
        }

        [Fact(DisplayName = "04 - Get Person By ID With Disallowed Origin")]
        [TestPriority(4)]
        public async Task FindByIdPerson_WithDisallowedOrigin_ShouldReturnForbiden()
        {
            // Arrange
            AddOriginHeader("https://semeru.com.br");

            // Act
            var response = await _httpClient.GetAsync($"api/person/v1/{_person!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("CORS policy not allowed");
        }

        [Fact(DisplayName = "05 - Update Person By ID With Allowed Origin")]
        [TestPriority(5)]
        public async Task UpdatePerson_WithAllowedOrigin_ShouldReturnOk()
        {
            // Arrange
            AddOriginHeader("https://erudio.com.br");

            if (_person == null)
            {
                var createdPerson = await _httpClient.PostAsJsonAsync("api/person/v1", PersonTestHelper.CreateRandomPerson());
                var created = await createdPerson.Content.ReadFromJsonAsync<PersonDTO>();
                created.Should().NotBeNull();
                _person = created;
            }
            _person.Address = "Silent Hill - Virginia - USA";

            // Act

            var response = await _httpClient.PutAsJsonAsync("api/person/v1/", _person);
            var edited = await response.Content.ReadFromJsonAsync<PersonDTO>();

            // Assert
            edited.Should().NotBeNull();
            edited.Id.Should().Be(_person.Id);
            edited.FirstName.Should().Be(_person.FirstName);
            edited.LastName.Should().Be(_person.LastName);
            edited.Address.Should().Be("Silent Hill - Virginia - USA");
        }

        [Fact(DisplayName = "06 - Update Person By ID With Disallowed Origin")]
        [TestPriority(6)]
        public async Task UpdatePerson_WithDisallowedOrigin_ShouldReturnForbiden()
        {
            // Arrange
            AddOriginHeader("https://semeru.com.br");

            // Act
            var response = await _httpClient.PutAsJsonAsync("api/person/v1/", _person);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            var content = await response.Content
                .ReadAsStringAsync();
            content.Should().Be("CORS policy not allowed");
        }

        [Fact(DisplayName = "07 - Delete Person By ID With Allowed Origin")]
        [TestPriority(7)]
        public async Task DeletePerson_WithAllowedOrigin_ShouldReturnNoContent()
        {
            // Arrange
            AddOriginHeader("https://erudio.com.br");

            // Act
            if (_person == null)
            {
                var createdPerson = await _httpClient.PostAsJsonAsync("api/person/v1", PersonTestHelper.CreateRandomPerson());
                var created = await createdPerson.Content.ReadFromJsonAsync<PersonDTO>();
                created.Should().NotBeNull();
                _person = created;
            }

            var response = await _httpClient.DeleteAsync($"api/person/v1/{_person.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            _person = null;
        }

        [Fact(DisplayName = "08 - Delete Person By ID With Disallowed Origin")]
        [TestPriority(8)]
        public async Task DeletePerson_WithDisallowedOrigin_ShouldReturnForbiden()
        {
            // Arrange
            AddOriginHeader("https://semeru.com.br");

            // Act
            var response = await _httpClient
                .DeleteAsync($"api/person/v1/{_person!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            var content = await response.Content
                .ReadAsStringAsync();
            content.Should().Be("CORS policy not allowed");
        }


    }
}