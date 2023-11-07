using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.Core.Models
{
    public class TomaPedidos_DTO
    {
        public string Pedido { get; set; }
        public string Linea { get; set; }
        public string FechaCompromiso { get; set; }
        public string Destino { get; set; }
        public string Producto { get; set; }
        public string Demanda { get; set; }
        public string Unidad { get; set; }
        public string Origen1 { get; set; }
        public string Origen2 { get; set; }
        public string Procesado { get; set; }
        public string FechaProcesado { get; set; }
        public string TipoPedido { get; set; }
    }
}
