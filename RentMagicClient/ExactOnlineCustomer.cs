using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RentMagicClient
{
    public class ExactOnlineCustomer
    {
        public string ID { get; set; }

        public string AddressLine1 { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string StateName { get; set; }

        public string Language { get; set; }

        public string City { get; set; }

        public string Name { get; set; }

        public string Created { get; set; }

        public string Website { get; set; }

        public string Phone { get; set; }

        public string Postcode { get; set; }

    }

    public class RootObject
    {
        [JsonProperty("d")]
        public Result D { get; set; }
    }

    public class Result
    {
        [JsonProperty("results")]
        public List<ExactOnlineCustomer> Results { get; set; }
    }
}
