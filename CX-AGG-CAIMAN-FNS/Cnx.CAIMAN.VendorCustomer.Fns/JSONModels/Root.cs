using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns.JSONModels
{
    public class Root
    {
        public MTServiceBusCustomer MT_ServiceBusCustomer { get; set; }
        public MTServiceBusJobSite MT_ServiceBusJobSite { get; set; }
        public MTSalesDocument MT_SalesDocument { get; set; }
        public MT_VendorsSupliers MT_VendorsSupliers { get; set; }
    }

    public class RootAddressList
    {
        public MTServiceBusCustomerAddressList MT_ServiceBusCustomer { get; set; }
        public MTServiceBusJobSiteAdressList MT_ServiceBusJobSite { get; set; }
    }

    public class RootSingleAddress
    {
        public MTServiceBusCustomerSingleAddress MT_ServiceBusCustomer { get; set; }
        public MTServiceBusJobSiteSingleAdress MT_ServiceBusJobSite { get; set; }
    }
}
