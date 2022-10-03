using AlohaWebServiceMobile.Models.Aloha;
using AlohaWebServiceMobile.Models.Aloha.Desktop;
using AlohaWebServiceMobile.Models.Transacciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            ResponseAloha response = App.AlohaConnection.AddItem(requestAddItem); ;
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OrderMode")]
        public HttpResponseMessage OrderMode(RequestOrderMode requestOrderMode)
        {
            ResponseAloha response = App.AlohaConnection.ConfirmOrderMode(requestOrderMode.IdTerm, requestOrderMode.IdMesa, requestOrderMode.IdModoPedido, requestOrderMode.IdEmpleado);
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
                requestVoidItem.IdEntry,
                requestVoidItem.IdVoidReason);
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
    }
}
