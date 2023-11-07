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
    public class OrigenInterfazRepository : IOrigenInterfazRepository
    {
        private ISqlDapperHelper _sqlDapperHelpr;
        public OrigenInterfazRepository(ISqlDapperHelper sqlDapperHelpr)
        {
            _sqlDapperHelpr = sqlDapperHelpr;
        }
        public async Task<Result<OrigenInterfaz_DTO>> InsertOrigenInterfaz(OrigenInterfaz_DTO item)
        {
            try
            {
                await _sqlDapperHelpr.CUDStoredProc("[dbo].[FnOrigen_InterfazInsert]",
                        new
                        {
                            vcSAP = item.vcSAP,
                            vc50Nombre = item.vc50Nombre,
                            vcBorrar = item.vcBorrar,
                            bProcesado = item.bProcesado
                        });
                return Result<OrigenInterfaz_DTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return Result<OrigenInterfaz_DTO>.Error(item, ex.Message);
            }
        }
    }
}
