using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns.JSONModels
{
    public class Characteristic
    {
        public string ProductCode { get; set; }
        public string ProductClass { get; set; }
        public string ProductCharacteristic { get; set; }
        public string ProductCharacteristicDesc { get; set; }
        public string ProductCharacteristicValue { get; set; }
    }

    public class MTServiceBusProduct
    {
        public string fromCountryCode { get; set; }
        public string systemSource { get; set; }
        public string interfaceId { get; set; }
        public string messageId { get; set; }
        public string QUEUEID { get; set; }
        public DateTime MessageTimeStamp { get; set; }
        public TimeLog TimeLog { get; set; }
        public List<Product> products { get; set; }
    }

    public class Plant
    {
        public string plantCode { get; set; }
        public string countryCode { get; set; }
        public string availabilityCheck { get; set; }
        public string flagforDelete { get; set; }
        public string plantSpecMaterialStatus { get; set; }
    }

    public class Product
    {
        public string productCode { get; set; }
        public string unitId { get; set; }
        public string divisionCode { get; set; }
        public string grossWeight { get; set; }
        public string netWeight { get; set; }
        public string weightUnit { get; set; }
        public string netWeightUnitId { get; set; }
        public string volume { get; set; }
        public string volumeUnitId { get; set; }
        public string productHierarchy { get; set; }
        public string transportationGroup { get; set; }
        public string materialType { get; set; }
        public string materialGroup { get; set; }
        public string flagforDelete { get; set; }
        public string manufactPartNum { get; set; }
        public string numberManufact { get; set; }
        public string nameManufact { get; set; }
        public List<Plant> plants { get; set; }
        public List<SalesArea> salesAreas { get; set; }
        public List<ProductDesc> productDesc { get; set; }
        public List<Unit> Units { get; set; }
        public List<Characteristic> Characteristic { get; set; }
    }

    public class ProductDesc
    {
        public string languageCode { get; set; }
        public string description { get; set; }
    }

    public class SalesArea
    {
        public string salesOrganizationCode { get; set; }
        public string channelCode { get; set; }
        public string countryCode { get; set; }
        public string flagforDelete { get; set; }
    }

    public class Unit
    {
        public string alternativeUnitCode { get; set; }
        public string denominatorConversion { get; set; }
        public string numeratorConversion { get; set; }
    }
}
