using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Cnx.CAIMAN.Product.Fns.Core.Repositories;
using NEO.Utilities.Helpers;
using Cnx.CAIMAN.Product.Fns.JSONModels;
using Cnx.CAIMAN.Product.Fns.Core.Models;
using Cnx.CAIMAN.Product.Fns.Helpers;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using NEO.Utilities.Helpers.HelpersRepositories;

namespace Cnx.CAIMAN.Product.Fns
{
    public class FnProductSB : FnBase
    {
        private IProductRepository _productRepository;
        private ISendToSapHelper _sentToSapHelper;
        public FnProductSB(IProductRepository productRepository, ILogInterfaceHelper logDBHelper, ISendToSapHelper sendToSapHelper)
        {
            _productRepository = productRepository;
            _logDBHelper = logDBHelper;
            _sentToSapHelper = sendToSapHelper;
        }

        [FunctionName("FnProductSB")]
        public async Task Run([ServiceBusTrigger("%ProductTopic%", "%ProductSubscription%", Connection = "Product_Data_SB", IsSessionsEnabled = true)] string mySbMsg, ILogger log)
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
                messageId = data?.MT_ServiceBusProduct?.messageId;
                interfaceId = data?.MT_ServiceBusProduct?.interfaceId;
                string SystemSource = data?.MT_ServiceBusProduct?.systemSource;
                string QUEUEID = data?.MT_ServiceBusProduct?.QUEUEID;
                string TargetSystem = "CAIMAN";//Por definir
                string StatusDescrition = string.Empty;
                DateTime SourceSystemDateTime = DateTime.Now;
                DateTime PIDateTimeReceived = DateTime.Now;
                DateTime PIProcessingTime = DateTime.Now;
                if (!string.IsNullOrEmpty(data?.MT_ServiceBusProduct?.TimeLog?.SourceSystemTime))
                    SourceSystemDateTime = DateTime.Parse(data?.MT_ServiceBusProduct?.TimeLog?.SourceSystemTime);
                if (!string.IsNullOrEmpty(data?.MT_ServiceBusProduct?.TimeLog?.SourceSystemTime))
                    PIDateTimeReceived = DateTime.Parse(data?.MT_ServiceBusProduct?.TimeLog?.SourceSystemTime);
                if (!string.IsNullOrEmpty(data?.MT_ServiceBusProduct?.TimeLog?.PIProcessingTime))
                    PIProcessingTime = DateTime.Parse(data?.MT_ServiceBusProduct?.TimeLog?.PIProcessingTime);

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

                //Se mapea el obejeto del JSON al DTO a insertar, en este caso Product
                log.LogInformation("Se mapea el objeto del JSON al DTO a insertar, en este caso Product");
                Result<Product_DTO> Result;
                Product_DTO FinalItem;
                Result<HttpResponseMessage> resultadoACK;
                List<Acknowledge> resultadosError = new List<Acknowledge>();
                foreach (JSONModels.Product item in data?.MT_ServiceBusProduct?.products)
                {
                    FinalItem = MappingModels.MappingRootToProduct(item);
                    //Se insertan los datos en la BD
                    log.LogInformation("Se insertan los datos: {0}", JsonConvert.SerializeObject(FinalItem));
                    Result = await _productRepository.InsertProduct(FinalItem);
                    //Se manda el acknowledge
                    _dataAck.AcknowledgeMsg.ObjectKey = item.productCode + data?.MT_ServiceBusProduct?.fromCountryCode;
                    _dataAck.AcknowledgeMsg.StatusDescription = Result.Success ? "Ok" : "Error: " + Result.Message;
                    _dataAck.AcknowledgeMsg.Status = Result.Success ? (int)AckStatusEnum.SapAckCodeOk : (int)AckStatusEnum.SapAckCodeError;
                    resultadoACK = await _sentToSapHelper.SentToSAPObjectAsJson(_dataAck, "ackAPIKey", "ackAPIURL", log);

                    if (!resultadoACK.Success)
                        resultadosError.Add(_dataAck);

                }
                requestBody = JsonConvert.SerializeObject(data);
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
                log.LogInformation("Proceso concluido");

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
