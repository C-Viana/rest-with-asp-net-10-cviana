using rest_with_asp_net_10_cviana.Data.DTO.V1;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace rest_with_asp_net_10_cviana.test.Integration.Tools
{
    public class AuthenticationHelper
    {
        public static UsersDTO SetValidUser()
        {
            return new UsersDTO()
            {
                Username = "edmilsongc",
                Password = "user@1234"
            };
        }

        public static async Task<TokenDTO?> RunSignInAndSetToken(HttpClient _httpClient, UsersDTO _user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/signin", _user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TokenDTO>();
        }
    }
}
