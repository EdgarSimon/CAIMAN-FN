using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.JSONModels
{
    public class TimeLog
    {
        public DateTime SourceSystemTime { get; set; }
        public DateTime PITimeReceived { get; set; }
        public DateTime serviceBusArrivingTime { get; set; }
        public DateTime PIProcessingTime { get; set; }
        //Campos para function de archivo
        public DateTime APIArrivingTime { get; set; }
        ///////////////////////////////////////////////////////////////
        public string logicAppProcessingTime { get; set; }
    }
}
