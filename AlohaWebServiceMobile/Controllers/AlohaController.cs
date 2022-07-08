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
            return Request.CreateResponse(HttpStatusCode.OK, $"Servicio web Aloha Mobile {App.Version} - Web Service", Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("menus")]
        public HttpResponseMessage menus()
        {

            var menus = App.Catalogos.ObtenerMenus();
            return Request.CreateResponse(HttpStatusCode.OK, menus, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("submenus")]
        public HttpResponseMessage submenus()
        {
            //TODO REALIZAR DEVOLUCION DE MODELO DE MENUS
            var submenus = App.Catalogos.ObtenerSubMenus();
            return Request.CreateResponse(HttpStatusCode.OK, submenus, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("items")]
        public HttpResponseMessage items()
        {
            //TODO REALIZAR DEVOLUCION DE MODELO DE MENUS
            var items = App.Catalogos.ObtenerItems();
            return Request.CreateResponse(HttpStatusCode.OK, items, Configuration.Formatters.JsonFormatter);
        }

        [HttpGet]
        [Route("modificadores")]
        public HttpResponseMessage modificadores()
        {
            //TODO REALIZAR DEVOLUCION DE MODELO DE MENUS
            var mods = App.Catalogos.ObtenerModificadores();
            return Request.CreateResponse(HttpStatusCode.OK, mods, Configuration.Formatters.JsonFormatter);
        }

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
            //TODO REALIZAR DEVOLUCION DE MODELO DE ESTA FUNCION
            return Request.CreateResponse(HttpStatusCode.OK, $"Servicio web Aloha Mobile {App.Version} - Web Service", Configuration.Formatters.JsonFormatter);
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
        public HttpResponseMessage login()
        {
            //TODO REALIZAR DEVOLUCION DE MODELO DE ESTA FUNCION
            while (App.ocupado)
            {

            }
            App.ocupado = true;
            bool IsSuccess = App.AlohaConnection.login(999);
            App.ocupado = false;
            return Request.CreateResponse(HttpStatusCode.OK, $"Servicio web Aloha Mobile {App.Version} - Web Service", Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("logout")]
        public HttpResponseMessage logout()
        {
            //TODO REALIZAR DEVOLUCION DE MODELO DE ESTA FUNCION
            return Request.CreateResponse(HttpStatusCode.OK, $"Servicio web Aloha Mobile {App.Version} - Web Service", Configuration.Formatters.JsonFormatter);
        }

    }
}
