using WindowsServiceAlohaMobile.AlohaExtractInfo;
using WindowsServiceAlohaMobile.EntityFrameWork.Models;
using WindowsServiceAlohaMobile.Models;
using WindowsServiceAlohaMobile.Models.Aloha;
using WindowsServiceAlohaMobile.Models.Aloha.Desktop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WindowsServiceAlohaMobile.Controllers
{
    [RoutePrefix("api/v1/aloha")]
    public class AlohaController : ApiController
    {
        //RECUPERACION DE CATALOGOS
        [HttpGet]
        [Route("version")]
        public HttpResponseMessage version()
        {
            object obj = new { ip = Service1.appConfig.IP, port = Service1.appConfig.PORT };
            return Request.CreateResponse(HttpStatusCode.OK, obj, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("menus")]
        public HttpResponseMessage menus()
        {

            var menus = Service1.Catalogos.ObtenerMenuMovil();
            return Request.CreateResponse(HttpStatusCode.OK, menus, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("menus/{ID_MENU}")]
        public HttpResponseMessage GETMenuSpecific(int ID_MENU)
        {
            var menus = Service1.Catalogos.ObtenerMenuMovil(ID_MENU);

            return Request.CreateResponse(HttpStatusCode.OK, menus, Configuration.Formatters.JsonFormatter);
        }



        [HttpGet]
        [Route("modos_pedidos")]
        public HttpResponseMessage modos_pedidos()
        {
            var orderMods = Service1.Catalogos.ObtenerModosDePedido();
            return Request.CreateResponse(HttpStatusCode.OK, orderMods, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("formas_pago")]
        public HttpResponseMessage formas_pago()
        {
            var tenders = Service1.Catalogos.FormasDePago();
            return Request.CreateResponse(HttpStatusCode.OK, tenders, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("jobcodes")]
        public HttpResponseMessage jobcodes()
        {
            var perfiles = Service1.Catalogos.ObtenerPerfilesTrabajo();
            return Request.CreateResponse(HttpStatusCode.OK, perfiles, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("ModCodes")]
        public HttpResponseMessage ModCodes()
        {
            var ModeCodes = Service1.Catalogos.ObtenerModCodes();
            return Request.CreateResponse(HttpStatusCode.OK, ModeCodes, Configuration.Formatters.JsonFormatter);

        }
        [HttpGet]
        [Route("Printers")]
        public HttpResponseMessage Printers()
        {
            var Catalogos = Service1.Catalogos.ObtenerImpresoras();
            return Request.CreateResponse(HttpStatusCode.OK, Catalogos, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("Voids")]
        public HttpResponseMessage Voids()
        {
            var Voids = Service1.Catalogos.ObtenerVoids();
            return Request.CreateResponse(HttpStatusCode.OK, Voids, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("Design")]
        public HttpResponseMessage Design()
        {
            string Design = Service1.Catalogos.ObtenerDesign();
            var obj = JsonConvert.DeserializeObject(Design);
            return Request.CreateResponse(HttpStatusCode.OK, obj, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("Panels")]
        public HttpResponseMessage Panels()
        {
            var Paneles = Service1.Catalogos.ObtenerPaneles();
            return Request.CreateResponse(HttpStatusCode.OK, Paneles, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("Btns")]
        public HttpResponseMessage Btns()
        {
            var Paneles = Service1.Catalogos.ObtenerBotones();
            return Request.CreateResponse(HttpStatusCode.OK, Paneles, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("SMP")]
        public HttpResponseMessage SMP()
        {
            var SmartAlohaCodigos = Service1.Catalogos.obtenerCodigosAlohaSmart();
            return Request.CreateResponse(HttpStatusCode.OK, SmartAlohaCodigos, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("TAX")]
        public HttpResponseMessage TAX()
        {
            var SmartAlohaCodigos = Service1.Catalogos.ObtenerTaxSucursal();
            return Request.CreateResponse(HttpStatusCode.OK, SmartAlohaCodigos, Configuration.Formatters.JsonFormatter);
        }

        //ACCIONES DE ALOHA CONNECTION
        [HttpPost]
        [Route("clockin")]
        public HttpResponseMessage clockin(RequestClockIn requestClockIn)
        {
            var response = Service1.AlohaConnection.ClockIn(requestClockIn.IdTerm, requestClockIn.IdJobCode, requestClockIn.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage login(RequestLogin requestLogin)
        {
            ResponseAloha response = Service1.AlohaConnection.login(requestLogin.TermId, requestLogin.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("logout")]
        public HttpResponseMessage logout(RequestLogout requestLogout)
        {
            ResponseAloha response = Service1.AlohaConnection.logout(requestLogout.TermId, requestLogout.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OpenTable")]
        public HttpResponseMessage OpenTable(RequestOpenTable requestOpenTable)
        {
            ResponseAloha response = Service1.AlohaConnection.OpenTable(requestOpenTable.IdTerm, requestOpenTable.IdMesa, requestOpenTable.NombreMesa, requestOpenTable.NumInvitados, requestOpenTable.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("OpenTab")]
        public HttpResponseMessage OpenTab(RequestOpenTable requestOpenTable)
        {
            ResponseAloha response = Service1.AlohaConnection.OpenTab(requestOpenTable.IdTerm, requestOpenTable.IdMesa, requestOpenTable.NombreMesa, requestOpenTable.NumInvitados, requestOpenTable.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("CloseTabTable")]
        public HttpResponseMessage CloseTabTable(RequestCloseTabTable requestCloseTabTable)
        {
            ResponseAloha response = Service1.AlohaConnection.CloseTabTable(requestCloseTabTable.IdTerm, requestCloseTabTable.IdMesaInterno, requestCloseTabTable.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OpenCheck")]
        public HttpResponseMessage OpenCheck(RequestOpenCheck requestOpenCheck)
        {
            ResponseAloha response = Service1.AlohaConnection.OpenCheck(requestOpenCheck.IdTerm, requestOpenCheck.IdMesaInterno, requestOpenCheck.IdEmpleado, requestOpenCheck.IsNewCheck);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("CloseCheck")]
        public HttpResponseMessage CloseCheck(RequestCloseCheck requestCloseCheck)
        {
            ResponseAloha response = Service1.AlohaConnection.CloseCheck(requestCloseCheck.IdTerm, requestCloseCheck.IdChequeInterno, requestCloseCheck.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("AddItem")]
        public HttpResponseMessage AddItem(RequestAddItem requestAddItem)
        {
            ResponseAloha response = Service1.AlohaConnection.AddItems(requestAddItem);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OrderMode")]
        public HttpResponseMessage OrderMode(RequestOrderMode requestOrderMode)
        {
            ResponseAloha response = Service1.AlohaConnection.ConfirmOrderMode(requestOrderMode.IdTerm, requestOrderMode.IdMesa, requestOrderMode.IdModoPedido, requestOrderMode.IdEmpleado, requestOrderMode.SelectedEntries, requestOrderMode.IdCheck);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("AplyPayment")]
        public HttpResponseMessage AplyPayment(RequestAplyPayment requestAplyPayment)
        {
            var response = Service1.AlohaConnection.AplicarPago(requestAplyPayment.IdEmpleado, requestAplyPayment.IdTerm, requestAplyPayment.IdCheckId, requestAplyPayment.IdTender, requestAplyPayment.Amount, requestAplyPayment.Tip, requestAplyPayment.Digitos, requestAplyPayment.Expiration, requestAplyPayment.Info, requestAplyPayment.authorization);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("ListTables")]
        public HttpResponseMessage ListTables(RequestListTables requestListTables)
        {
            ResponseAloha response = Service1.AlohaConnection.ListTables(requestListTables.IdEmpleado, requestListTables.IdTerm);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("GetCheck")]
        public HttpResponseMessage GetCheck(RequestGetCheck requestGetCheck)
        {
            ResponseAloha response = Service1.AlohaConnection.GetCheck(requestGetCheck.IdCheck);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("DeletePayment")]
        public HttpResponseMessage DeletePayment(RequestDeletePayment requestDeletePayment)
        {
            ResponseAloha response = Service1.AlohaConnection.EliminarPago(requestDeletePayment.IdTerm, requestDeletePayment.IdCheckId, requestDeletePayment.IdPayment, requestDeletePayment.IdEmpleado);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("Print")]
        public HttpResponseMessage Print(RequestPrint requestPrint)
        {
            ResponseAloha response = Service1.AlohaConnection.Print(requestPrint.IdTerm, requestPrint.IdCheck, requestPrint.IdEmpleado, requestPrint.IdTermImpresora);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("VoidItem")]
        public HttpResponseMessage VoidItem(RequestVoidItem requestVoidItem)
        {
            ResponseAloha response = Service1.AlohaConnection.VoidItem(
                requestVoidItem.IdTerm,
                requestVoidItem.IdEmpleado,
                requestVoidItem.IdCheck,
                requestVoidItem.ItemAnulados,
                requestVoidItem.IdVoidReason);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("PrintBluetooth")]
        public HttpResponseMessage PrintBluetooth(RequestPrintBluetooth requestPrintBluetooth)
        {
            var response = Service1.AlohaConnection.PrintBluetooth(requestPrintBluetooth.IdCheck, requestPrintBluetooth.IdTable, requestPrintBluetooth.IdTerm, requestPrintBluetooth.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPut]
        [Route("UpdatePayment")]
        public HttpResponseMessage UpdatePayment(Pagos_pendientes requestPagoPendiente)
        {
            var response = Service1.AlohaConnection.UpdatePayment(requestPagoPendiente);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("ValidarPagoPendiente")]
        public HttpResponseMessage ValidarPagoPendiente(Pagos_pendientes requestPagoPendiente)
        {
            var response = Service1.AlohaConnection.ValidarPagoPendiente(requestPagoPendiente);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        #region Desktop mobile

        #endregion
        //CONTROLADORES PARA APLICACION DE ESCRITORIO
        [HttpGet]
        [Route("Users")]
        public HttpResponseMessage Users()
        {
            var response = Service1.AlohaConnection.GetUsersInSession();
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("ReleaseUser")]
        public HttpResponseMessage ReleaseUser(RequestUser requestUser)
        {
            var response = Service1.AlohaConnection.ReleaseUser(requestUser.IdUser);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }


        [HttpGet]
        [Route("AddItemNiveles")]
        public HttpResponseMessage AddItemNiveles()
        {
            ResponseAloha response = Service1.AlohaConnection.AddItemNivelesPruebas();
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("pagoPendiente")]
        public HttpResponseMessage pagoPendiente(Pagos_pendientes requestPagoPendiente)
        {
            var response = Service1.AlohaConnection.GuardarPagoPendiente(requestPagoPendiente);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("recuperarPagoPendiente/{IdEmpleado}")]
        public HttpResponseMessage RecuperarPagoPendiente(int IdEmpleado)
        {
            var response = Service1.AlohaConnection.RecuperarPagoPendiente(IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("SaveTicketSmart")]
        public HttpResponseMessage SaveTicketSmart(Ticket_smart requestPagoPendiente)
        {
            var response = Service1.AlohaConnection.SaveTicketSmart(requestPagoPendiente);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("GETTicketSmart/{IdEmpleado}/{IdCheck}")]
        public HttpResponseMessage GETTicketSmart(int IdEmpleado, int IdCheck)
        {
            var response = Service1.AlohaConnection.GETTicketSmart(IdEmpleado, IdCheck);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Eods this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("EOD")]
        public HttpResponseMessage EOD()
        {
            Service1.AlohaConnection.ProcesarEOD();
            return Request.CreateResponse(HttpStatusCode.OK, $"EOD detectado", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("PrintNewXML")]
        public HttpResponseMessage PrintNewXML(RequestPrintXML requestPrintXML)
        {

            var base64EncodedBytes = System.Convert.FromBase64String(requestPrintXML.XML);
            string XML = Encoding.UTF8.GetString(base64EncodedBytes);
            Service1.AlohaConnection.printXML(XML, requestPrintXML.IdCheck);
            return Request.CreateResponse(HttpStatusCode.OK, $"ARCHIVO XML RECIBIDO CORRECTAMENTE", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("DividirCuentas")]
        public HttpResponseMessage DividirCuentas(RequestDividirCuenta requestDividirCuenta)
        {

            Service1.AlohaConnection.DividirCuentas(requestDividirCuenta);

            return Request.CreateResponse(HttpStatusCode.OK, $"División realizada correctamente", Configuration.Formatters.JsonFormatter);
        }


        [HttpPost]
        [Route("CombineTables")]
        public HttpResponseMessage CombineTables(RequestCombineTables requestCombineTables)
        {
            Service1.AlohaConnection.CombineTables(requestCombineTables);
            return Request.CreateResponse(HttpStatusCode.OK, $"mesas unidas correctamente", Configuration.Formatters.JsonFormatter);
        }




        [HttpPost]
        [Route("PrintTicketSap")]
        public HttpResponseMessage PrintTicketSap(RequestPrintCheckSap requestCloseCheckSAP)
        {
            if (!string.IsNullOrEmpty(requestCloseCheckSAP.SAP_XML))
            {
                //Si viene con info se manda al servicio de SAP
                Service1.logger.Info($"INFORMACION SAP RECIBIDA");
                Service1.logger.Info($"XML SAP:\r\n {requestCloseCheckSAP.SAP_XML}");
                //File.WriteAllText(@".\XMLSAP.txt", requestCloseCheckSAP.SAP_XML);

                Service1.AlohaConnection.PrintTicketSap(requestCloseCheckSAP);

            }
            else
            {
                Service1.logger.Info($"REGISTRANDO VARIABLE TIPO SAP PARA ENVIO");

                //si esta vacio solo se registra variable para realizar la impresion y que se reporte 
                Service1.AlohaConnection.RegistrarVariableALOHA(requestCloseCheckSAP);
            }
            return Request.CreateResponse(HttpStatusCode.OK, $"Cheque cerrado recibido correctamente", Configuration.Formatters.JsonFormatter);
        }


        [HttpPost]
        [Route("CloseCheckSap")]
        public HttpResponseMessage CloseCheckSap(RequestCloseCheckSap requestCloseCheckSap)
        {
            Service1.logger.Info($"EVENTO CIERRE DE CHEQUE RECIBIDO, INICIADO");
            Service1.AlohaConnection.GetPagosTicketSap(requestCloseCheckSap.CheckId);
            Service1.logger.Info($"EVENTO CIERRE DE CHEQUE RECIBIDO, FIN");

            return Request.CreateResponse(HttpStatusCode.OK, $"Cheque cerrado recibido correctamente", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("HoldCheck")]
        public HttpResponseMessage HoldCheck(RequestHoldCheck requestHoldCheck)
        {
            Service1.logger.Info($"EVENTO ESPERA DE PRODUCTOS DEL CHEQUE, INICIO");
            Service1.AlohaConnection.SetHoldItemsSelected(requestHoldCheck);
            Service1.logger.Info($"EVENTO ESPERA DE PRODUCTOS DEL CHEQUE, FIN");

            return Request.CreateResponse(HttpStatusCode.OK, $"Cheque cerrado recibido correctamente", Configuration.Formatters.JsonFormatter);
        }



        [HttpGet]
        [Route("LogoACPOS")]
        public HttpResponseMessage LogoACPOS()
        {
            Service1.logger.Info($"EVENTO PARA RECUPERAR LOGO DE ALOHA, INICIO");

            var response = Service1.AlohaConnection.RecuperarBMPLogoALoha();

            Service1.logger.Info($"EVENTO PARA RECUPERAR LOGO DE ALOHA, FIN");
            return response;
        }

        [HttpGet]
        [Route("LOCALSTATE")]
        public HttpResponseMessage LOCALSTATE()
        {
            Service1.AlohaConnection.GetLocalState();

            return Request.CreateResponse(HttpStatusCode.OK, $"Cheque cerrado recibido correctamente", Configuration.Formatters.JsonFormatter);
        }
    }
}
