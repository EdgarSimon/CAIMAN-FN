using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Product.Fns.JSONModels
{
    public class TimeLog
    {
        public string SourceSystemTime { get; set; }
        public string PITimeReceived { get; set; }
        public string serviceBusArrivingTime { get; set; }
        public string PIProcessingTime { get; set; }
        //Campos para function de archivo
        public string APIArrivingTime { get; set; }
        ///////////////////////////////////////////////////////////////
        public string logicAppProcessingTime { get; set; }
    }
}
