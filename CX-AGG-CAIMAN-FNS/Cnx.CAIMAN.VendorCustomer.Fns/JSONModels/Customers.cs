using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns.JSONModels
{
    public class CustSalesAreaData
    {
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public string PaymentTerms { get; set; }
        public string RateCode { get; set; }
        public string RateDescription { get; set; }
        public string MarkForDeleteSalesArea { get; set; }
        public string OrderBlock { get; set; }
        public string DeliveryBlock { get; set; }
        public string BillingBlock { get; set; }
        public string OwnFreightFlag { get; set; }
        public string SalesOffice { get; set; }
        public string CustomerCurrency { get; set; }
        public string CreationDateInSalesArea { get; set; }
    }

    public class CustPartnerData
    {
        public string SalesOrganization { get; set; }
        public string DistributionChannel { get; set; }
        public string Division { get; set; }
        public string PartnerType { get; set; }
        public string PartnerNumber { get; set; }
    }

    public class CustCompanyData
    {
        public string CompanyCode { get; set; }
        public string PostingBlock { get; set; }
        public string DeletionFlag { get; set; }
        public string HeadOffice { get; set; }
        public string AlternativePayer { get; set; }
        public string PaymentMethods { get; set; }
        public string PaymentBlock { get; set; }
        public string TermsOfPayment { get; set; }
        public string HouseBank { get; set; }
        public string ArPledging { get; set; }
    }



    public class Customer
    {
        //Campos para function de archivo
        public int CustomerId { get; set; }
        public string CustomeName { get; set; }
        ///////////////////////////////////////////////////////////////
        public string CustomerCode { get; set; }
        public string CustomerDesc { get; set; }
        public string AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string VAT { get; set; }
        public string CountryCode { get; set; }
        public string RegionId { get; set; }
        public string PartyId { get; set; }
        public string MarkForDelete { get; set; }
        public string OrderBlock { get; set; }
        public string DeliveryBlock { get; set; }
        public string BillingBlock { get; set; }
        public string CustomerDesc2 { get; set; }
        public string Hierarchy_G { get; set; }
        public string CreationDate { get; set; }
        public string Action { get; set; }
        public List<CustSalesAreaData> CustSalesAreaData { get; set; }
        public List<CustPartnerData> CustPartnerData { get; set; }
        public List<CustCompanyData> CustCompanyData { get; set; }
        
    }

    public class CustomerAddressList : Customer
    {
        public List<Address> Address { get; set; }
    }
    public class CustomerSingleAddress : Customer
    {
        public Address Address { get; set; }
    }

    public class MTServiceBusCustomer
    {
        public string SystemSource { get; set; }
        public string InterfaceID { get; set; }
        public string MessageID { get; set; }
        public string QUEUEID { get; set; }
        public DateTime MessageTimeStamp { get; set; }
        public TimeLog TimeLog { get; set; }
        public List<Customer> CustomersList { get; set; }

    }

    public class MTServiceBusCustomerAddressList : MTServiceBusCustomer
    {
        public List<CustomerAddressList> Customers { get; set; }
    }

    public class MTServiceBusCustomerSingleAddress : MTServiceBusCustomer
    {
        public List<CustomerSingleAddress> Customers { get; set; }
    }
}
