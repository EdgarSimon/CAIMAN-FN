using Cnx.CAIMAN.Acknowledge.Fns.Core.Models;
using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Acknowledge.Fns.Core.Repositories
{
    public interface IResultadoRepository
    {
        Task<Result<int>> UpdateResultado(Resultado_DTO item);
    }
}
