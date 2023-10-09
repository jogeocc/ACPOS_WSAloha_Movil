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
    public class AlohaController : ApiController
    {
        //RECUPERACION DE CATALOGOS
        [HttpGet]
        [Route("version")]
        public HttpResponseMessage version()
        {
            object obj = new { ip = Program.appConfig.IP, port = Program.appConfig.PORT };

            return Request.CreateResponse(HttpStatusCode.OK, obj, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("menus")]
        public HttpResponseMessage menus()
        {

            var menus = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerMenuMovil();
            return Request.CreateResponse(HttpStatusCode.OK, menus, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("menus/{ID_MENU}")]
        public HttpResponseMessage GETMenuSpecific(int ID_MENU)
        {
            var menus = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerMenuMovil(ID_MENU);

            return Request.CreateResponse(HttpStatusCode.OK, menus, Configuration.Formatters.JsonFormatter);
        }



        [HttpGet]
        [Route("modos_pedidos")]
        public HttpResponseMessage modos_pedidos()
        {
            var orderMods = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerModosDePedido();
            return Request.CreateResponse(HttpStatusCode.OK, orderMods, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("formas_pago")]
        public HttpResponseMessage formas_pago()
        {
            var tenders = ACPOS_SERVICE_MOBILE.Catalogos.FormasDePago();
            return Request.CreateResponse(HttpStatusCode.OK, tenders, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("jobcodes")]
        public HttpResponseMessage jobcodes()
        {
            var perfiles = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerPerfilesTrabajo();
            return Request.CreateResponse(HttpStatusCode.OK, perfiles, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("ModCodes")]
        public HttpResponseMessage ModCodes()
        {
            var ModeCodes = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerModCodes();
            return Request.CreateResponse(HttpStatusCode.OK, ModeCodes, Configuration.Formatters.JsonFormatter);

        }
        [HttpGet]
        [Route("Printers")]
        public HttpResponseMessage Printers()
        {
            var Catalogos = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerImpresoras();
            return Request.CreateResponse(HttpStatusCode.OK, Catalogos, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("Voids")]
        public HttpResponseMessage Voids()
        {
            var Voids = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerVoids();
            return Request.CreateResponse(HttpStatusCode.OK, Voids, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("Design")]
        public HttpResponseMessage Design()
        {
            string Design = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerDesign();
            var obj = JsonConvert.DeserializeObject(Design);
            return Request.CreateResponse(HttpStatusCode.OK, obj, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("Panels")]
        public HttpResponseMessage Panels()
        {
            var Paneles = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerPaneles();
            return Request.CreateResponse(HttpStatusCode.OK, Paneles, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("Btns")]
        public HttpResponseMessage Btns()
        {
            var Paneles = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerBotones();
            return Request.CreateResponse(HttpStatusCode.OK, Paneles, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("SMP")]
        public HttpResponseMessage SMP()
        {
            var SmartAlohaCodigos = ACPOS_SERVICE_MOBILE.Catalogos.obtenerCodigosAlohaSmart();
            return Request.CreateResponse(HttpStatusCode.OK, SmartAlohaCodigos, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("TAX")]
        public HttpResponseMessage TAX()
        {
            var SmartAlohaCodigos = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerTaxSucursal();
            return Request.CreateResponse(HttpStatusCode.OK, SmartAlohaCodigos, Configuration.Formatters.JsonFormatter);
        }


        [HttpGet]
        [Route("PROMO")]
        public HttpResponseMessage PROMO()
        {
            var promociones = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerPromos();
            return Request.CreateResponse(HttpStatusCode.OK, promociones, Configuration.Formatters.JsonFormatter);
        }


        [HttpGet]
        [Route("COMP")]
        public HttpResponseMessage COMP()
        {
            var Cortesias = ACPOS_SERVICE_MOBILE.Catalogos.ObtenerCortesias();
            return Request.CreateResponse(HttpStatusCode.OK, Cortesias, Configuration.Formatters.JsonFormatter);
        }

        //ACCIONES DE ALOHA CONNECTION
        [HttpPost]
        [Route("clockin")]
        public HttpResponseMessage clockin(RequestClockIn requestClockIn)
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.ClockIn(requestClockIn.IdTerm, requestClockIn.IdJobCode, requestClockIn.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage login(RequestLogin requestLogin)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.login(requestLogin.TermId, requestLogin.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("logout")]
        public HttpResponseMessage logout(RequestLogout requestLogout)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.logout(requestLogout.TermId, requestLogout.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OpenTable")]
        public HttpResponseMessage OpenTable(RequestOpenTable requestOpenTable)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.OpenTable(requestOpenTable.IdTerm, requestOpenTable.IdMesa, requestOpenTable.NombreMesa, requestOpenTable.NumInvitados, requestOpenTable.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("OpenTab")]
        public HttpResponseMessage OpenTab(RequestOpenTable requestOpenTable)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.OpenTab(requestOpenTable.IdTerm, requestOpenTable.IdMesa, requestOpenTable.NombreMesa, requestOpenTable.NumInvitados, requestOpenTable.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("CloseTabTable")]
        public HttpResponseMessage CloseTabTable(RequestCloseTabTable requestCloseTabTable)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.CloseTabTable(requestCloseTabTable.IdTerm, requestCloseTabTable.IdMesaInterno, requestCloseTabTable.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OpenCheck")]
        public HttpResponseMessage OpenCheck(RequestOpenCheck requestOpenCheck)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.OpenCheck(requestOpenCheck.IdTerm, requestOpenCheck.IdMesaInterno, requestOpenCheck.IdEmpleado, requestOpenCheck.IsNewCheck);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("CloseCheck")]
        public HttpResponseMessage CloseCheck(RequestCloseCheck requestCloseCheck)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.CloseCheck(requestCloseCheck.IdTerm, requestCloseCheck.IdChequeInterno, requestCloseCheck.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("AddItem")]
        public HttpResponseMessage AddItem(RequestAddItem requestAddItem)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.AddItems(requestAddItem);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OrderMode")]
        public HttpResponseMessage OrderMode(RequestOrderMode requestOrderMode)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.ConfirmOrderMode(requestOrderMode.IdTerm, requestOrderMode.IdMesa, requestOrderMode.IdModoPedido, requestOrderMode.IdEmpleado, requestOrderMode.SelectedEntries, requestOrderMode.IdCheck);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("AplyPayment")]
        public HttpResponseMessage AplyPayment(RequestAplyPayment requestAplyPayment)
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.AplicarPago(requestAplyPayment.IdEmpleado, requestAplyPayment.IdTerm, requestAplyPayment.IdCheckId, requestAplyPayment.IdTender, requestAplyPayment.Amount, requestAplyPayment.Tip, requestAplyPayment.Digitos, requestAplyPayment.Expiration, requestAplyPayment.Info, requestAplyPayment.authorization);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("ListTables")]
        public HttpResponseMessage ListTables(RequestListTables requestListTables)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.ListTables(requestListTables.IdEmpleado, requestListTables.IdTerm);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("GetCheck")]
        public HttpResponseMessage GetCheck(RequestGetCheck requestGetCheck)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.GetCheck(requestGetCheck.IdCheck);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("DeletePayment")]
        public HttpResponseMessage DeletePayment(RequestDeletePayment requestDeletePayment)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.EliminarPago(requestDeletePayment.IdTerm, requestDeletePayment.IdCheckId, requestDeletePayment.IdPayment, requestDeletePayment.IdEmpleado);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("Print")]
        public HttpResponseMessage Print(RequestPrint requestPrint)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.Print(requestPrint.IdTerm, requestPrint.IdCheck, requestPrint.IdEmpleado, requestPrint.IdTermImpresora);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("VoidItem")]
        public HttpResponseMessage VoidItem(RequestVoidItem requestVoidItem)
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.VoidItem(
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
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.PrintBluetooth(requestPrintBluetooth.IdCheck, requestPrintBluetooth.IdTable, requestPrintBluetooth.IdTerm, requestPrintBluetooth.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPut]
        [Route("UpdatePayment")]
        public HttpResponseMessage UpdatePayment(Pagos_pendientes requestPagoPendiente)
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.UpdatePayment(requestPagoPendiente);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("ValidarPagoPendiente")]
        public HttpResponseMessage ValidarPagoPendiente(Pagos_pendientes requestPagoPendiente)
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.ValidarPagoPendiente(requestPagoPendiente);

            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        #region Desktop mobile

        #endregion
        //CONTROLADORES PARA APLICACION DE ESCRITORIO
        [HttpGet]
        [Route("Users")]
        public HttpResponseMessage Users()
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.GetUsersInSession();
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("ReleaseUser")]
        public HttpResponseMessage ReleaseUser(RequestUser requestUser)
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.ReleaseUser(requestUser.IdUser);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }


        [HttpGet]
        [Route("AddItemNiveles")]
        public HttpResponseMessage AddItemNiveles()
        {
            ResponseAloha response = ACPOS_SERVICE_MOBILE.AlohaConnection.AddItemNivelesPruebas();
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("pagoPendiente")]
        public HttpResponseMessage pagoPendiente(Pagos_pendientes requestPagoPendiente)
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.GuardarPagoPendiente(requestPagoPendiente);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("recuperarPagoPendiente/{IdEmpleado}")]
        public HttpResponseMessage RecuperarPagoPendiente(int IdEmpleado)
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.RecuperarPagoPendiente(IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("SaveTicketSmart")]
        public HttpResponseMessage SaveTicketSmart(Ticket_smart requestPagoPendiente)
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.SaveTicketSmart(requestPagoPendiente);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("GETTicketSmart/{IdEmpleado}/{IdCheck}")]
        public HttpResponseMessage GETTicketSmart(int IdEmpleado, int IdCheck)
        {
            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.GETTicketSmart(IdEmpleado, IdCheck);
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
            ACPOS_SERVICE_MOBILE.AlohaConnection.ProcesarEOD();
            return Request.CreateResponse(HttpStatusCode.OK, $"EOD detectado", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("PrintNewXML")]
        public HttpResponseMessage PrintNewXML(RequestPrintXML requestPrintXML)
        {

            var base64EncodedBytes = System.Convert.FromBase64String(requestPrintXML.XML);
            string XML = Encoding.UTF8.GetString(base64EncodedBytes);
            ACPOS_SERVICE_MOBILE.AlohaConnection.printXML(XML, requestPrintXML.IdCheck);
            return Request.CreateResponse(HttpStatusCode.OK, $"ARCHIVO XML RECIBIDO CORRECTAMENTE", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("DividirCuentas")]
        public HttpResponseMessage DividirCuentas(RequestDividirCuenta requestDividirCuenta)
        {

            ACPOS_SERVICE_MOBILE.AlohaConnection.DividirCuentas(requestDividirCuenta);

            return Request.CreateResponse(HttpStatusCode.OK, $"División realizada correctamente", Configuration.Formatters.JsonFormatter);
        }


        [HttpPost]
        [Route("CombineTables")]
        public HttpResponseMessage CombineTables(RequestCombineTables requestCombineTables)
        {
            ACPOS_SERVICE_MOBILE.AlohaConnection.CombineTables(requestCombineTables);
            return Request.CreateResponse(HttpStatusCode.OK, $"mesas unidas correctamente", Configuration.Formatters.JsonFormatter);
        }




        [HttpPost]
        [Route("PrintTicketSap")]
        public HttpResponseMessage PrintTicketSap(RequestPrintCheckSap requestCloseCheckSAP)
        {
            if (!string.IsNullOrEmpty(requestCloseCheckSAP.SAP_XML))
            {
                //Si viene con info se manda al servicio de SAP
                ACPOS_SERVICE_MOBILE.logger.Info($"INFORMACION SAP RECIBIDA");
                ACPOS_SERVICE_MOBILE.logger.Info($"XML SAP:\r\n {requestCloseCheckSAP.SAP_XML}");
                //File.WriteAllText(@".\XMLSAP.txt", requestCloseCheckSAP.SAP_XML);

                ACPOS_SERVICE_MOBILE.AlohaConnection.PrintTicketSap(requestCloseCheckSAP);

            }
            else
            {
                ACPOS_SERVICE_MOBILE.logger.Info($"REGISTRANDO VARIABLE TIPO SAP PARA ENVIO");

                //si esta vacio solo se registra variable para realizar la impresion y que se reporte 
                ACPOS_SERVICE_MOBILE.AlohaConnection.RegistrarVariableALOHA(requestCloseCheckSAP);
            }
            return Request.CreateResponse(HttpStatusCode.OK, $"Cheque cerrado recibido correctamente", Configuration.Formatters.JsonFormatter);
        }


        [HttpPost]
        [Route("CloseCheckSap")]
        public HttpResponseMessage CloseCheckSap(RequestCloseCheckSap requestCloseCheckSap)
        {
            ACPOS_SERVICE_MOBILE.logger.Info($"EVENTO CIERRE DE CHEQUE RECIBIDO, INICIADO");
            ACPOS_SERVICE_MOBILE.AlohaConnection.GetPagosTicketSap(requestCloseCheckSap.CheckId);
            ACPOS_SERVICE_MOBILE.logger.Info($"EVENTO CIERRE DE CHEQUE RECIBIDO, FIN");

            return Request.CreateResponse(HttpStatusCode.OK, $"Cheque cerrado recibido correctamente", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("HoldCheck")]
        public HttpResponseMessage HoldCheck(RequestHoldCheck requestHoldCheck)
        {
            ACPOS_SERVICE_MOBILE.logger.Info($"EVENTO ESPERA DE PRODUCTOS DEL CHEQUE, INICIO");
            ACPOS_SERVICE_MOBILE.AlohaConnection.SetHoldItemsSelected(requestHoldCheck);
            ACPOS_SERVICE_MOBILE.logger.Info($"EVENTO ESPERA DE PRODUCTOS DEL CHEQUE, FIN");

            return Request.CreateResponse(HttpStatusCode.OK, $"Cheque cerrado recibido correctamente", Configuration.Formatters.JsonFormatter);
        }



        [HttpGet]
        [Route("LogoACPOS")]
        public HttpResponseMessage LogoACPOS()
        {
            ACPOS_SERVICE_MOBILE.logger.Info($"EVENTO PARA RECUPERAR LOGO DE ALOHA, INICIO");

            var response = ACPOS_SERVICE_MOBILE.AlohaConnection.RecuperarBMPLogoALoha();

            ACPOS_SERVICE_MOBILE.logger.Info($"EVENTO PARA RECUPERAR LOGO DE ALOHA, FIN");
            return response;
        }

        [HttpGet]
        [Route("LOCALSTATE")]
        public HttpResponseMessage LOCALSTATE()
        {
            ACPOS_SERVICE_MOBILE.AlohaConnection.GetLocalState();

            return Request.CreateResponse(HttpStatusCode.OK, $"Cheque cerrado recibido correctamente", Configuration.Formatters.JsonFormatter);
        }
    }
}
