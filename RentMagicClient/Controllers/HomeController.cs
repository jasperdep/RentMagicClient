using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OauthClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var serverClient = _httpClientFactory.CreateClient();

            serverClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var serverResponse = await serverClient.GetAsync("https://start.exactonline.nl/api/v1/2741128/bulk/CRM/Contacts?$top=1");

            await RefreshAccessToken();

            var apiClient = _httpClientFactory.CreateClient();

            token = await HttpContext.GetTokenAsync("access_token");

            apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var apiResponse = await apiClient.GetAsync("https://localhost:5001/home/secret");

            return View();
        }

        public async Task<string> RefreshAccessToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var refreshTokenClient = _httpClientFactory.CreateClient();

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

            var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

            authInfo.Properties.UpdateTokenValue("access_token", newAccessToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", newRefreshToken);

            await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);

            return "";

        }
    }
}
