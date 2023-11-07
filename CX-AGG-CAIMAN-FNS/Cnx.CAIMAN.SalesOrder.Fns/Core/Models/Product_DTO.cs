using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.Core.Models
{
    public class Product_DTO
    {
        public string vcSAP { get; set; }
        public string vcNombre900 { get; set; }
        public string IdProd55 { get; set; }
        public string vcNombre55 { get; set; }
        public decimal nPesoVolumetrico { get; set; }
        public DateTime dtCreacion { get; set; }
        public DateTime dtActualizacion { get; set; }
        public string vc20UsuarioCreacion { get; set; }
        public string vc20UsuarioActualizacion { get; set; }
        public string vcBorrar { get; set; }
        public bool Procesado { get; set; }
        public bool bProcesado { get; set; }

    }
}
