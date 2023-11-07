using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Acknowledge.Fns
{
    public class FnBase
    {
        internal LogInterface_DTO DBlog;
        internal string requestBody;
        internal string messageId;
        internal string interfaceId;
    }
}
