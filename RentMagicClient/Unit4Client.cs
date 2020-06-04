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
using RentMagicClient;
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

            var result = await client.GetAsync("https://sandbox.api.online.unit4.nl/V21/api/MVL71239/CustomerInfoList");
            if (result.IsSuccessStatusCode)
            {
                List<Unit4Customer> unit4Customers = await result.Content.ReadAsAsync<List<Unit4Customer>>();

                customers = MapCustomers(unit4Customers);
            }
            return customers;
        }

        public async Task<string> PostRentMagicCustomerAsync(string path, string token, Customer customer)
        {

            Unit4Customer unit4Customer = null;

            unit4Customer = MapUnit4Customer(customer);

            var stringContent = new StringContent(JsonConvert.SerializeObject(unit4Customer), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var result = await client.PostAsync("https://sandbox.api.online.unit4.nl/V21/api/MVL71239/Customer", stringContent);

            return "";
        }

        private List<Customer> MapCustomers(List<Unit4Customer> unit4Customers)
        {
            return unit4Customers.ConvertAll(unit4Customers => new Customer
            {
                CustomerID = unit4Customers.customerId,
                City = unit4Customers.city,
                CompanyName = unit4Customers.name,
                Email = unit4Customers.email,
                HouseNumber = unit4Customers.street2,
                HouseNumberAddition = unit4Customers.street2,
                LanguageID = unit4Customers.languageId,
                Street = unit4Customers.street2,
                Tel = unit4Customers.telephone,
                CountryID = unit4Customers.countryId,
                ZipCode = unit4Customers.zipCode,
                FullName = unit4Customers.contactPerson
            });
        }

        private Unit4Customer MapUnit4Customer(Customer customer)
        {
            string name = customer.CompanyName;

            string noSpace = name.Replace(" ", "");

            string maxEight = noSpace.Substring(0, 8);

            string newShortName = maxEight.ToUpper();


            Unit4Customer unit4Customer = new Unit4Customer()
            {
                customerId = customer.CustomerID,
                city = customer.City,
                name = customer.CompanyName,
                email = customer.Email,
                countryId = customer.CountryID,
                street2 = customer.Street + " " + customer.HouseNumber + " " + customer.HouseNumberAddition,
                languageId = customer.LanguageID,
                telephone = customer.Tel,
                zipCode = customer.ZipCode,
                shortName = newShortName,
                person = customer.Salutation + " " + customer.FirstName + " " + customer.LastName
            };

            return unit4Customer;
        }
    }
}
