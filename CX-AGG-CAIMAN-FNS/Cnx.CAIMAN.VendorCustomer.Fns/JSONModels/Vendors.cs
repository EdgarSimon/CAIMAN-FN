using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns.JSONModels
{
    public class MT_VendorsSupliers
    {
        public string SystemSource { get; set; }
        public string InterfaceID { get; set; }
        public string MessageID { get; set; }
        public string QUEUEID { get; set; }
        public string EventType { get; set; }
        public string TruckRFID { get; set; }
        public Timelog TimeLog { get; set; }
        public VendorData[] VendorData { get; set; }
    }
    public class Timelog
    {
        public DateTime SourceSystemTime { get; set; }
        public DateTime PITimeReceived { get; set; }
        public DateTime PIProcessingTime { get; set; }
    }
    public class VendorData
    {
        public string ObjectKey { get; set; }
        public string VendorCode { get; set; }
        public string Country { get; set; }
        public string VendorName { get; set; }
        public string VendorName2 { get; set; }
        public string AccountGroup { get; set; }
        public string SearchTerm1 { get; set; }
        public string SearchTerm2 { get; set; }
        public string AccountingBlock { get; set; }
        public string PurchaseBlock { get; set; }
        public string QualityBlock { get; set; }
        public string GeneralDeleteFlag { get; set; }
        public AddressData AddressData { get; set; }
        public ContactInformation[] ContactInformation { get; set; }
        public CompanyData[] CompanyData { get; set; }
        public PurchasingOrganizationData PurchasingOrganizationData { get; set; }
    }
    public class AddressData
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string eMailAddress { get; set; }
    }
    public class ContactInformation
    {
        public string Department { get; set; }
        public string Function { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

    }
    public class CompanyData
    {
        public int CompanyCode { get; set; }
        public string PaymentTerm { get; set; }
        public string PaymentTermDesc { get; set; }
        public string AccountingCompanyBlock { get; set; }
        public string CompanyDeleteFlag { get; set; }
    }
    public class PurchasingOrganizationData
    {
        public string PurchaseOrganization { get; set; }
        public string OrderCurrency { get; set; }
        public string PurchaseOrgBlock { get; set; }
        public string PurchaseOrgDeleteFlag { get; set; }
    }
}
