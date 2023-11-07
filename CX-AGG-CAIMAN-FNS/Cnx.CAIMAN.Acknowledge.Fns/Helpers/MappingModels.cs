using Cnx.CAIMAN.Acknowledge.Fns.Core.Models;
using Cnx.CAIMAN.Acknowledge.Fns.JSONModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Acknowledge.Fns.Helpers
{
    public class MappingModels
    {
        public static Resultado_DTO MappingResultadotoRoot(Root item)
        {
            Resultado_DTO outputItem = new Resultado_DTO();

            outputItem.iEstatusAzure = (int)item?.MT_ACKData?.AcknowledgmentMsg?.StatusId;
            outputItem.IdPlan = int.Parse(item?.MT_ACKData?.AcknowledgmentMsg?.MessageIDref);
            outputItem.vc20MensajeAzure = item?.MT_ACKData?.AcknowledgmentMsg?.StatusDetail;
            outputItem.TipoPlan = item?.MT_ACKData?.AcknowledgmentMsg?.ID;

            return outputItem;
        }
    }
}
