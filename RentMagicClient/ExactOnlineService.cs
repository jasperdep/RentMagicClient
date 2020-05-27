using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RentMagicClient

{
    public class ExactOnlineService
    {
        public async Task<List<Customer>> GetCustomers(HttpContext httpcontext)
        {
            var token = await httpcontext.GetTokenAsync("access_token");

            var client = new ExactOnlineClient();

            var customers = await client.GetExactOnlineCustomerAsync("", token);

            //await RefreshAccessToken(httpcontext, refreshTokenClient);

            return customers;
        }

        public async Task<string> PostCustomers(HttpContext httpContext, Customer customer)
        {
            var token = await httpContext.GetTokenAsync("access_token");

            var client = new ExactOnlineClient();

            await client.PostRentMagicCustomerAsync("", token, customer);

            return "";
        }

        public async Task<string> RefreshAccessToken(HttpContext httpcontext, HttpClient refreshTokenClient)
        {
            var refreshToken = await httpcontext.GetTokenAsync("refresh_token");

            //var refreshTokenClient = _httpClientFactory.CreateClient();

            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["client_id"] = "f458b220-7c45-45fd-a352-97a09246426d",
                ["client_secret"] = "yJF2o6BJEqKO"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://start.exactonline.nl/api/oauth2/token")
            {
                Content = new FormUrlEncodedContent(requestData)
            };

            var response = await refreshTokenClient.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

            var newAccessToken = responseData.GetValueOrDefault("access_token");
            var newRefreshToken = responseData.GetValueOrDefault("refresh_token");

            var authInfo = await httpcontext.AuthenticateAsync("ClientCookie");

            authInfo.Properties.UpdateTokenValue("access_token", newAccessToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", newRefreshToken);

            await httpcontext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);

                return "";

        }
    }
}
