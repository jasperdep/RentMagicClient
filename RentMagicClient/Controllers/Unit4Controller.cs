﻿using System;
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

namespace RentMagicClient.Controllers
{
    public class Unit4Controller : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public Unit4Controller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Overzicht()
        {
            var httpcontext = HttpContext;

            //var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            //var service = new ExactOnlineService();

            var refreshTokenClient = _httpClientFactory.CreateClient();

            //var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

            //await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);

            var service = new Unit4Service();

            await service.RefreshAccessToken(httpcontext, refreshTokenClient);

            var customers = await service.GetCustomers(httpcontext);

            //await service.PostCustomers(httpcontext);

            //await service.RefreshAccessToken(httpcontext, refreshTokenClient);

            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyName,FirstName,LastName,Salutation,Street,HouseNumber,HouseNumberAddition,Email,CountryID,Tel,LanguageID,City,ZipCode,State")] Customer customer)
        {

            if (ModelState.IsValid)
            {
                var httpcontext = HttpContext;

                var cust = customer;

                var service = new Unit4Service();

                await service.PostCustomers(httpcontext, customer);

                return RedirectToAction(nameof(Overzicht));
            }
            return View(customer);
        }
    }
}
