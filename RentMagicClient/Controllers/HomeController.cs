using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RentMagicClient;

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
        public async Task<IActionResult> Exact()
        {
            var httpcontext = HttpContext;

            //var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            //var service = new ExactOnlineService();

            var refreshTokenClient = _httpClientFactory.CreateClient();

            //var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

            //await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);

            var service = new ExactOnlineService();

            await service.GetCustomers(httpcontext);

            //await service.PostCustomers(httpcontext);

            await service.RefreshAccessToken(httpcontext, refreshTokenClient);

            return View();
        }
    }
}
