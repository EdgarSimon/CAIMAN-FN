using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.Core.Models
{
    public class DestinoInterfaz_DTO
    {
        public string vcSAP { get; set; }
        public string vc50Nombre { get; set; }
        public string vcZonaEnt { get; set; }
        public bool bEsCliente { get; set; }
        public int IdZonaCaiman { get; set; }
        public string vcBorrar { get; set; }
        public string Cedis { get; set; }
        public bool bProcesado { get; set; }
        public int iSector { get; set; }
        public int iAux { get; set; }

    }
}
