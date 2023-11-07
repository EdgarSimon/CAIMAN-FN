using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.JSONModels
{
    public class Header
    {
        public string Action { get; set; }
        public string OrderCode { get; set; }
        public string OrderCat { get; set; }
        public string OrderTypeId { get; set; }
        public string SalesAreaId_Organization { get; set; }
        public string SalesAreaId_DistChannel { get; set; }
        public string SalesAreaId_Division { get; set; }
        public string ShippingConditionId { get; set; }
        public string PurchaseOrder { get; set; }
        public string CustomerId { get; set; }
        public string JobSiteId { get; set; }
        public string PointofDeliveryId { get; set; }
        public string CountryCode { get; set; }
        public string ContactRequester { get; set; }
        public string ContactReceiver { get; set; }
        public string AddressId { get; set; }
        public int RouteTimeNum { get; set; }
        public int SpacingNum { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string InstructionsDesc { get; set; }
        public string ReferenceDocumentNumber { get; set; }
        public string ReferenceDocumentType { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string LastUpdateByUser { get; set; }
        public string Element { get; set; }
        public string Slump { get; set; }
        public string SlumpDescription { get; set; }
        public string PaymentTerm { get; set; }
        public string PurchaseOrderType { get; set; }
        public string ShippingType { get; set; }
        public string JobsiteTimeZone { get; set; }
        public string CallTypeCode { get; set; }
        public List<ITEM> ITEMS { get; set; }
    }

    public class MTSalesDocument
    {
        public string SystemSource { get; set; }
        public string InterfaceID { get; set; }
        public string MessageID { get; set; }
        public string QUEUEID { get; set; }
        public string EventType { get; set; }
        public TimeLog TimeLog { get; set; }
        public Header Header { get; set; }
    }

    public class SchedLine
    {
        public string ActionSchedLines { get; set; }
        public string OrderCode { get; set; }
        public int ItemSeqNum { get; set; }
        public string SchedLinNum { get; set; }
        public string SchedLinCat { get; set; }
        public string SchedLinRel { get; set; }
        public string OrderQuantity { get; set; }
        public string ConfQuantity { get; set; }
        public string ReqQuantity { get; set; }
        public DateTime DeliveryDateTimeSL { get; set; }
        public DateTime LoadingDateTimeSL { get; set; }
    }

}
