using FluentAssertions;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Hypermedia.Constants;
using rest_with_asp_net_10_cviana.test.Integration.Tools;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace rest_with_asp_net_10_cviana.test.Integration.Hateoas
{
    public class BookControllerHateoasTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;

        public BookControllerHateoasTests(SqlServerFixture sqlFixture)
        {
            _httpClient = SqlServerFixture.GetDefaultTestingClient(sqlFixture);
            string? token = AuthenticationHelper.RunSignInAndSetToken(_httpClient, AuthenticationHelper.SetValidUser()).Result?.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private static void ValidateHypermediaLinks(BookDTO book)
        {
            book.Links.Should().HaveCount(7);
            book.Links[0].Rel.Should().Be(RelationType.COLLECTION);
            book.Links[1].Rel.Should().Be(RelationType.SELF);
            book.Links[2].Rel.Should().Be(RelationType.CREATE);
            book.Links[3].Rel.Should().Be(RelationType.UPDATE);
            book.Links[6].Rel.Should().Be(RelationType.DELETE);
        }

        private async Task<BookDTO> GetNewSavedBookAsync()
        {
            var request = BookTestHelper.CreateRandomBook();
            var response = await _httpClient.PostAsJsonAsync("api/book/v1", request);
            response.EnsureSuccessStatusCode();
            var createdBook = await response.Content.ReadFromJsonAsync<BookDTO>();
            createdBook.Should().NotBeNull();
            return createdBook;
        }

        [Fact(DisplayName = "POST - Creating book returns hypermedia links")]
        public async Task CreateBook_ShouldReturnHypermediaLinks()
        {
            ValidateHypermediaLinks(await GetNewSavedBookAsync());
        }

        [Fact(DisplayName = "GET - Fetching book returns hypermedia links")]
        public async Task GetBook_ShouldReturnHypermediaLinks()
        {
            var response = await _httpClient.GetAsync("api/book/v1/1");

            response.EnsureSuccessStatusCode();

            var fetchedBook = await response.Content.ReadFromJsonAsync<BookDTO>();
            fetchedBook.Should().NotBeNull();
            fetchedBook.Id.Should().Be(1);

            ValidateHypermediaLinks(fetchedBook);
        }

        [Fact(DisplayName = "PUT - Updating book returns hypermedia links")]
        public async Task PutBook_ShouldReturnHypermediaLinks()
        {
            var created = await GetNewSavedBookAsync();
            created.Price = 99.99M;

            var updatedRes = await _httpClient.PutAsJsonAsync("api/book/v1", created);
            var updatedBook = await updatedRes.Content.ReadFromJsonAsync<BookDTO>();
            updatedBook.Should().NotBeNull();

            ValidateHypermediaLinks(updatedBook);
        }

    }
}
