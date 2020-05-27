using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using AutoMapper;
using Newtonsoft.Json.Linq;
using OauthClient.Controllers;
using System.Net;

namespace RentMagicClient
{
    public class Unit4Client
    {
        public HttpClient client = new HttpClient();

        public async Task<List<Customer>> GetUnit4CustomerAsync(string path, string token)
        {
            List<Customer> customers = null;

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            //Rootobject container = null;
            var result = await client.GetAsync("https://sandbox.api.online.unit4.nl/V21/api/MVL71239/CustomerInfoList");
            //if (result.IsSuccessStatusCode)
            //{
            //    //exactonlinecustomer = await response.Content.ReadAsAsync<ExactOnlineCustomer>();
            //    var request = await result.Content.ReadAsStringAsync();
            //    var rootObject = JsonConvert.DeserializeObject<RootObject>(request);
            //    //customers = MapCustomers(rootObject.D.Results);
            //}
            return customers;
        }
    }
}
