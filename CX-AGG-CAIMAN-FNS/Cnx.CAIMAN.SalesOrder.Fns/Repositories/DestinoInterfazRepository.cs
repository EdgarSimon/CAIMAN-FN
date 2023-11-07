using Cnx.CAIMAN.SalesOrder.Fns.Core.Models;
using Cnx.CAIMAN.SalesOrder.Fns.Core.Repositories;
using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.SalesOrder.Fns.Repositories
{
    public class DestinoInterfazRepository : IDestinoInterfazRepository
    {
        private ISqlDapperHelper _sqlDapperHelpr;
        public DestinoInterfazRepository(ISqlDapperHelper sqlDapperHelpr)
        {
            _sqlDapperHelpr = sqlDapperHelpr;
        }
        public async Task<Result<DestinoInterfaz_DTO>> InsertDestinoInterfaz(DestinoInterfaz_DTO item)
        {
            try
            {
                await _sqlDapperHelpr.CUDStoredProc("[dbo].[FnDestino_InterfazInsert]",
                        new
                        {
                            vcSAP = item.vcSAP,
                            vc50Nombre = item.vc50Nombre,
                            vcBorrar = item.vcBorrar,
                            bProcesado = item.bProcesado
                        });
                return Result<DestinoInterfaz_DTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return Result<DestinoInterfaz_DTO>.Error(item, ex.Message);
            }
        }
    }
}
