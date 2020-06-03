using System;
using System.Collections.Generic;

namespace RentMagicClient
{
    public class Unit4Customer
    {
        public string customerId { get; set; }

        public string street2 { get; set; }

        public string email { get; set; }

        public string countryId { get; set; }

        public string languageId { get; set; }

        public string city { get; set; }

        public string name { get; set; }

        public string contactPerson { get; set; }

        public string telephone { get; set; }

        public string homepage { get; set; }

        public string zipCode { get; set; }

        public string shortName { get; set; }
    }

    public class RootUnit4
    {
        public List<Unit4Customer> Results { get; set; }
    }
}
