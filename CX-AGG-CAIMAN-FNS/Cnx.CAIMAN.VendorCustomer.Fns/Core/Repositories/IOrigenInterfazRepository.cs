using Cnx.CAIMAN.VendorCustomer.Fns.Core.Models;
using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns.Core.Repositories
{
    public interface IOrigenInterfazRepository
    {
        Task<Result<OrigenInterfaz_DTO>> InsertOrigenInterfaz(OrigenInterfaz_DTO item);
    }
}
