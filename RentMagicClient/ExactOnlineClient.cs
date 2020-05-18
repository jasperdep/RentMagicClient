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

namespace RentMagicClient
{
    public class EOContainer
    {
        public List<ExactOnlineCustomer> d;
    }

    //public class DivisionContainer
    //{
    //    public List<Division> d;
    //}

    public class ExactOnlineClient
    {
        public HttpClient client = new HttpClient();

        public async Task<List<Customer>> GetExactOnlineCustomerAsync(string path, string token)
        {
            List<Customer> customers = null;

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            EOContainer container = null;
            var result = await client.GetAsync("https://start.exactonline.nl/api/v1/2741128/bulk/CRM/Accounts?$top=1");
            if (result.IsSuccessStatusCode)
            {
                //exactonlinecustomer = await response.Content.ReadAsAsync<ExactOnlineCustomer>();
                var request = await result.Content.ReadAsStringAsync();
                container = JsonConvert.DeserializeObject<EOContainer>(request);

                customers = MapCustomers(container.d);
            }
            return customers;
        }

        private List<Customer> MapCustomers(List<ExactOnlineCustomer> exactOnlineCustomers)
        {
            return exactOnlineCustomers.ConvertAll(eoCustomer => new Customer
            {

                CustomerID = eoCustomer.ID,
                City = eoCustomer.City,
                CompanyName = eoCustomer.Name,
                CountryID = eoCustomer.Country,
                Email = eoCustomer.Email,
                HouseNumber = eoCustomer.AddressLine1,
                HouseNumberAddition = eoCustomer.AddressLine1,
                LanguageID = eoCustomer.Language,
                Street = eoCustomer.AddressLine1,
                Tel = eoCustomer.Phone,
                ZipCode = eoCustomer.Postcode,
                State = eoCustomer.StateName

            });
        }

        //public async Task<string> GetCurrentDivision(string token)
        //{
        //    //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        //    //client.DefaultRequestHeaders.Add("Accept", "application/json");

        //    DivisionContainer currentdivision = null;
        //    var result = await client.GetAsync("https://start.exactonline.nl/api/v1/current/Me?$select=CurrentDivision");
        //    if (result.IsSuccessStatusCode)
        //    {
        //        var request = await result.Content.ReadAsStringAsync();
        //    }

        //    {
        //        //exactonlinecustomer = await response.Content.ReadAsAsync<ExactOnlineCustomer>();
        //        var request = await result.Content.ReadAsStringAsync();
        //        currentdivision = JsonConvert.DeserializeObject<DivisionContainer>(request);
        //    }
        //    return null;
        //}
    }
}
