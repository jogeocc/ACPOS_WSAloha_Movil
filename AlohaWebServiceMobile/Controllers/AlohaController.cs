using AlohaWebServiceMobile.Models.Aloha;
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
            object obj = new { ip = "192.168.101.110", port = "8082" };
            return Request.CreateResponse(HttpStatusCode.OK, obj, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("menus")]
        public HttpResponseMessage menus()
        {

            var menus = App.Catalogos.ObtenerMenuMovil();
            return Request.CreateResponse(HttpStatusCode.OK, menus, Configuration.Formatters.JsonFormatter);
        }
        //[HttpGet]
        //[Route("submenus")]
        //public HttpResponseMessage submenus()
        //{
        //    //TODO REALIZAR DEVOLUCION DE MODELO DE MENUS
        //    var submenus = App.Catalogos.ObtenerSubMenus();
        //    return Request.CreateResponse(HttpStatusCode.OK, submenus, Configuration.Formatters.JsonFormatter);
        //}

        //[HttpGet]
        //[Route("items")]
        //public HttpResponseMessage items()
        //{
        //    //TODO REALIZAR DEVOLUCION DE MODELO DE MENUS
        //    var items = App.Catalogos.ObtenerItems();
        //    return Request.CreateResponse(HttpStatusCode.OK, items, Configuration.Formatters.JsonFormatter);
        //}

        //[HttpGet]
        //[Route("modificadores")]
        //public HttpResponseMessage modificadores()
        //{
        //    //TODO REALIZAR DEVOLUCION DE MODELO DE MENUS
        //    var mods = App.Catalogos.ObtenerModificadores();
        //    return Request.CreateResponse(HttpStatusCode.OK, mods, Configuration.Formatters.JsonFormatter);
        //}


        [HttpGet]
        [Route("modos_pedidos")]
        public HttpResponseMessage modos_pedidos()
        {
            //TODO REALIZAR DEVOLUCION DE MODELO DE ESTA FUNCION
            var orderMods = App.Catalogos.ObtenerModosDePedido();
            return Request.CreateResponse(HttpStatusCode.OK, orderMods, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("formas_pago")]
        public HttpResponseMessage formas_pago()
        {
            //TODO REALIZAR DEVOLUCION DE MODELO DE ESTA FUNCION
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
        //ACCIONES DE ALOHA CONNECTION

        [HttpPost]
        [Route("clockin")]
        public HttpResponseMessage clockin()
        {
            //TODO REALIZAR DEVOLUCION DE MODELO DE ESTA FUNCION
            return Request.CreateResponse(HttpStatusCode.OK, $"Servicio web Aloha Mobile {App.Version} - Web Service", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage login(RequestLogin requestLogin)
        {
            var response = App.AlohaConnection.login(requestLogin.TermId, requestLogin.IdEmpleado);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("logout")]
        public HttpResponseMessage logout(RequestLogout requestLogout)
        {
            bool IsSuccess = App.AlohaConnection.logout(requestLogout.TermId);
            return Request.CreateResponse(HttpStatusCode.OK, IsSuccess, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OpenTable")]
        public HttpResponseMessage OpenTable(RequestOpenTable requestOpenTable)
        {
            var response = App.AlohaConnection.OpenTable(requestOpenTable.IdTerm, requestOpenTable.IdMesa, requestOpenTable.NombreMesa, requestOpenTable.NumInvitados);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
        [HttpPost]
        [Route("CloseTabTable")]
        public HttpResponseMessage CloseTabTable(RequestCloseTabTable requestCloseTabTable)
        {
            var response = App.AlohaConnection.CloseTabTable(requestCloseTabTable.IdTerm, requestCloseTabTable.IdMesaInterno);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("OpenCheck")]
        public HttpResponseMessage OpenCheck(RequestOpenCheck requestOpenCheck)
        {
            var response = App.AlohaConnection.OpenCheck(requestOpenCheck.IdTerm, requestOpenCheck.IdMesaInterno);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("CloseCheck")]
        public HttpResponseMessage CloseCheck(RequestCloseCheck requestCloseCheck)
        {
            var response = App.AlohaConnection.CloseCheck(requestCloseCheck.IdTerm, requestCloseCheck.IdChequeInterno);
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }

    }
}
