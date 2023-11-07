using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.JSONModels
{
    public class Root
    {
        public MTServiceBusCustomer MT_ServiceBusCustomer { get; set; }
        public MTServiceBusJobSite MT_ServiceBusJobSite { get; set; }
        public MTServiceBusProduct MT_ServiceBusProduct { get; set; }
        public MTSalesDocument MT_SalesDocument { get; set; }
        public MT_VendorsSupliers MT_VendorsSupliers { get; set; }
    }
}
