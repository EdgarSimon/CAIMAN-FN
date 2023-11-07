using Cnx.CAIMAN.SalesOrder.Fns.Core.Models;
using Cnx.CAIMAN.SalesOrder.Fns.JSONModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.Helpers
{
    public class MappingModels
    {
        public static TomaPedidos_DTO MappingRootToTomaPedidos(ITEM item, string customerId)
        {
            TomaPedidos_DTO outputItem = new TomaPedidos_DTO();
            outputItem.Pedido = item.OrderCode;
            outputItem.FechaProcesado = item.DeliveryDateTime.ToString();
            outputItem.FechaCompromiso = item.DeliveryDateTime.ToString();
            outputItem.Producto = item.ProductCode;
            outputItem.Demanda = item.ProductQuantity;
            outputItem.Destino = customerId;
            outputItem.Unidad = item.UnitId;
            outputItem.Origen1 = item.PlantId;
            outputItem.TipoPedido = item.ShippingType;
            outputItem.Linea = item.ItemSeqNum.ToString();

            return outputItem;
        }
    }
}
