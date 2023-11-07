using Cnx.CAIMAN.VendorCustomer.Fns.Core.Models;
using Cnx.CAIMAN.VendorCustomer.Fns.JSONModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns.Helpers
{
    public class MappingModels
    {
        public static OrigenInterfaz_DTO MappingRootToOrigenInterfaz(VendorData item)
        {
            OrigenInterfaz_DTO outputItem = new OrigenInterfaz_DTO();

            outputItem.vcBorrar = item.GeneralDeleteFlag;
            outputItem.vcSAP = item.VendorCode;
            outputItem.vc50Nombre = item.VendorName;

            return outputItem;
        }
        public static DestinoInterfaz_DTO MappingRootToDestinoInterfaz(Customer item)
        {
            DestinoInterfaz_DTO outputItem = new DestinoInterfaz_DTO();
            outputItem.vcSAP = item.CustomerCode;
            outputItem.vc50Nombre = item.CustomerDesc;
            outputItem.vcBorrar = item.MarkForDelete;
            
            return outputItem;
        }

        public static DestinoInterfaz_DTO MappingRootToDestinoInterfaz(JobSite item)
        {
            DestinoInterfaz_DTO outputItem = new DestinoInterfaz_DTO();
            outputItem.vcSAP = item.JobSiteCode;
            outputItem.vc50Nombre = item.JobSiteDesc;
            outputItem.vcBorrar = item.MarkForDelete;
            outputItem.vcZonaEnt = item?.AddressList.FirstOrDefault()?.TransportationZone;

            return outputItem;
        }

    }
}
