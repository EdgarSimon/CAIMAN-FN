using Cnx.CAIMAN.VendorCustomer.Fns.Core.Models;
using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns.Core.Repositories
{
    public interface IDestinoInterfazRepository
    {
        Task<Result<DestinoInterfaz_DTO>> InsertDestinoInterfaz(DestinoInterfaz_DTO item);
        Task<Result<DestinoInterfaz_DTO>> InsertDestinoInterfazComplemento(DestinoInterfaz_DTO item);
    }
}
