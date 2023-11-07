using System;
using Microsoft.Azure.Amqp.Framing;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Cnx.CAIMAN.VendorCustomer.Fns.Core.Repositories;
using NEO.Utilities.Helpers;
using Cnx.CAIMAN.VendorCustomer.Fns.JSONModels;
using Cnx.CAIMAN.VendorCustomer.Fns.Core.Models;
using Cnx.CAIMAN.VendorCustomer.Fns.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using NEO.Utilities.Helpers.HelpersRepositories;

namespace Cnx.CAIMAN.VendorCustomer.Fns
{
    public class FnVendorSB : FnBase
    {
        private IOrigenInterfazRepository _origenInterfazRepository;
        public FnVendorSB(IOrigenInterfazRepository origenInterfazRepository, ILogInterfaceHelper logDBHelper,ISendToSapHelper sendToSapHelper)
        {
            _origenInterfazRepository = origenInterfazRepository;
            _logDBHelper = logDBHelper;
            _sentToSapHelper = sendToSapHelper;
        }

        [FunctionName("FnVendorSB")]
        public async Task Run([ServiceBusTrigger("%VendorTopic%", "%VendorSubscription%", Connection = "Vendor_Data_SB")] string mySbMsg, ILogger log)
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
                messageId = data?.MT_VendorsSupliers?.MessageID;
                interfaceId = data?.MT_VendorsSupliers?.InterfaceID;
                string SystemSource = data?.MT_VendorsSupliers?.SystemSource;
                string QUEUEID = data?.MT_VendorsSupliers?.QUEUEID;
                string TargetSystem = "CAIMAN";//Por definir
                string StatusDescrition = string.Empty;
                DateTime SourceSystemDateTime = (DateTime)data?.MT_VendorsSupliers?.TimeLog?.SourceSystemTime;
                DateTime PIDateTimeReceived = (DateTime)data?.MT_VendorsSupliers?.TimeLog?.SourceSystemTime;
                DateTime PIProcessingTime = (DateTime)data?.MT_VendorsSupliers?.TimeLog?.PIProcessingTime;

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

                //Se mapea el obejeto del JSON al DTO a insertar, en este caso OrigenInterfaz
                log.LogInformation("Se mapea el objeto del JSON al DTO a insertar, en este caso OrigenInterfaz");
                Result<OrigenInterfaz_DTO> Result;
                OrigenInterfaz_DTO FinalItem;
                Result<HttpResponseMessage> resultadoACK;
                List<Acknowledge> resultadosError = new List<Acknowledge>();
                foreach (var item in data.MT_VendorsSupliers.VendorData)
                {
                    FinalItem = MappingModels.MappingRootToOrigenInterfaz(item);
                    //Se insertan los datos
                    log.LogInformation("Se insertan los datos");
                    Result = await _origenInterfazRepository.InsertOrigenInterfaz(FinalItem);

                    _dataAck.AcknowledgeMsg.ObjectKey = item.VendorCode;
                    _dataAck.AcknowledgeMsg.StatusDescription = Result.Success ? "Ok" : "Error: " + Result.Message;
                    _dataAck.AcknowledgeMsg.Status = Result.Success ? (int)AckStatusEnum.SapAckCodeOk : (int)AckStatusEnum.SapAckCodeError;
                    resultadoACK = await _sentToSapHelper.SentToSAPObjectAsJson(_dataAck, "ackAPIKey", "ackAPIURL", log);

                    if (!resultadoACK.Success)
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
