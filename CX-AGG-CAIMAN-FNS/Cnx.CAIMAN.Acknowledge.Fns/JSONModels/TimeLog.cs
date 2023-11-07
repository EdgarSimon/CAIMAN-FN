using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Acknowledge.Fns.JSONModels
{
    public class TimeLog
    {
        public string SourceSystemTime { get; set; }
        public string PITimeReceived { get; set; }
        public string PIProcessingTime { get; set; }
    }
}
