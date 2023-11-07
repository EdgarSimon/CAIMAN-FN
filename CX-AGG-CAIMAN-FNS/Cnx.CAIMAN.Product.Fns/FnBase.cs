using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Product.Fns
{
    public class FnBase
    {
        internal ILogInterfaceHelper _logDBHelper;

        internal LogInterface_DTO DBlog;
        internal string requestBody;
        internal string messageId;
        internal string interfaceId;
    }
}
