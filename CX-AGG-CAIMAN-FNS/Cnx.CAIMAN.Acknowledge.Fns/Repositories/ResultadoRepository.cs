using Cnx.CAIMAN.Acknowledge.Fns.Core.Models;
using Cnx.CAIMAN.Acknowledge.Fns.Core.Repositories;
using Microsoft.Extensions.Configuration;
using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Acknowledge.Fns.Repositories
{
    public class ResultadoRepository : IResultadoRepository
    {
        private ISqlDapperHelper _sqlDapperHelper;
        public ResultadoRepository(ISqlDapperHelper sqlDapperHelper)
        {
            _sqlDapperHelper = sqlDapperHelper;
        }

        public async Task<Result<int>> UpdateResultado(Resultado_DTO item)
        {
            try
            {
                int Response = await _sqlDapperHelper.CUDStoredProc("[dbo].[Evo_ActualizaStatusPlan]",
                        new
                        {
                            TipoPlan = item.TipoPlan,
                            IdPlan = item.IdPlan,
                            iEstatusAzure = item.iEstatusAzure,
                            vc20MensajeAzure = item.vc20MensajeAzure
                        });
                return Result<int>.Ok(Response);
            }
            catch (Exception ex)
            {
                return Result<int>.Error(-1, ex.Message);
            }
        }
    }
}
