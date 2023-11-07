using System;
using Microsoft.Azure.Amqp.Framing;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Cnx.CAIMAN.SalesOrder.Fns.Core.Repositories;
using NEO.Utilities.Helpers;
using Cnx.CAIMAN.SalesOrder.Fns.JSONModels;
using Cnx.CAIMAN.SalesOrder.Fns.Core.Models;
using Cnx.CAIMAN.SalesOrder.Fns.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using NEO.Utilities.Helpers.HelpersRepositories;

namespace Cnx.CAIMAN.SalesOrder.Fns
{
    public class FnSalesOrderSB : FnBase
    {
        private ITomaPedidosRepository _tomaPedidosRepository;
        private ISendToSapHelper _sentToSapHelper;
        public FnSalesOrderSB(ITomaPedidosRepository tomaPedidosRepository, ILogInterfaceHelper logDBHelper, ISendToSapHelper sendToSapHelper)
        {
            _tomaPedidosRepository = tomaPedidosRepository;
            _logDBHelper = logDBHelper;
            _sentToSapHelper = sendToSapHelper;
        }

        [FunctionName("FnSalesOrderSB")]
        public async Task Run([ServiceBusTrigger("%SalesOrderTopic%", "%SalesOrderSubscription%", Connection = "SalesOrder_Data_SB", IsSessionsEnabled = true)] string mySbMsg, ILogger log)
        {
            requestBody = mySbMsg;
            messageId = string.Empty;
            interfaceId = string.Empty;

            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");
                //Se deserealiza el mensaje recibido de SAP
                log.LogInformation("Se deserealiza el mensaje recibido de SAP");
                log.LogInformation("RequestBody: " + requestBody);
                Root data = JsonConvert.DeserializeObject<Root>(requestBody);
                //ACKNOWLEDGE
                messageId = data?.MT_SalesDocument?.MessageID;
                interfaceId = data?.MT_SalesDocument?.InterfaceID;
                string SystemSource = data?.MT_SalesDocument?.SystemSource;
                string QUEUEID = data?.MT_SalesDocument?.QUEUEID;
                string TargetSystem = "CAIMAN";//Por definir
                string StatusDescrition = string.Empty;
                DateTime SourceSystemDateTime = (DateTime)data?.MT_SalesDocument?.TimeLog?.SourceSystemTime;
                DateTime PIDateTimeReceived = (DateTime)data?.MT_SalesDocument?.TimeLog?.SourceSystemTime;
                DateTime PIProcessingTime = (DateTime)data?.MT_SalesDocument?.TimeLog?.PIProcessingTime;

                // Obtener variabels de ACK data
                var _dataAck = new Acknowledge();
                _dataAck.AcknowledgeMsg = new AcknowledgeMsg();
                _dataAck.SystemSource = SystemSource;
                _dataAck.COMMANDDB = "";
                _dataAck.QUEUEID = QUEUEID;
                _dataAck.TargetSystem = TargetSystem;
                _dataAck.AcknowledgeMsg.InterfaceID = interfaceId;
                _dataAck.AcknowledgeMsg.MessageID = messageId;

                _dataAck.AcknowledgeMsg.SourceSystemDateTime = SourceSystemDateTime;
                _dataAck.AcknowledgeMsg.PIDateTimeReceived = PIDateTimeReceived;
                _dataAck.AcknowledgeMsg.AZUREDateTimeReceived = PIProcessingTime;
                _dataAck.AcknowledgeMsg.AZUREDateTimeProcessed = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                _dataAck.AcknowledgeMsg.AZUREDateTimeAckSent = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

                //Se mapea el obejeto del JSON al DTO a insertar, en este caso TomaPedido
                log.LogInformation("Se mapea el objeto del JSON al DTO a insertar, en este caso TomaPedido");
                TomaPedidos_DTO FinalItem;
                Result<TomaPedidos_DTO> Result;
                Result<HttpResponseMessage> resultadoACK;
                List<Acknowledge> resultadosError = new List<Acknowledge>();
                foreach (ITEM item in data?.MT_SalesDocument?.Header?.ITEMS)
                {
                    FinalItem = MappingModels.MappingRootToTomaPedidos(item, data?.MT_SalesDocument?.Header?.CustomerId);
                    //Se insertan los datos
                    log.LogInformation("Se insertan los datos");
                    Result = await _tomaPedidosRepository.InsertTomaPedidos(FinalItem);
                    //Se manda el acknowledge
                    _dataAck.AcknowledgeMsg.ObjectKey = item.OrderCode;
                    _dataAck.AcknowledgeMsg.StatusDescription = Result.Success ? "Ok" : "Error: " + Result.Message;
                    _dataAck.AcknowledgeMsg.Status = Result.Success ? (int)AckStatusEnum.SapAckCodeOk : (int)AckStatusEnum.SapAckCodeError;
                    resultadoACK = await _sentToSapHelper.SentToSAPObjectAsJson(_dataAck, "ackAPIKey", "ackAPIURL", log);

                    if (!resultadoACK.Success || _dataAck.AcknowledgeMsg.Status == (int)AckStatusEnum.SapAckCodeError)
                        resultadosError.Add(_dataAck);
                }
                
                //Se realiza el log en BD
                log.LogInformation("Se realiza el log en BD");
                DBlog = new LogInterface_DTO()
                {
                    DateLog = DateTime.Now,
                    InputMessageText = requestBody,
                    OutputMessageText = JsonConvert.SerializeObject(resultadosError),
                    InterfaceCode = interfaceId,
                    MessageId = messageId,
                    ResultText = resultadosError.Any() ? "Proceso concluido correctamente con algunos errores." : "Proceso concluido correctamente"
                };
                await _logDBHelper.InsertLogInterface(DBlog);
                //Se retorna el result
                log.LogInformation("Se retorna el result");

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

            }
        }
    }
}
