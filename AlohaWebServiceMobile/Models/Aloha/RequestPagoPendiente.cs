using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models
{
    public class RequestPagoPendiente
    {
        //BANDERA DE 1 PAGO PENDIENTE, 0 PAGO FINALIZADO 
        public int infoPago { get; set; }
        public int IdEmpleado { get; set; }
        public String Fecha { get; set; }
        public int CheckID { get; set; }
        public string NombreMesa { get; set; }
        public double Pago { get; set; }
        public double Tip { get; set; }
        public string ReferenciaUnica { get; set; }
    }
}
