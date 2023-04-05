using AlohaWebServiceMobile.EntityFrameWork.Models;
using AlohaWebServiceMobile.Models;
using AlohaWebServiceMobile.Models.Aloha;
using AlohaWebServiceMobile.Models.Aloha.Desktop;
using AlohaWebServiceMobile.Models.Transacciones;
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

namespace AlohaWebServiceMobile.Controllers
{
    [RoutePrefix("api/v1/aloha")]
    public class AlohaController : ApiController
    {
        //RECUPERACION DE CATALOGOS
        [HttpGet]
        [Route("version")]
        public HttpResponseMessage version()
        {
            object obj = new { ip = App.appConfig.IP, port = App.appConfig.PORT };
            return Request.CreateResponse(HttpStatusCode.OK, obj, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("menus")]
        public HttpResponseMessage menus()
        {

            var menus = App.Catalogos.ObtenerMenuMovil();
            return Request.CreateResponse(HttpStatusCode.OK, menus, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("menus/{ID_MENU}")]
        public HttpResponseMessage GETMenuSpecific(int ID_MENU)
        {
            var menus = App.Catalogos.ObtenerMenuMovil(ID_MENU);

            return Request.CreateResponse(HttpStatusCode.OK, menus, Configuration.Formatters.JsonFormatter);
        }



        [HttpGet]
        [Route("modos_pedidos")]
        public HttpResponseMessage modos_pedidos()
        {
            var orderMods = App.Catalogos.ObtenerModosDePedido();
            return Request.CreateResponse(HttpStatusCode.OK, orderMods, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("formas_pago")]
        public HttpResponseMessage formas_pago()
        {
            var tenders = App.Catalogos.FormasDePago();
            return Request.CreateResponse(HttpStatusCode.OK, tenders, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("jobcodes")]
        public HttpResponseMessage jobcodes()
        {
            var perfiles = App.Catalogos.ObtenerPerfilesTrabajo();
            return Request.CreateResponse(HttpStatusCode.OK, perfiles, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("ModCodes")]
        public HttpResponseMessage ModCodes()
        {
            var ModeCodes = App.Catalogos.ObtenerModCodes();
            return Request.CreateResponse(HttpStatusCode.OK, ModeCodes, Configuration.Formatters.JsonFormatter);

        }
        [HttpGet]
        [Route("Printers")]
        public HttpResponseMessage Printers()
        {
            var Catalogos = App.Catalogos.ObtenerImpresoras();
            return Request.CreateResponse(HttpStatusCode.OK, Catalogos, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("Voids")]
        public HttpResponseMessage Voids()
        {
            var Voids = App.Catalogos.ObtenerVoids();
            return Request.CreateResponse(HttpStatusCode.OK, Voids, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("Design")]
        public HttpResponseMessage Design()
        {
            string Design = App.Catalogos.ObtenerDesign();
            var obj = JsonConvert.DeserializeObject(Design);
            return Request.CreateResponse(HttpStatusCode.OK, obj, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("Panels")]
        public HttpResponseMessage Panels()
        {
            var Paneles = App.Catalogos.ObtenerPaneles();
            return Request.CreateResponse(HttpStatusCode.OK, Paneles, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("Btns")]
        public HttpResponseMessage Btns()
        {
            var Paneles = App.Catalogos.ObtenerBotones();
            return Request.CreateResponse(HttpStatusCode.OK, Paneles, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("SMP")]
        public HttpResponseMessage SMP()
        {
            var SmartAlohaCodigos = App.Catalogos.obtenerCodigosAlohaSmart();
            return Request.CreateResponse(HttpStatusCode.OK, SmartAlohaCodigos, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("TAX")]
        public HttpResponseMessage TAX()
        {
            var SmartAlohaCodigos = App.Catalogos.ObtenerTaxSucursal();
            return Request.CreateResponse(HttpStatusCode.OK, SmartAlohaCodigos, Configuration.Formatters.JsonFormatter);
        }

        //ACCIONES DE ALOHA CONNECTION
        [HttpPost]
        [Route("clockin")]
        public HttpResponseMessage clockin(RequestClockIn requestClockIn)
        {
            var response = App.AlohaConnection.ClockIn(requestClockIn.IdTerm, requestClockIn.IdJobCode, requestClockIn.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage login(RequestLogin requestLogin)
        {
            ResponseAloha response = App.AlohaConnection.login(requestLogin.TermId, requestLogin.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("logout")]
        public HttpResponseMessage logout(RequestLogout requestLogout)
        {
            ResponseAloha response = App.AlohaConnection.logout(requestLogout.TermId, requestLogout.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OpenTable")]
        public HttpResponseMessage OpenTable(RequestOpenTable requestOpenTable)
        {
            ResponseAloha response = App.AlohaConnection.OpenTable(requestOpenTable.IdTerm, requestOpenTable.IdMesa, requestOpenTable.NombreMesa, requestOpenTable.NumInvitados, requestOpenTable.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("OpenTab")]
        public HttpResponseMessage OpenTab(RequestOpenTable requestOpenTable)
        {
            ResponseAloha response = App.AlohaConnection.OpenTab(requestOpenTable.IdTerm, requestOpenTable.IdMesa, requestOpenTable.NombreMesa, requestOpenTable.NumInvitados, requestOpenTable.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("CloseTabTable")]
        public HttpResponseMessage CloseTabTable(RequestCloseTabTable requestCloseTabTable)
        {
            ResponseAloha response = App.AlohaConnection.CloseTabTable(requestCloseTabTable.IdTerm, requestCloseTabTable.IdMesaInterno, requestCloseTabTable.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OpenCheck")]
        public HttpResponseMessage OpenCheck(RequestOpenCheck requestOpenCheck)
        {
            ResponseAloha response = App.AlohaConnection.OpenCheck(requestOpenCheck.IdTerm, requestOpenCheck.IdMesaInterno, requestOpenCheck.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("CloseCheck")]
        public HttpResponseMessage CloseCheck(RequestCloseCheck requestCloseCheck)
        {
            ResponseAloha response = App.AlohaConnection.CloseCheck(requestCloseCheck.IdTerm, requestCloseCheck.IdChequeInterno, requestCloseCheck.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("AddItem")]
        public HttpResponseMessage AddItem(RequestAddItem requestAddItem)
        {
            ResponseAloha response = App.AlohaConnection.AddItems(requestAddItem);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OrderMode")]
        public HttpResponseMessage OrderMode(RequestOrderMode requestOrderMode)
        {
            ResponseAloha response = App.AlohaConnection.ConfirmOrderMode(requestOrderMode.IdTerm, requestOrderMode.IdMesa, requestOrderMode.IdModoPedido, requestOrderMode.IdEmpleado, requestOrderMode.SelectedEntries);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("AplyPayment")]
        public HttpResponseMessage AplyPayment(RequestAplyPayment requestAplyPayment)
        {
            var response = App.AlohaConnection.AplicarPago(requestAplyPayment.IdEmpleado, requestAplyPayment.IdTerm, requestAplyPayment.IdCheckId, requestAplyPayment.IdTender, requestAplyPayment.Amount, requestAplyPayment.Tip, requestAplyPayment.Digitos, requestAplyPayment.Expiration, requestAplyPayment.Info, requestAplyPayment.authorization);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("ListTables")]
        public HttpResponseMessage ListTables(RequestListTables requestListTables)
        {
            ResponseAloha response = App.AlohaConnection.ListTables(requestListTables.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("GetCheck")]
        public HttpResponseMessage GetCheck(RequestGetCheck requestGetCheck)
        {
            ResponseAloha response = App.AlohaConnection.GetCheck(requestGetCheck.IdCheck);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("DeletePayment")]
        public HttpResponseMessage DeletePayment(RequestDeletePayment requestDeletePayment)
        {
            ResponseAloha response = App.AlohaConnection.EliminarPago(requestDeletePayment.IdTerm, requestDeletePayment.IdCheckId, requestDeletePayment.IdPayment, requestDeletePayment.IdEmpleado);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("Print")]
        public HttpResponseMessage Print(RequestPrint requestPrint)
        {
            ResponseAloha response = App.AlohaConnection.Print(requestPrint.IdTerm, requestPrint.IdCheck, requestPrint.IdEmpleado, requestPrint.IdTermImpresora);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("VoidItem")]
        public HttpResponseMessage VoidItem(RequestVoidItem requestVoidItem)
        {
            ResponseAloha response = App.AlohaConnection.VoidItem(
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
            var response = App.AlohaConnection.PrintBluetooth(requestPrintBluetooth.IdCheck, requestPrintBluetooth.IdTable, requestPrintBluetooth.IdTerm);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        //CONTROLADORES PARA APLICACION DE ESCRITORIO
        [HttpGet]
        [Route("Users")]
        public HttpResponseMessage Users()
        {
            var response = App.AlohaConnection.GetUsersInSession();
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("ReleaseUser")]
        public HttpResponseMessage ReleaseUser(RequestUser requestUser)
        {
            var response = App.AlohaConnection.ReleaseUser(requestUser.IdUser);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }


        [HttpGet]
        [Route("AddItemNiveles")]
        public HttpResponseMessage AddItemNiveles()
        {
            ResponseAloha response = App.AlohaConnection.AddItemNivelesPruebas();
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("pagoPendiente")]
        public HttpResponseMessage pagoPendiente(Pagos_pendientes requestPagoPendiente)
        {
            var response = App.AlohaConnection.GuardarPagoPendiente(requestPagoPendiente);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("recuperarPagoPendiente/{IdEmpleado}")]
        public HttpResponseMessage RecuperarPagoPendiente(int IdEmpleado)
        {
            var response = App.AlohaConnection.RecuperarPagoPendiente(IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("SaveTicketSmart")]
        public HttpResponseMessage SaveTicketSmart(Ticket_smart requestPagoPendiente)
        {
            var response = App.AlohaConnection.SaveTicketSmart(requestPagoPendiente);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("GETTicketSmart/{IdEmpleado}/{IdCheck}")]
        public HttpResponseMessage GETTicketSmart(int IdEmpleado, int IdCheck)
        {
            var response = App.AlohaConnection.GETTicketSmart(IdEmpleado, IdCheck);
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
            App.AlohaConnection.ProcesarEOD();
            return Request.CreateResponse(HttpStatusCode.OK, $"EOD detectado", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("PrintNewXML")]
        public HttpResponseMessage PrintNewXML(RequestPrintXML requestPrintXML)
        {

            var base64EncodedBytes = System.Convert.FromBase64String(requestPrintXML.XML);
            string XML = Encoding.UTF8.GetString(base64EncodedBytes);
            App.AlohaConnection.printXML(XML, requestPrintXML.IdCheck);
            return Request.CreateResponse(HttpStatusCode.OK, $"ARCHIVO XML RECIBIDO CORRECTAMENTE", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("DividirCuentas")]
        public HttpResponseMessage DividirCuentas(RequestDividirCuenta requestDividirCuenta)
        {

            App.AlohaConnection.DividirCuentas(requestDividirCuenta);

            return Request.CreateResponse(HttpStatusCode.OK, $"División realizada correctamente", Configuration.Formatters.JsonFormatter);
        }


        [HttpPost]
        [Route("CombineTables")]
        public HttpResponseMessage CombineTables(RequestCombineTables requestCombineTables)
        {
            App.AlohaConnection.CombineTables(requestCombineTables);
            return Request.CreateResponse(HttpStatusCode.OK, $"mesas unidas correctamente", Configuration.Formatters.JsonFormatter);
        }




        [HttpPost]
        [Route("SendCloseCheckSAP")]
        public HttpResponseMessage CloseCheck(RequestCloseCheckSAP requestCloseCheckSAP)
        {
            if (!string.IsNullOrEmpty(requestCloseCheckSAP.SAP_XML))
            {
                //Si viene con info se manda al servicio de SAP
                App.logger.Info($"XML SAP:\r\n {requestCloseCheckSAP.SAP_XML}");
                //File.WriteAllText(@".\XMLSAP.txt", requestCloseCheckSAP.SAP_XML);
                App.AlohaConnection.SendCloseCheckSAP(requestCloseCheckSAP);

            }
            else
            {
                //si esta vacio solo se registra variable para realizar la impresion y que se reporte 
                App.AlohaConnection.RegistrarVariableALOHA(requestCloseCheckSAP);
            }



            return Request.CreateResponse(HttpStatusCode.OK, $"Cheque cerrado recibido correctamente", Configuration.Formatters.JsonFormatter);
        }
    }
}
