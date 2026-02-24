using FluentAssertions;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Hypermedia.Constants;
using rest_with_asp_net_10_cviana.test.Integration.Tools;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace rest_with_asp_net_10_cviana.test.Integration.Hateoas
{
    public class PersonControllerHateoasTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;

        public PersonControllerHateoasTests(SqlServerFixture sqlFixture)
        {
            _httpClient = SqlServerFixture.GetDefaultTestingClient(sqlFixture);
            string? token = AuthenticationHelper.RunSignInAndSetToken(_httpClient, AuthenticationHelper.SetValidUser()).Result?.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private static void ValidateHypermediaLinks(PersonDTO person)
        {
            person.Links.Should().HaveCount(7);
            person.Links[0].Rel.Should().Be(RelationType.COLLECTION);
            person.Links[1].Rel.Should().Be(RelationType.SELF);
            person.Links[2].Rel.Should().Be(RelationType.CREATE);
            person.Links[3].Rel.Should().Be(RelationType.UPDATE);
            person.Links[4].Rel.Should().Be(RelationType.PATCH);
            person.Links[5].Rel.Should().Be(RelationType.PATCH);
            person.Links[6].Rel.Should().Be(RelationType.DELETE);
        }

        private async Task<PersonDTO> GetNewSavedPersonAsync()
        {
            var request = PersonTestHelper.CreateRandomPerson();
            var response = await _httpClient.PostAsJsonAsync("api/person/v1", request);
            response.EnsureSuccessStatusCode();
            var createdPerson = await response.Content.ReadFromJsonAsync<PersonDTO>();
            createdPerson.Should().NotBeNull();
            return createdPerson;
        }

        [Fact(DisplayName = "POST - Creating person returns hypermedia links")]
        public async Task CreatePerson_ShouldReturnHypermediaLinks()
        {
            ValidateHypermediaLinks(await GetNewSavedPersonAsync());
        }

        [Fact(DisplayName = "GET - Fetching person returns hypermedia links")]
        public async Task GetPerson_ShouldReturnHypermediaLinks()
        {
            var response = await _httpClient.GetAsync("api/person/v1/1");

            response.EnsureSuccessStatusCode();

            var fetchedPerson = await response.Content.ReadFromJsonAsync<PersonDTO>();
            fetchedPerson.Should().NotBeNull();
            fetchedPerson.Id.Should().Be(1);

            ValidateHypermediaLinks(fetchedPerson);
        }

        [Fact(DisplayName = "PUT - Updating person returns hypermedia links")]
        public async Task PutPerson_ShouldReturnHypermediaLinks()
        {
            var created = await GetNewSavedPersonAsync();
            created.Address = "Raccon City - Oregon/USA";

            var updatedRes = await _httpClient.PutAsJsonAsync("api/person/v1", created);
            var updatedPerson = await updatedRes.Content.ReadFromJsonAsync<PersonDTO>();
            updatedPerson.Should().NotBeNull();

            ValidateHypermediaLinks(updatedPerson);
        }

        [Fact(DisplayName = "PATCH - Disabling person returns hypermedia links")]
        public async Task PatchPerson_ShouldReturnHypermediaLinks()
        {
            var created = await GetNewSavedPersonAsync();
            created.Enabled.Should().BeTrue();

            var updatedRes = await _httpClient.PatchAsync($"api/person/v1/disable/{created.Id}", null);
            var disabledPerson = await updatedRes.Content.ReadFromJsonAsync<PersonDTO>();
            disabledPerson.Should().NotBeNull();

            ValidateHypermediaLinks(disabledPerson);
        }

    }
}
