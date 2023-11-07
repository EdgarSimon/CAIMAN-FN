using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.JSONModels
{
    public class Address
    {
        public string AddressCode { get; set; }
        public string CityDesc { get; set; }
        public string SettlementDesc { get; set; }
        public string CityCode { get; set; }
        public string ConurbationCode { get; set; }
        public string ConurbationDesc { get; set; }
        public string PostalCode { get; set; }
        public string StreetName { get; set; }
        public string DomicileNum { get; set; }
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string RegionDesc { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string TimeZone { get; set; }
        public string TransportationZone { get; set; }
    }
}
