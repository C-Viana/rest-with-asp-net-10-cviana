using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.test.Integration.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Xml;
using System.Xml.Serialization;
using static Bogus.DataSets.Name;

namespace rest_with_asp_net_10_cviana.test.Integration.Person.XML
{
    [TestCaseOrderer(
        TestConfigs.TEST_CASE_ORDERER_FULLNAME,
        TestConfigs.TEST_CASE_ORDERER_ASSEMBLY)]
    public class PersonControllerXmlTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;
        private static PersonDTO? _person;

        public PersonControllerXmlTests(SqlServerFixture sqlFixture)
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
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");
        }

        private static PersonDTO XmlDeserializationFromJsonString(string jsonString)
        {
            var xmlSerializer = new XmlSerializer(typeof(PersonDTO));

            StringReader reader = new(jsonString);
            PersonDTO? dto = xmlSerializer.Deserialize(reader) as PersonDTO;

            return dto!;
        }

        [Fact(DisplayName = "01 - Create Person and read Xml response body")]
        [TestPriority(1)]
        public async Task CreatePerson_ShouldReturnXmlBody()
        {
            var request = PersonTestHelper.CreateRandomPerson();

            var response = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(request));

            response.EnsureSuccessStatusCode();

            //var stringResponseBody = await response.Content.ReadAsStringAsync();
            //PersonDTO xmlPerson = XmlDeserializationFromJsonString(stringResponseBody);
            
            PersonDTO? xmlPerson = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            xmlPerson.Should().NotBeNull();
            xmlPerson.Id.Should().BeGreaterThan(0);
            xmlPerson.FirstName.Should().Be(request.FirstName);
            xmlPerson.LastName.Should().Be(request.LastName);
            xmlPerson.Gender.Should().Be(request.Gender);
            xmlPerson.Address.Should().Be(request.Address);

            _person = xmlPerson;
        }

        [Fact(DisplayName = "02 - Get Person By ID and read Xml response body")]
        [TestPriority(2)]
        public async Task FindPersonById_ShouldReturnXmlBody()
        {
            if(_person == null)
            {
                var createdPerson = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(PersonTestHelper.CreateRandomPerson()));
                var created = XmlDeserializationFromJsonString(await createdPerson.Content.ReadAsStringAsync());
                _person = created;
                _person.Should().NotBeNull();
            }

            var response = await _httpClient.GetAsync($"api/person/v1/{_person.Id}");

            response.EnsureSuccessStatusCode();

            //var stringResponseBody = await response.Content.ReadAsStringAsync();
            //PersonDTO xmlPerson = XmlDeserializationFromJsonString(stringResponseBody);
            PersonDTO? xmlPerson = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            xmlPerson.Should().NotBeNull();
            xmlPerson.Id.Should().Be(_person.Id);
            xmlPerson.FirstName.Should().Be(_person.FirstName);
            xmlPerson.LastName.Should().Be(_person.LastName);
            xmlPerson.Gender.Should().Be(_person.Gender);
            xmlPerson.Address.Should().Be(_person.Address);
        }

        [Fact(DisplayName = "03 - Update Person By ID and read Xml response body")]
        [TestPriority(3)]
        public async Task UpdatePerson_ShouldReturnXmlBody()
        {
            if (_person == null)
            {
                var createdPerson = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(PersonTestHelper.CreateRandomPerson()));
                var created = XmlDeserializationFromJsonString(await createdPerson.Content.ReadAsStringAsync());
                created.Should().NotBeNull();
                _person = created;
            }
            _person.Address = "Silent Hill - Virginia - USA";

            var response = await _httpClient.PutAsync("api/person/v1/", XmlHelper.SerializeToXml(_person));

            response.EnsureSuccessStatusCode();
            //var stringResponseBody = await response.Content.ReadAsStringAsync();
            //PersonDTO xmlPerson = XmlDeserializationFromJsonString(stringResponseBody);
            PersonDTO? xmlPerson = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            xmlPerson.Should().NotBeNull();
            xmlPerson.Id.Should().Be(_person.Id);
            xmlPerson.FirstName.Should().Be(_person.FirstName);
            xmlPerson.LastName.Should().Be(_person.LastName);
            xmlPerson.Address.Should().Be("Silent Hill - Virginia - USA");
        }

        [Fact(DisplayName = "04 - Disable Person By ID and read Xml response body")]
        [TestPriority(4)]
        public async Task DisablePerson_ShouldReturnXmlBody()
        {
            var createdPerson = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(PersonTestHelper.CreateRandomPerson()));
            var created = XmlDeserializationFromJsonString(await createdPerson.Content.ReadAsStringAsync());
            created.Should().NotBeNull();
            _person = created;

            var response = await _httpClient.PatchAsync($"api/person/v1/disable/{_person.Id}", null);

            response.EnsureSuccessStatusCode();
            //var stringResponseBody = await response.Content.ReadAsStringAsync();
            //PersonDTO xmlPerson = XmlDeserializationFromJsonString(stringResponseBody);
            PersonDTO? xmlPerson = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            xmlPerson.Should().NotBeNull();
            xmlPerson.Id.Should().Be(_person.Id);
            xmlPerson.FirstName.Should().Be(_person.FirstName);
            xmlPerson.LastName.Should().Be(_person.LastName);
            xmlPerson.Address.Should().Be(_person.Address);
            xmlPerson.Enabled.Should().Be(false);
        }

        [Fact(DisplayName = "05 - Enable Person By ID and read Xml response body")]
        [TestPriority(5)]
        public async Task EnablePerson_ShouldReturnXmlBody()
        {
            PersonDTO newPerson = PersonTestHelper.CreateRandomPerson();
            newPerson.Enabled = false;
            var createdPerson = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(newPerson));
            var created = XmlDeserializationFromJsonString(await createdPerson.Content.ReadAsStringAsync());
            created.Should().NotBeNull();
            _person = created;
            _person.Enabled.Should().Be(false);

            var response = await _httpClient.PatchAsync($"api/person/v1/enable/{_person.Id}", null);

            response.EnsureSuccessStatusCode();
            //var stringResponseBody = await response.Content.ReadAsStringAsync();
            //PersonDTO xmlPerson = XmlDeserializationFromJsonString(stringResponseBody);
            PersonDTO? xmlPerson = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            xmlPerson.Should().NotBeNull();
            xmlPerson.Id.Should().Be(_person.Id);
            xmlPerson.FirstName.Should().Be(_person.FirstName);
            xmlPerson.LastName.Should().Be(_person.LastName);
            xmlPerson.Address.Should().Be(_person.Address);
            xmlPerson.Enabled.Should().Be(true);
        }

        [Fact(DisplayName = "06 - Delete Person By ID")]
        [TestPriority(6)]
        public async Task DeletePerson_ShouldReturnNoContent()
        {
            if (_person == null)
            {
                var createdPerson = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(PersonTestHelper.CreateRandomPerson()));
                var created = XmlDeserializationFromJsonString(await createdPerson.Content.ReadAsStringAsync());
                created.Should().NotBeNull();
                _person = created;
            }

            var response = await _httpClient.DeleteAsync($"api/person/v1/{_person.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "07 - Find all people")]
        [TestPriority(7)]
        public async Task FindAllPerson_ShouldReturnListOfPeople()
        {
            var response = await _httpClient.GetAsync($"api/person/v1");

            var responseBody = XmlHelper.ReadFromXmlAsync<List<PersonDTO>>(response)?.Result;

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            responseBody.Should().NotBeNull();
            responseBody.Count.Should().BeGreaterThan(0);
            var firstPerson = responseBody.First(p => p.FirstName == "Edmilson");

            firstPerson.Id.Should().Be(1);
            firstPerson.LastName.Should().Be("Garbelini Carneiro");
            firstPerson.Gender.Should().Be("masculino");
            firstPerson.Address.Should().Be("Rua A8, 11, Setor Novo Horizonte, Goiânia/GO");
        }

    }
}