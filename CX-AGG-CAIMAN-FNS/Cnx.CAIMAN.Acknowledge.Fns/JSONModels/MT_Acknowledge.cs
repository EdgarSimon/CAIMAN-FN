using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Acknowledge.Fns.JSONModels
{
    public class MT_Acknowledge
    {
        public string MessageIDref { get; set; }
        public string ID { get; set; }
        public int StatusId { get; set; }
        public string StatusDetail { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
