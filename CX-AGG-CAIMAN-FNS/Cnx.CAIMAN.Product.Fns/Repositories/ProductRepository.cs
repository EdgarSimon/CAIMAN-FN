using Cnx.CAIMAN.Product.Fns.Core.Models;
using Cnx.CAIMAN.Product.Fns.Core.Repositories;
using Cnx.CAIMAN.Product.Fns.JSONModels;
using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Product.Fns.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private ISqlDapperHelper _sqlDapperHelpr;
        public ProductRepository(ISqlDapperHelper sqlDapperHelpr)
        {
            _sqlDapperHelpr = sqlDapperHelpr;
        }
        public async Task<Result<Product_DTO>> InsertProduct(Product_DTO item)
        {
            try
            {
                await _sqlDapperHelpr.CUDStoredProc("[dbo].[FnProducto_InterfazInsert]",
                        new
                        {
                            vcSAP = item.vcSAP,
                            vcNombre900 = item.vcNombre900,
                            IdProd55 = item.IdProd55,
                            nPesoVolumetrico = item.nPesoVolumetrico,
                            dtCreacion = item.dtCreacion,
                            dtActualizacion = item.dtActualizacion,
                            vc20UsuarioCreacion = item.vc20UsuarioCreacion,
                            vc20UsuarioActualizacion = item.vc20UsuarioActualizacion,
                            vcBorrar = item.vcBorrar,
                            Procesado = item.Procesado,
                            bProcesado = item.bProcesado
                        });
                return Result<Product_DTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return Result<Product_DTO>.Error(item, ex.Message);
            }
        }
    }
}
