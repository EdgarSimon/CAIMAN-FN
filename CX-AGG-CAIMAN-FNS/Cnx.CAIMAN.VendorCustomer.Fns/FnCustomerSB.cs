using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Cnx.CAIMAN.VendorCustomer.Fns.Core.Models;
using Cnx.CAIMAN.VendorCustomer.Fns.Core.Repositories;
using Cnx.CAIMAN.VendorCustomer.Fns.Helpers;
using Cnx.CAIMAN.VendorCustomer.Fns.JSONModels;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using NEO.Utilities.Helpers;
using NEO.Utilities.Helpers.HelpersRepositories;
using Newtonsoft.Json;

namespace Cnx.CAIMAN.VendorCustomer.Fns
{
    public class FnCustomerSB : FnBase
    {
        private readonly IDestinoInterfazRepository _destinoInterfazRepository;
        private readonly IMapper _mapper;
        private static List<string> CustomerQueue;
        private static List<string> JobSiteQueue;

        public FnCustomerSB(IDestinoInterfazRepository destinoInterfazRepository, ILogInterfaceHelper logDBHelper, ISendToSapHelper sendToSapHelper,IMapper mapper)
        {
            _destinoInterfazRepository = destinoInterfazRepository;
            _logDBHelper = logDBHelper;
            _sentToSapHelper = sendToSapHelper;
            CustomerQueue = new List<string>();
            JobSiteQueue = new List<string>();
            _mapper = mapper;
        }

