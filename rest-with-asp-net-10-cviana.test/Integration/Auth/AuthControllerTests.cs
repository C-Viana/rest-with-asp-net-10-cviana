using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.test.Integration.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace rest_with_asp_net_10_cviana.test.Integration.Auth
{
    [TestCaseOrderer(
        TestConfigs.TEST_CASE_ORDERER_FULLNAME,
        TestConfigs.TEST_CASE_ORDERER_ASSEMBLY)]
    public class AuthControllerTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;
        private static AccountCredentialsDTO? _accountCredentials;
        private static UsersDTO? _user;
        private static TokenDTO? _token;

        public AuthControllerTests(SqlServerFixture sqlFixture)
        {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost")
                }
            );
        }

        [Fact(DisplayName = "01 - Must create an user successfully")]
        [TestPriority(1)]
        public async Task CreateUser_ShouldReturnJsonBody()
        {
            _accountCredentials = new()
            {
                Username = "owkenobi",
                Fullname = "Obi-Wan Kenobi",
                Password = "user@1234"
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/create", _accountCredentials);

            response.EnsureSuccessStatusCode();

            var generatedUser = await response.Content.ReadFromJsonAsync<AccountCredentialsDTO>();

            generatedUser.Should().NotBeNull();
            generatedUser.Username.Should().Be(_accountCredentials.Username);
            generatedUser.Fullname.Should().Be(_accountCredentials.Fullname);
            generatedUser.Password.Should().NotBeNull();
        }

        [Fact(DisplayName = "02 - Must authorize access successfully")]
        [TestPriority(2)]
        public async Task SignIn_ShouldReturnSuccess()
        {
            _user = AuthenticationHelper.SetValidUser();

            var response = await _httpClient.PostAsJsonAsync("api/auth/signin", _user);
            response.EnsureSuccessStatusCode();
            var generatedToken = await response.Content.ReadFromJsonAsync<TokenDTO>();

            generatedToken.Should().NotBeNull();
            generatedToken.Authenticated.Should().Be(true);
            generatedToken.AccessToken.Should().NotBeNull();
            generatedToken.RefreshToken.Should().NotBeNull();
            generatedToken.Created.Should().NotBeNull();
            generatedToken.Expiration.Should().NotBeNull();

            _token = generatedToken;
        }

        [Fact(DisplayName = "03 - Must renew access successfully")]
        [TestPriority(3)]
        public async Task Refresh_ShouldReturnSuccess()
        {
            _user = AuthenticationHelper.SetValidUser();
            _token = AuthenticationHelper.RunSignInAndSetToken(_httpClient, _user)?.Result;

            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", _token);
            response.EnsureSuccessStatusCode();
            var generatedToken = await response.Content.ReadFromJsonAsync<TokenDTO>();

            generatedToken.Should().NotBeNull();
            generatedToken.Authenticated.Should().Be(true);
            generatedToken.AccessToken.Should().NotBeNull();
            generatedToken.RefreshToken.Should().NotBeNull();
            generatedToken.Created.Should().NotBeNull();
            generatedToken.Expiration.Should().NotBeNull();

            _token = generatedToken;
        }

        [Fact(DisplayName = "04 - Must revoke access successfully")]
        [TestPriority(4)]
        public async Task Revoke_ShouldReturnSuccess()
        {
            if(_token == null)
            {
                _user = AuthenticationHelper.SetValidUser();
                _token = AuthenticationHelper.RunSignInAndSetToken(_httpClient, _user)?.Result;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);
            var response = await _httpClient.PostAsync("api/auth/revoke", null);
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "05 - Sign In with invalid credentials should return unauthorized")]
        [TestPriority(5)]
        public async Task Signin_InvalidCredentials_ShouldReturnUnauthorized()
        {
            _user = AuthenticationHelper.SetValidUser();
            _user.Password = "wrong@password";

            var response = await _httpClient.PostAsJsonAsync("api/auth/signin", _user);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact(DisplayName = "06 - Revoke access without authorization header should return unauthorized")]
        [TestPriority(6)]
        public async Task Revoke_WithoutHeader_ShouldReturnUnauthorized()
        {
            if (_token == null)
            {
                _user = AuthenticationHelper.SetValidUser();
                _token = AuthenticationHelper.RunSignInAndSetToken(_httpClient, _user)?.Result;
            }

            _httpClient.DefaultRequestHeaders.Authorization = null;
            var response = await _httpClient.PostAsync("api/auth/revoke", null);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}
