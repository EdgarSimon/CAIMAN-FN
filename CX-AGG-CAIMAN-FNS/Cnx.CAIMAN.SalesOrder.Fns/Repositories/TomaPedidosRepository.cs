using Cnx.CAIMAN.SalesOrder.Fns.Core.Models;
using Cnx.CAIMAN.SalesOrder.Fns.Core.Repositories;
using Cnx.CAIMAN.SalesOrder.Fns.JSONModels;
using Microsoft.Extensions.Configuration;
using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.Repositories
{
    public class TomaPedidosRepository : ITomaPedidosRepository
    {
        private ISqlDapperHelper _sqlDapperHelpr;
        public TomaPedidosRepository(ISqlDapperHelper sqlDapperHelpr)
        {
            _sqlDapperHelpr = sqlDapperHelpr;
        }

        public async Task<Result<TomaPedidos_DTO>> InsertTomaPedidos(TomaPedidos_DTO item)
        {
            try
            {
                await _sqlDapperHelpr.CUDStoredProc("[dbo].[FnTomaPedidosInsert]",
                        new
                        {
                            Pedido = item.Pedido,
                            Linea = item.Linea,
                            FechaCompromiso = item.FechaCompromiso,
                            Destino = item.Destino,
                            Producto = item.Producto ,
                            Demanda = item.Demanda,
                            Unidad = item.Unidad,
                            Origen1 = item.Origen1,
                            Origen2 = item.Origen2,
                            Procesado = 0,
                            FechaProcesado = item.FechaProcesado,
                            TipoPedido = item.TipoPedido
                        });
                return Result<TomaPedidos_DTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return Result<TomaPedidos_DTO>.Error(item, ex.Message);
            }
        }
    }
}
