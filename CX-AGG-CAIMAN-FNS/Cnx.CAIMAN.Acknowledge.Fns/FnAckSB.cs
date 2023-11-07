using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NEO.Utilities.Helpers;
using Cnx.CAIMAN.Acknowledge.Fns.Core.Repositories;
using Cnx.CAIMAN.Acknowledge.Fns.JSONModels;
using Cnx.CAIMAN.Acknowledge.Fns.Core.Models;
using Cnx.CAIMAN.Acknowledge.Fns.Helpers;
using System.Threading.Tasks;

namespace Cnx.CAIMAN.Acknowledge.Fns
{
    public class FnAckSB : FnBase
    {
        private readonly ILogInterfaceHelper _logDBHelper;
        private readonly IResultadoRepository _resultadoRepository;

        public FnAckSB(IResultadoRepository resultadoRepository, ILogInterfaceHelper logDBHelper)
        {
            _logDBHelper = logDBHelper;
            _resultadoRepository = resultadoRepository;
        }

        [FunctionName("FnAckSB")]
        public async Task Run([ServiceBusTrigger("%AcknowledgeTopic%", "%AcknowledgeSuscription%", Connection = "Acknowledge_Data_SB")]string mySbMsg,ILogger log)
        {
            requestBody = mySbMsg;
            messageId = string.Empty;
            interfaceId = string.Empty;
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                //Se deserealiza el mensaje recibido de SAP
                log.LogInformation("Se deserealiza el mensaje recibido de SAP");
                log.LogInformation("RequestBody: " + requestBody);
                Root data = JsonConvert.DeserializeObject<Root>(requestBody);
                messageId = data?.MT_ACKData?.MessageID;
                interfaceId = data?.MT_ACKData?.InterfaceID;
                //Se mapea el obejeto del JSON al DTO a insertar, en este caso ACKTbl
                ///NOTA
                //Preguntar que se haría si el  status Id = 1 , es decir, es erroneo en SAP, se actualiza algo en la tabla?
                //if (data?.MT_ACKData?.AcknowledgmentMsg?.StatusId == 1)
                //{
                //    log.LogInformation("Mensaje con statusId = 1, Status de error enviado por SAP");
                //    Result<MT_ACKData> resStatusError = new Result<MT_ACKData>() { Success = false, Data = data?.MT_ACKData, Message = data?.MT_ACKData?.AcknowledgmentMsg?.StatusDetail };
                //    DBlog = new LogInterface_DTO()
                //    {
                //        DateLog = DateTime.Now,
                //        InputMessageText = requestBody,
                //        OutputMessageText = JsonConvert.SerializeObject(resStatusError),
                //        InterfaceCode = interfaceId,
                //        MessageId = messageId,
                //        ResultText = "StatusId = 1, Status de error enviado desde SAP"
                //    };
                //    await _logDBHelper.InsertLogInterface(DBlog);
                //    //return new OkObjectResult(resStatusError);
                //}
                //else
                //{

                    log.LogInformation("Se mapea el objeto del JSON al DTO a insertar, en este caso ACKTbl");
                    Resultado_DTO item = MappingModels.MappingResultadotoRoot(data);
                    //Se insertan los datos
                    log.LogInformation("Se insertan los datos");
                    Result<int> Result = await _resultadoRepository.UpdateResultado(item);
                    log.LogInformation("Resultado : " + JsonConvert.SerializeObject(Result)); 
                    //Se realiza el log en BD
                    log.LogInformation("Se realiza el log en BD");
                    DBlog = new LogInterface_DTO()
                    {
                        DateLog = DateTime.Now,
                        InputMessageText = requestBody,
                        OutputMessageText = JsonConvert.SerializeObject(Result),
                        InterfaceCode = interfaceId,
                        MessageId = messageId,
                        ResultText = JsonConvert.SerializeObject(Result)
                    };
                    await _logDBHelper.InsertLogInterface(DBlog);
                    //Se retorna el result
                    log.LogInformation("Se retorna el result");
                    //return new OkObjectResult(Result);
                //}
            }
            catch (Exception ex)
            {
                log.LogInformation("Error: " + JsonConvert.SerializeObject(ex));
                DBlog = new LogInterface_DTO()
                {
                    DateLog = DateTime.Now,
                    InputMessageText = requestBody,
                    OutputMessageText = JsonConvert.SerializeObject(ex),
                    InterfaceCode = interfaceId,
                    MessageId = messageId,
                    ResultText = ex.Message
                };
                await _logDBHelper.InsertLogInterface(DBlog);
                //return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
