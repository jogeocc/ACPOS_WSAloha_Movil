using EncryptDataJson.Modelos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.Endpoints;
using WindowsServiceAlohaMobile.Models.Aloha.SAP;

namespace WindowsServiceAlohaMobile.Rest
{
    /// <summary>
    /// 27-03-2023
    /// Clase contenedora de las peticiones hacia los servicios de SAP.
    /// 
    /// </summary>
    public class RestSAP
    {
        RestClient ClientSap;
        /// <summary>
        /// CONSTRUCTOR DE SAP
        /// </summary>
        public RestSAP()
        {
            ClientSap = new RestClient(Service1.appConfig.DIRECCION_SAP);
        }

        public void SendXmlSAP(string xml)
        {
            try
            {
                RestRequest restRequest = new RestRequest(SapEndpoints.TicketSAP, Method.Post);
                TicketSapModel ticketSapModel = new TicketSapModel()
                {
                    Xml = xml,
                };
                DataEncrypt EncriptInfo = Service1.EncryptDataJson.EncryptDataJSON(ticketSapModel);
                Service1.logger.Info($"JSON: \r\n {JsonConvert.SerializeObject(EncriptInfo)}");
                restRequest.AddJsonBody(EncriptInfo);
                RestResponse<DataEncrypt> response = ClientSap.Execute<DataEncrypt>(restRequest);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Service1.logger.Info($"INFORMACION ENVIADA CON EXITO A SAP");
                    Service1.logger.Info($"{response.Content}");
                }
                else
                {
                    Service1.logger.Info($"ERROR AL MANDAR XML DE TICKET");
                    Service1.logger.Info($"{response.Content}");
                }
            }
            catch (Exception ex)
            {
                Service1.logger.Error($"ERROR AL ENVIAR INFORMACION TICKET HACIA SAP", ex);
            }
        }

        public void SendPagosSocioSap(List<DetallePago> detallePagos)
        {
            try
            {
                RestRequest restRequest = new RestRequest(SapEndpoints.PagosSocioSAP, Method.Post);
                TicketSapModel ticketSapModel = new TicketSapModel()
                {
                    socios = detallePagos
                };
                DataEncrypt EncriptInfo = Service1.EncryptDataJson.EncryptDataJSON(ticketSapModel);
                Service1.logger.Info($"JSON: \r\n {JsonConvert.SerializeObject(EncriptInfo)}");
                restRequest.AddJsonBody(EncriptInfo);
                RestResponse<DataEncrypt> response = ClientSap.Execute<DataEncrypt>(restRequest);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Service1.logger.Info($"INFORMACION ENVIADA CON EXITO A SAP");
                    Service1.logger.Info($"{response.Content}");
                }
                else
                {
                    Service1.logger.Info($"ERROR AL MANDAR PAGOS DE TICKET");
                    Service1.logger.Info($"{response.Content}");
                }
            }
            catch (Exception ex)
            {
                Service1.logger.Error($"ERROR AL ENVIAR INFORMACION DE PAGOS HACIA SAP", ex);
            }
        }
    }
}
