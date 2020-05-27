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
    public class Unit4Service
    {
        public async Task<List<Customer>> GetCustomers(HttpContext httpcontext)
        {
            var token = await httpcontext.GetTokenAsync("access_token");

            var client = new Unit4Client();

            var customers = await client.GetUnit4CustomerAsync("", token);

            //await RefreshAccessToken(httpcontext, refreshTokenClient);

            return customers;
        }

        //public async Task<string> PostCustomers(HttpContext httpContext)
        //{
        //    var token = await httpContext.GetTokenAsync("access_token");

        //    var client = new ExactOnlineClient();

        //    await client.PostRentMagicCustomerAsync("", token);

        //    return "";
        //}

        public async Task<string> RefreshAccessToken(HttpContext httpcontext, HttpClient refreshTokenClient)
        {
            var refreshToken = await httpcontext.GetTokenAsync("refresh_token");

            //var refreshTokenClient = _httpClientFactory.CreateClient();

            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["client_id"] = "f0f36c11-6dd4-425d-9abe-9a69df19989f",
                ["client_secret"] = "2pYS7RWBY77pYDmRNBlIeV5KJZ505xfus66Nbczhxq7A12e7j9"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://sandbox.api.online.unit4.nl/V21/OAuth/Token")
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
