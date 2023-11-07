using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.JSONModels
{
    public class ITEM
    {
        public string ActionItem { get; set; }
        public string TicketId { get; set; }
        public string TicketDetailCode { get; set; }
        public string CountryCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductDesc { get; set; }
        public string UnitId { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductTypeDesc { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string ProofOfDelivery { get; set; }
        public string OrderIdItem { get; set; }
        public string ProdPresentationDesc { get; set; }
        public string AREAID { get; set; }
        public int ZONEID { get; set; }


        // public string ActionItem { get; set; }
        public string OrderCode { get; set; }
        public int ItemSeqNum { get; set; }
        // public string ProductCode { get; set; }
        // public string ProductDesc { get; set; }
        public string MaterialType { get; set; }
        public string CommercialProductNumber { get; set; }
        public string CommercialProductDescription { get; set; }
        public int HighLevelItemSeqNum { get; set; }
        public string ProductQuantity { get; set; }
        // public string UnitId { get; set; }
        public string PlantId { get; set; }
        public string LoadNumber { get; set; }
        // public string CountryCode { get; set; }
        public string ReferenceDocumentNumberI { get; set; }
        public string ReferenceDocumentItemI { get; set; }
        public string ReferenceDocumentTypeI { get; set; }
        // public DateTime DeliveryDateTime { get; set; }
        public DateTime LoadingDateTime { get; set; }
        public string UnitaryPrice { get; set; }
        public string ExtendedPrice { get; set; }
        public string Currency { get; set; }
        public string RejectionCode { get; set; }
        public string DeliveryGroup { get; set; }
        public string Taxes { get; set; }
        public string TotalAmmount { get; set; }
        public string PurchaseOrderType { get; set; }
        public string ShippingType { get; set; }
        public string ProductHierarchy { get; set; }
        public string TransporationGroup { get; set; }
        public string PaymentTerm { get; set; }
        public List<SchedLine> SchedLines { get; set; }

    }
}
