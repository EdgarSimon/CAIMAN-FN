using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Acknowledge.Fns.Core.Models
{
    public class Resultado_DTO
    {
        public string vc20MensajeAzure { get; set; }
        public int IdPlan { get; set; }
        public int iEstatusAzure { get; set; }
        public string TipoPlan { get; set; }
    }
}
