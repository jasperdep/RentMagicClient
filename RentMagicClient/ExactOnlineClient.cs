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
using RentMagicClient.Controllers;
using System.Net;

namespace RentMagicClient
{
    //public class EOContainer
    //{
    //    public List<ExactOnlineCustomer> d;
    //}

    public class ExactOnlineClient
    {
        public HttpClient client = new HttpClient();

        public async Task<List<Customer>> GetExactOnlineCustomerAsync(string path, string token)
        {
            List<Customer> customers = null;

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            //Rootobject container = null;
            var result = await client.GetAsync("https://start.exactonline.nl/api/v1/2757837/crm/Accounts?$select=ID,AddressLine1,City,Country,Created,Email,Language,Name,Phone,Postcode,StateName,Website");
            if (result.IsSuccessStatusCode)
            {
                //exactonlinecustomer = await response.Content.ReadAsAsync<ExactOnlineCustomer>();
                var request = await result.Content.ReadAsStringAsync();
                var rootObject = JsonConvert.DeserializeObject<RootObject>(request);
                customers = MapCustomers(rootObject.D.Results);
            }
            return customers;
        }

        public async Task<string> PostRentMagicCustomerAsync(string path, string token, Customer customer)
        {

            ExactOnlineCustomer exactOnlineCustomer = null;

            exactOnlineCustomer = MapExactOnlineCustomer(customer);

            var stringContent = new StringContent(JsonConvert.SerializeObject(exactOnlineCustomer), Encoding.UTF8, "application/json");
 
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var result = await client.PostAsync("https://start.exactonline.nl/api/v1/2757837/crm/Accounts", stringContent);

            return "";
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

        private ExactOnlineCustomer MapExactOnlineCustomer(Customer customer)
        {
            ExactOnlineCustomer exactOnlineCustomer = new ExactOnlineCustomer()
            {

                ID = customer.CustomerID,
                City = customer.City,
                Name = customer.CompanyName,
                Country = customer.CountryID,
                Email = customer.Email,
                AddressLine1 = customer.Street + " " + customer.HouseNumber + customer.HouseNumberAddition,
                Language = customer.LanguageID,
                Phone = customer.Tel,
                Postcode = customer.ZipCode,
                StateName = customer.State,
                Status = "C"
            };

            return exactOnlineCustomer;
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
