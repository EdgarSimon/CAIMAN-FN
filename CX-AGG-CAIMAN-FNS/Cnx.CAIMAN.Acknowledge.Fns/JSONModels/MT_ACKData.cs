using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Acknowledge.Fns.JSONModels
{
    public class MT_ACKData
    {
        public string SystemSource { get; set; }
        public string InterfaceID { get; set; }
        public string MessageID { get; set; }
        public string QUEUEID { get; set; }
        public TimeLog TimeLog { get; set; }
        public MT_Acknowledge AcknowledgmentMsg { get; set; }
    }
}
