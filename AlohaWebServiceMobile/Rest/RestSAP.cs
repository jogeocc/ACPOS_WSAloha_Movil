using AlohaWebServiceMobile.Endpoints;
using AlohaWebServiceMobile.Models.SAP;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Rest
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
        RestSAP()
        {
            ClientSap = new RestClient(App.appConfig.DIRECCION_SAP);
        }

        public void SendXmlSAP(string xml)
        {
            try
            {
                RestRequest restRequest = new RestRequest(SapEndpoints.TicketSAP, Method.Post);
                TicketSapModel ticketSapModel = new TicketSapModel()
                {
                    XML = xml
                };
                var EncriptInfo = App.EncryptDataJson.EncryptDataJSON(ticketSapModel);

                restRequest.AddJsonBody(EncriptInfo);
                RestResponse response = ClientSap.Execute(restRequest);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    App.logger.Info($"INFORMACION ENVIADA CON EXITO A SAP");
                }
                else
                {
                    App.logger.Info($"ERROR AL MANDAR XML DE TICKET");
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"ERROR AL ENVIAR INFORMACION TICKET HACIA SAP", ex);
            }
        }
    }
}