        [FunctionName("FnCustomerSB")]
        public async Task Run([ServiceBusTrigger("%CustomerTopic%", "%CustomerSubscription%", Connection = "Customer_Data_SB", IsSessionsEnabled = true)] string mySbMsg, ILogger log)
        {
            requestBody = mySbMsg;
            messageId = string.Empty;
            interfaceId = string.Empty;

            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            log.LogInformation("C# HTTP trigger function processed a request.");
            //Se deserealiza el mensaje recibido de SAP
            log.LogInformation("Se deserealiza el mensaje recibido de SAP");
            log.LogInformation("RequestBody: " + requestBody);
            Root data = new Root();
            RootAddressList data1 = new RootAddressList();
            RootSingleAddress data2 = new RootSingleAddress();
            bool isAddressList = true;
            try
            {
                data1 = JsonConvert.DeserializeObject<RootAddressList>(requestBody);
                data = _mapper.Map<Root>(data1);
            }
            catch
            {
                data2 = JsonConvert.DeserializeObject<RootSingleAddress>(requestBody);
                data = _mapper.Map<Root>(data2);
                isAddressList = false;
            }

            try
            {
                
                //ACKNOWLEDGE
                messageId = string.Empty;
                interfaceId = string.Empty;
                string SystemSource = string.Empty;
                string QUEUEID = string.Empty;
                string TargetSystem = "CAIMAN";//Por definir
                string StatusDescrition = string.Empty;
                DateTime SourceSystemDateTime = DateTime.Now;
                DateTime PIDateTimeReceived = DateTime.Now;
                DateTime PIProcessingTime = DateTime.Now;
                Acknowledge _dataAck = null;

                // si es un mensje de jobsite
                if (data != null && data.MT_ServiceBusJobSite != null && data.MT_ServiceBusJobSite.InterfaceID == "JST")
                {
                    
                    messageId = data?.MT_ServiceBusJobSite?.MessageID;
                    interfaceId = data?.MT_ServiceBusJobSite?.InterfaceID;
                    SystemSource = data?.MT_ServiceBusJobSite?.SystemSource;
                    QUEUEID = data?.MT_ServiceBusJobSite?.QUEUEID;
                    TargetSystem = "CAIMAN";//Por definir
                    StatusDescrition = string.Empty;

                    if (!string.IsNullOrEmpty(data?.MT_ServiceBusJobSite?.TimeLog?.SourceSystemTime))
                        SourceSystemDateTime = DateTime.Parse(data?.MT_ServiceBusJobSite?.TimeLog?.SourceSystemTime);
                    if (!string.IsNullOrEmpty(data?.MT_ServiceBusJobSite?.TimeLog?.SourceSystemTime))
                        PIDateTimeReceived = DateTime.Parse(data?.MT_ServiceBusJobSite?.TimeLog?.SourceSystemTime);
                    if (!string.IsNullOrEmpty(data?.MT_ServiceBusJobSite?.TimeLog?.PIProcessingTime))
                        PIProcessingTime = DateTime.Parse(data?.MT_ServiceBusJobSite?.TimeLog?.PIProcessingTime);

                    // Obtener variabels de ACK data
                    _dataAck = new Acknowledge();
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

                    //Se añade queue para evitar errores porque SAP envia 2 mensajes con intervalo de tiempo en milisegundos
                    lock (JobSiteQueue)
                    {
                        if (!JobSiteQueue.Contains(data.MT_ServiceBusJobSite?.QUEUEID))
                        {
                            JobSiteQueue.Add(data.MT_ServiceBusJobSite?.QUEUEID);
                            //CODIGO A BLOQUEAR

                            //Se mapea el obejeto del JSON al DTO a insertar, en este caso Product
                            log.LogInformation("Se mapea el objeto del JSON al DTO a insertar, en este caso Product");
                            Result<DestinoInterfaz_DTO> Result;
                            DestinoInterfaz_DTO FinalItem;
                            Result<HttpResponseMessage> resultadoACK;
                            List<Acknowledge> resultadosError = new List<Acknowledge>();
                            foreach (var item in data.MT_ServiceBusJobSite.JobSitesList)
                            {
                                // se incluye filtro para no procesar la info a la BD de CAIMAN cuando no sean de division 4
                                if (!item.JobSiteSalesAreaData.Any(x => x.SalesAreaId_Division == "4" || x.SalesAreaId_Division == "04"))
                                {
                                    log.LogInformation("No hay una Sales Area con division 4 para el Jobsite {0}", item.JobSiteCode);
                                    _dataAck.AcknowledgeMsg.ObjectKey = item.JobSiteCode;
                                    _dataAck.AcknowledgeMsg.StatusDescription = "Info: " + string.Format("No hay una Sales Area con division 4 para el Jobsite {0}.", item.JobSiteCode);
                                    _dataAck.AcknowledgeMsg.Status = (int)AckStatusEnum.SapAckCodeOk;
                                }
                                else
                                {
                                    item.AddressList = new List<Address>();
                                    if (isAddressList)
                                        item.AddressList.AddRange(data1.MT_ServiceBusJobSite.JobSites.FirstOrDefault(x => x.JobSiteCode == item.JobSiteCode).Address);
                                    else
                                        item.AddressList.Add(data2.MT_ServiceBusJobSite.JobSites.FirstOrDefault(x => x.JobSiteCode == item.JobSiteCode).Address);

                                    FinalItem = MappingModels.MappingRootToDestinoInterfaz(item);
                                    //Se insertan los datos
                                    log.LogInformation("Se insertan los datos");
                                    Result = _destinoInterfazRepository.InsertDestinoInterfazComplemento(FinalItem).GetAwaiter().GetResult();

                                    _dataAck.AcknowledgeMsg.ObjectKey = item.JobSiteCode;
                                    _dataAck.AcknowledgeMsg.StatusDescription = Result.Success ? "Ok" : "Error: " + Result.Message;
                                    _dataAck.AcknowledgeMsg.Status = Result.Success ? (int)AckStatusEnum.SapAckCodeOk : (int)AckStatusEnum.SapAckCodeError;
                                }

                                resultadoACK = _sentToSapHelper.SentToSAPObjectAsJson(_dataAck, "ackAPIKey", "ackAPIURL", log).GetAwaiter().GetResult();

                                if (!resultadoACK.Success)
                                    resultadosError.Add(_dataAck);
                            }

                            //Se realiza el log en BD
                            log.LogInformation("Se realiza el log en BD");
                            DBlog = new LogInterface_DTO()
                            {
                                DateLog = DateTime.Now,
                                InputMessageText = requestBody,
                                OutputMessageText = resultadosError.Any() ? JsonConvert.SerializeObject(resultadosError) : JsonConvert.SerializeObject(_dataAck),
                                InterfaceCode = interfaceId,
                                MessageId = messageId,
                                ResultText = resultadosError.Any() ? "Proceso concluido correctamente con algunos errores." : "Proceso concluido correctamente"
                            };
                            var Res = _logDBHelper.InsertLogInterface(DBlog).GetAwaiter().GetResult();
                            //Se retorna el result
                            log.LogInformation("Se retorna el result");
                            //
                            JobSiteQueue.Remove(data.MT_ServiceBusJobSite?.QUEUEID);
                        }
                    }


                }
                else // si es un mensje de Customer
                if (data != null && data.MT_ServiceBusCustomer != null && data.MT_ServiceBusCustomer.InterfaceID == "CTM")
                {
                    messageId = data?.MT_ServiceBusCustomer?.MessageID;
                    interfaceId = data?.MT_ServiceBusCustomer?.InterfaceID;
                    SystemSource = data?.MT_ServiceBusCustomer?.SystemSource;
                    QUEUEID = data?.MT_ServiceBusCustomer?.QUEUEID;
                    TargetSystem = "CAIMAN";//Por definir
                    StatusDescrition = string.Empty;

                    if (!string.IsNullOrEmpty(data?.MT_ServiceBusCustomer?.TimeLog?.SourceSystemTime))
                        SourceSystemDateTime = DateTime.Parse(data?.MT_ServiceBusCustomer?.TimeLog?.SourceSystemTime);
                    if (!string.IsNullOrEmpty(data?.MT_ServiceBusCustomer?.TimeLog?.SourceSystemTime))
                        PIDateTimeReceived = DateTime.Parse(data?.MT_ServiceBusCustomer?.TimeLog?.SourceSystemTime);
                    if (!string.IsNullOrEmpty(data?.MT_ServiceBusCustomer?.TimeLog?.PIProcessingTime))
                        PIProcessingTime = DateTime.Parse(data?.MT_ServiceBusCustomer?.TimeLog?.PIProcessingTime);

                    // Obtener variabels de ACK data
                    _dataAck = new Acknowledge();
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

                    //Se añade queue para evitar errores porque SAP envia 2 mensajes con intervalo de tiempo en milisegundos
                    lock (CustomerQueue)
                    {
                        if (!CustomerQueue.Contains(data.MT_ServiceBusCustomer?.QUEUEID))
                        {
                            CustomerQueue.Add(data.MT_ServiceBusCustomer?.QUEUEID);
                            //CODIGO A BLOQUEAR

                            //Se mapea el obejeto del JSON al DTO a insertar, en este caso Product
                            log.LogInformation("Se mapea el objeto del JSON al DTO a insertar, en este caso Product");
                            Result<DestinoInterfaz_DTO> Result;
                            DestinoInterfaz_DTO FinalItem;
                            Result<HttpResponseMessage> resultadoACK;
                            List<Acknowledge> resultadosError = new List<Acknowledge>();
                            foreach (var item in data.MT_ServiceBusCustomer.CustomersList)
                            {
                                // se incluye filtro para no procesar la info a la BD de CAIMAN cuando no sean de division 4
                                if (!item.CustSalesAreaData.Any(x => x.Division == "4" || x.Division == "04"))
                                {
                                    log.LogInformation("No hay una Sales Area con division 4 para el Customer {0}", item.CustomerCode);
                                    _dataAck.AcknowledgeMsg.ObjectKey = item.CustomerCode;
                                    _dataAck.AcknowledgeMsg.StatusDescription = "Info: " + string.Format("No hay una Sales Area con division 4 para el Customer {0}.", item.CustomerCode);
                                    _dataAck.AcknowledgeMsg.Status = (int)AckStatusEnum.SapAckCodeOk;
                                }
                                else
                                {

                                    FinalItem = MappingModels.MappingRootToDestinoInterfaz(item);
                                    //Se insertan los datos
                                    log.LogInformation("Se insertan los datos");
                                    Result = _destinoInterfazRepository.InsertDestinoInterfaz(FinalItem).GetAwaiter().GetResult();

                                    _dataAck.AcknowledgeMsg.ObjectKey = item.CustomerCode;
                                    _dataAck.AcknowledgeMsg.StatusDescription = Result.Success ? "Ok" : "Error: " + Result.Message;
                                    _dataAck.AcknowledgeMsg.Status = Result.Success ? (int)AckStatusEnum.SapAckCodeOk : (int)AckStatusEnum.SapAckCodeError;
                                }
                                
                                resultadoACK = _sentToSapHelper.SentToSAPObjectAsJson(_dataAck, "ackAPIKey", "ackAPIURL", log).GetAwaiter().GetResult();

                                if (!resultadoACK.Success)
                                    resultadosError.Add(_dataAck);
                            }

                            //Se realiza el log en BD
                            log.LogInformation("Se realiza el log en BD");
                            DBlog = new LogInterface_DTO()
                            {
                                DateLog = DateTime.Now,
                                InputMessageText = requestBody,
                                OutputMessageText = resultadosError.Any() ? JsonConvert.SerializeObject(resultadosError) : JsonConvert.SerializeObject(_dataAck),
                                InterfaceCode = interfaceId,
                                MessageId = messageId,
                                ResultText = resultadosError.Any() ? "Proceso concluido correctamente con algunos errores." : "Proceso concluido correctamente"
                            };
                            var Res = _logDBHelper.InsertLogInterface(DBlog).GetAwaiter().GetResult();
                            //Se retorna el result
                            log.LogInformation("Se retorna el result");

                            //
                            CustomerQueue.Remove(data.MT_ServiceBusCustomer?.QUEUEID);
                        }
                    }
                }


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
