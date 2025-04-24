using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.Models.Aloha;
using WindowsServiceAlohaMobile.Models.MonCocina;

namespace WindowsServiceAlohaMobile.Rest
{
    public class RestAgenteMon
    {
        RestClient ClientAgenteMon;

        public RestAgenteMon() {

            ClientAgenteMon = new RestClient(LecturaAppConfig.LACSystem.GetString("URL_AGENTE_MONITOR_COCINA"));
        }


        public OrdenRequest ConvertirOrdenRequest(ResponseAloha resp)
        {
            var orden = new OrdenRequest
            {
                FolioOrden = resp.check?.ChceckNumber.ToString() ?? "0",
                id_forma_de_pago = resp.check?.Payments?.FirstOrDefault()?.LabelPayment ?? "EFECTIVO",
                NumeroMesa = resp.idMesa.ToString(),
                DatosMesero = new DatosMesero
                {
                    IdMesero = resp.IdEmpleadoSistema.ToString(),
                    Nombre = resp.Nombre_Empleado ?? "Mesero"
                },
                HoraOrden = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // puedes ajustar si se tiene un campo real
                EstadoOrden = "Activa", // valor fijo, puedes personalizar
                Impreso = "NO",
                Productos = resp.check?.Items?.Select((item, index) => MapProducto(item, (index + 1).ToString())).ToList() ?? new List<Producto>()
            };

            return orden;
        }

        private Producto MapProducto(Item item, string idCompuesto)
        {
            return new Producto
            {
                IdProducto = item.Id,
                Comentario = item.SpecialMessage,
                NombreCorto = item.Name,
                Movimiento = item.IdEntry,
                IdProductoCompuesto = idCompuesto,
                Modificadores = item.Mods?.Select((mod, idx) => MapProducto(mod, idCompuesto)).ToList() ?? new List<Producto>()
            };
        }


        public async Task EnviarOrdenAlohaAsync(ResponseAloha aloha)
        {
            var orden = ConvertirOrdenRequest(aloha);

            var request = new RestRequest("", Method.Post);
            request.AddJsonBody(orden);

            var response = await ClientAgenteMon.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error al enviar: {response.StatusCode} - {response.Content}");
            }
        }

    }
}
