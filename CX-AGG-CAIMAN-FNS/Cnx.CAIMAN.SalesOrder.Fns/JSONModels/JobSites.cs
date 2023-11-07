using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.JSONModels
{
    public class MinimumLOT
    {
        public int Amount { get; set; }
        public string UoM { get; set; }
    }

    public class MaximumLOT
    {
        public int Amount { get; set; }
        public string UoM { get; set; }
    }

    public class MaximumDeliveryVolume
    {
        public int Amount { get; set; }
        public string UoM { get; set; }
    }

    public class MaximumVehicleWeight
    {
        public int Amount { get; set; }
        public string UoM { get; set; }
    }

    public class MaximumVehicleVolume
    {
        public int Amount { get; set; }
        public string UoM { get; set; }
    }

    public class SalesPersonData
    {
        public string SalesPNumber { get; set; }
        public string SalesPName { get; set; }
        public string SalesPEmail { get; set; }
    }

    public class JobSiteSalesAreaData
    {
        public string SalesAreaId_Organization { get; set; }
        public string SalesAreaId_DistChannel { get; set; }
        public string SalesAreaId_Division { get; set; }
        public string UserCreatedSalesArea { get; set; }
        public DateTime DateCreatedSalesArea { get; set; }
        public string OrderBlock { get; set; }
        public string DeliveryBlock { get; set; }
        public string BillingBlock { get; set; }
        public string MarkForDeleteSalesArea { get; set; }
        public string ShippingCondition { get; set; }
        public string DefaultPlant { get; set; }
        public string PaymentTerm { get; set; }
        public string CustomerPONumber { get; set; }
        public string Hierarchy_C { get; set; }
        public string Hierarchy_R { get; set; }
        public string CreationDateInSalesArea { get; set; }
        public string PaperlessFlag { get; set; }
        public SalesPersonData SalesPersonData { get; set; }
    }

    public class JobSite
    {
        public string JobSiteCode { get; set; }
        public string JobSiteDesc { get; set; }
        public string AddressId { get; set; }
        public string CountryCode { get; set; }
        public string RegionId { get; set; }
        public string CategoryCode { get; set; }
        public string CustomerId { get; set; }
        public string JobSiteProject { get; set; }
        public string JobSiteReference { get; set; }
        public string JobsiteRequestCode { get; set; }
        public string UserCreated { get; set; }
        public DateTime DateCreated { get; set; }
        public string OrderBlock { get; set; }
        public string DeliveryBlock { get; set; }
        public string BillingBlock { get; set; }
        public string MarkForDelete { get; set; }
        public string JobSiteDesc2 { get; set; }
        public string CreationDate { get; set; }
        public string Action { get; set; }
        //public string Epod { get; set; }
        public List<JobSiteSalesAreaData> JobSiteSalesAreaData { get; set; }
        public Address Address { get; set; }
    }

    public class MTServiceBusJobSite
    {
        public string SystemSource { get; set; }
        public string InterfaceID { get; set; }
        public string MessageID { get; set; }
        public string QUEUEID { get; set; }
        public DateTime MessageTimeStamp { get; set; }
        public TimeLog TimeLog { get; set; }
        public List<JobSite> JobSites { get; set; }
    }
}
