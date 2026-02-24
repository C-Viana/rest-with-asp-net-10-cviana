using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.test.Integration.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace rest_with_asp_net_10_cviana.test.Integration.Person.JSON
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
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        [Fact(DisplayName = "01 - Create Person and read Json response body")]
        [TestPriority(1)]
        public async Task CreatePerson_ShouldReturnJsonBody()
        {
            // Arrange
            var request = PersonTestHelper.CreateRandomPerson();

            // Act
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

        [Fact(DisplayName = "02 - Get Person By ID and read Json response body")]
        [TestPriority(2)]
        public async Task FindPersonById_ShouldReturnJsonBody()
        {
            if(_person == null)
            {
                var createdPerson = await _httpClient.PostAsJsonAsync("api/person/v1", PersonTestHelper.CreateRandomPerson());
                var created = await createdPerson.Content.ReadFromJsonAsync<PersonDTO>();
                _person = created;
                _person.Should().NotBeNull();
            }

            var response = await _httpClient.GetAsync($"api/person/v1/{_person.Id}");

            response.EnsureSuccessStatusCode();

            var found = await response.Content.ReadFromJsonAsync<PersonDTO>();

            found.Should().NotBeNull();
            found.Id.Should().Be(_person.Id);
            found.FirstName.Should().Be(_person.FirstName);
            found.LastName.Should().Be(_person.LastName);
            found.Gender.Should().Be(_person.Gender);
            found.Address.Should().Be(_person.Address);
        }

        [Fact(DisplayName = "03 - Update Person By ID and read Json response body")]
        [TestPriority(3)]
        public async Task UpdatePerson_ShouldReturnJsonBody()
        {
            if (_person == null)
            {
                var createdPerson = await _httpClient.PostAsJsonAsync("api/person/v1", PersonTestHelper.CreateRandomPerson());
                var created = await createdPerson.Content.ReadFromJsonAsync<PersonDTO>();
                _person = created;
                _person.Should().NotBeNull();
            }

            _person.Address = "Silent Hill - Virginia - USA";

            var response = await _httpClient.PutAsJsonAsync("api/person/v1/", _person);
            var edited = await response.Content.ReadFromJsonAsync<PersonDTO>();

            edited.Should().NotBeNull();
            edited.Id.Should().Be(_person.Id);
            edited.FirstName.Should().Be(_person.FirstName);
            edited.LastName.Should().Be(_person.LastName);
            edited.Address.Should().Be("Silent Hill - Virginia - USA");
        }

        [Fact(DisplayName = "04 - Disable Person By ID and read Json response body")]
        [TestPriority(4)]
        public async Task DisablePerson_ShouldReturnJsonBody()
        {
            if (_person == null)
            {
                var createdPerson = await _httpClient.PostAsJsonAsync("api/person/v1", PersonTestHelper.CreateRandomPerson());
                var created = await createdPerson.Content.ReadFromJsonAsync<PersonDTO>();
                _person = created;
                _person.Should().NotBeNull();
            }

            var response = await _httpClient.PatchAsync($"api/person/v1/disable/{_person.Id}", null);
            var personDisabled = await response.Content.ReadFromJsonAsync<PersonDTO>();

            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK);
            personDisabled.Should().NotBeNull();
            personDisabled.Id.Should().Be(_person.Id);
            personDisabled.FirstName.Should().Be(_person.FirstName);
            personDisabled.LastName.Should().Be(_person.LastName);
            personDisabled.Address.Should().Be(_person.Address);
            personDisabled.Enabled.Should().BeFalse();
        }

        [Fact(DisplayName = "05 - Enable Person By ID and read Json response body")]
        [TestPriority(5)]
        public async Task EnablePerson_ShouldReturnJsonBody()
        {
            if (_person == null)
            {
                PersonDTO newPerson = PersonTestHelper.CreateRandomPerson();
                newPerson.Enabled = false;
                var createdPerson = await _httpClient.PostAsJsonAsync("api/person/v1", newPerson);
                var created = await createdPerson.Content.ReadFromJsonAsync<PersonDTO>();
                _person = created;
                _person.Should().NotBeNull();
                _person.Enabled.Should().BeFalse();
            }

            var response = await _httpClient.PatchAsync($"api/person/v1/enable/{_person.Id}", null);
            var personEnabled = await response.Content.ReadFromJsonAsync<PersonDTO>();

            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK);
            personEnabled.Should().NotBeNull();
            personEnabled.Id.Should().Be(_person.Id);
            personEnabled.FirstName.Should().Be(_person.FirstName);
            personEnabled.LastName.Should().Be(_person.LastName);
            personEnabled.Address.Should().Be(_person.Address);
            personEnabled.Enabled.Should().BeTrue();
        }

        [Fact(DisplayName = "06 - Delete Person By ID")]
        [TestPriority(6)]
        public async Task DeletePerson_ShouldReturnNoContent()
        {
            if (_person == null)
            {
                var createdPerson = await _httpClient.PostAsJsonAsync("api/person/v1", PersonTestHelper.CreateRandomPerson());
                var created = await createdPerson.Content.ReadFromJsonAsync<PersonDTO>();
                _person = created;
                _person.Should().NotBeNull();
            }

            var response = await _httpClient.DeleteAsync($"api/person/v1/{_person.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            _person = null;
        }

        [Fact(DisplayName = "07 - Find all people")]
        [TestPriority(7)]
        public async Task FindAllPerson_ShouldReturnListOfPeople()
        {
            var response = await _httpClient.GetAsync($"api/person/v1");
            var responseBody = await response.Content.ReadFromJsonAsync<List<PersonDTO>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            responseBody.Should().NotBeNull();
            responseBody.Count.Should().BeGreaterThan(0);
            var firstPerson = responseBody.First( p => p.FirstName == "Edmilson");

            firstPerson.Id.Should().Be(1);
            firstPerson.LastName.Should().Be("Garbelini Carneiro");
            firstPerson.Gender.Should().Be("masculino");
            firstPerson.Address.Should().Be("Rua A8, 11, Setor Novo Horizonte, Goiânia/GO");
        }

        [Fact(DisplayName = "08 - Find all people with paged search")]
        [TestPriority(8)]
        public async Task FindAllPerson_ShouldReturnPageOfPeople()
        {
            var response = await _httpClient.GetAsync($"api/person/v1/1/10/asc");
            var responsePage = await response.Content.ReadFromJsonAsync<PagedSearch<PersonDTO>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            responsePage.Should().NotBeNull();
            responsePage.CurrentPage.Should().Be(1);
            responsePage.PageSize.Should().Be(10);
            responsePage.TotalResults.Should().BeGreaterThan(0);
            responsePage.SortDirection.Should().NotBeNull();

            var firstPerson = responsePage.Items.First();

            firstPerson.Id.Should().BeGreaterThan(0);
            firstPerson.FirstName.Should().NotBeNull();
            firstPerson.LastName.Should().NotBeNull();
            firstPerson.Gender.Should().NotBeNull();
            firstPerson.Address.Should().NotBeNull();
        }

    }
}