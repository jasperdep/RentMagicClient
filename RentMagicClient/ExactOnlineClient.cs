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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OauthClient.Controllers;

namespace RentMagicClient
{
    public class EOContainer
    {
        public List<ExactOnlineCustomer> d;
    }

    public class ExactOnlineClient
    {
        public HttpClient client = new HttpClient();

        public async Task<ExactOnlineCustomer> GetExactOnlineCustomerAsync(string path, string token)
        {

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IkN0VHVoTUptRDVNN0RMZHpEMnYyeDNRS1NSWSIsImtpZCI6IkN0VHVoTUptRDVNN0RMZHpEMnYyeDNRS1NSWSJ9.eyJhdWQiOiJodHRwczovL2luZm9kYXRlazAuY3JtNC5keW5hbWljcy5jb20iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC80MWY1MzVmYi1hOGZiLTRmYjgtYjRjMS1iMWQxYTI4NmE3MDQvIiwiaWF0IjoxNTg5NDUxNDY4LCJuYmYiOjE1ODk0NTE0NjgsImV4cCI6MTU4OTQ1NTM2OCwiYWNyIjoiMSIsImFpbyI6IjQyZGdZRWgvT0tzclhsZ3BRZnpzTjFZeFNaczhlUmFldEZjSjBScEJFaFgxNzg2ZTlnQUEiLCJhbXIiOlsicHdkIl0sImFwcGlkIjoiYjhmZTBmYjQtOTMzMC00OWMxLTkzZjAtMmNmY2RhZDJhMDEwIiwiYXBwaWRhY3IiOiIwIiwiZmFtaWx5X25hbWUiOiJkZSBQb290ZXIiLCJnaXZlbl9uYW1lIjoiSmFzcGVyIiwiaXBhZGRyIjoiMjE3LjYzLjY3LjIzMyIsIm5hbWUiOiJKYXNwZXIgZGUgUG9vdGVyIHwgSW5mb2RhdGVrIEdyb2VwIiwib2lkIjoiMDU3MmU2ZjMtYTE1MC00OTgzLTk1NmItZjdmYWQzMDA2ZjU2IiwicHVpZCI6IjEwMDMyMDAwOTQ3NTNCNjkiLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJzdWIiOiI0NUZmOFhXbmNIZm9aUlNDVDRJZlNlU0Nfc3MwNy13Rm5nR3ZEZW5kMmhBIiwidGlkIjoiNDFmNTM1ZmItYThmYi00ZmI4LWI0YzEtYjFkMWEyODZhNzA0IiwidW5pcXVlX25hbWUiOiJqZGVwb290ZXJAaW5mb2RhdGVrLmNvbSIsInVwbiI6ImpkZXBvb3RlckBpbmZvZGF0ZWsuY29tIiwidXRpIjoicW5TVm5jdGYyVVNLa1hUdWJ2cHlBQSIsInZlciI6IjEuMCJ9.GZBmD-69TrngtoBED_E2naEa36zexhGAYQkE6NDeUEVKija1YaLOkf8LhpkF_Q7QTZ2IcaJDB17B_XNXZzyKO_ZqNw2u2x0GLLLFgj8BqFUf09uCEkGxAihxN9Hf6_HQWvHQdLPS7dpn4WM-XtXkQz9DdlhmTUxzmvm-NnGuwsPr1A_h_lWPiBucaOZ0oaNBMVq6OGcH6eAjpoVLx7RHwvIC887ieBm53eUj7XmKWvSn4oXJ_FZ2LWg6gFFpu0YjaL9Nixq6m92pd9-bmNoiYJpFbhJTHWI8Jm9x1Yv1l3-o-WoHKQ-3aOOrGTB-1plpZM_RlyqEYpM_tNUU0XixAQ");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            EOContainer exactonlinecustomer = null;
            var result = await client.GetAsync("https://infodatek0.crm4.dynamics.com/api/data/v9.0/contacts");
            if (result.IsSuccessStatusCode)
            {
                //exactonlinecustomer = await response.Content.ReadAsAsync<ExactOnlineCustomer>();
                var request = await result.Content.ReadAsStringAsync();
                exactonlinecustomer = JsonConvert.DeserializeObject<EOContainer>(request);
            }
            return null;

            //var serverClient = _httpClientFactory.CreateClient();

            //serverClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            //serverClient.DefaultRequestHeaders.Add("Accept", "application/json");

            //var serverResponse = await serverClient.GetAsync("https://start.exactonline.nl/api/v1/2741128/bulk/CRM/Contacts?$top=1");

            //var request = await serverClient.GetAsync("https://start.exactonline.nl/api/v1/2741128/bulk/CRM/Contacts?$top=1");

            //var reponseBody = await request.Content.ReadAsStringAsync();

        }
    }
}
