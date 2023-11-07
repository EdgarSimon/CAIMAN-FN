using NEO.Utilities.Helpers;
using NEO.Utilities.Helpers.HelpersRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns
{
    public class FnBase
    {
        internal ILogInterfaceHelper _logDBHelper;
        internal ISendToSapHelper _sentToSapHelper;

        internal LogInterface_DTO DBlog;
        internal string requestBody;
        internal string messageId;
        internal string interfaceId;
    }
}
