using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.VendorCustomer.Fns.JSONModels
{
    public class Acknowledge
    {
        public Acknowledge()
        {
            AcknowledgeMsg = new AcknowledgeMsg();
        }
        public string SystemSource { get; set; }
        public string COMMANDDB { get; set; }
        public string QUEUEID { get; set; }
        public string TargetSystem { get; set; }
        public AcknowledgeMsg AcknowledgeMsg { get; set; }
    }

    public class AcknowledgeMsg
    {
        public string InterfaceID { get; set; }
        public string MessageID { get; set; }
        public string ObjectKey { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime SourceSystemDateTime { get; set; }
        public DateTime PIDateTimeReceived { get; set; }
        public DateTime AZUREDateTimeReceived { get; set; }
        public string AZUREDateTimeProcessed { get; set; }
        public string AZUREDateTimeAckSent { get; set; }
    }
}
