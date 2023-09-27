using AlohaWebServiceMobile.EntityFrameWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    public class ResponseAloha
    {
        public bool isClockIn { get; set; }
        public bool Estado { get; set; }
        public List<MesaEmpleado> mesas_empleado { get; set; }
        public int Codigo { get; set; }
        public string mensaje { get; set; }
        public string Nombre_Empleado { get; set; }
        public int idMesa { get; set; }
        public List<int> idJobs { get; set; } = new List<int>();
        public Check check { get; set; }
        public int idPago { get; set; }

        public string NombreMesa { get; set; }
        public int IdPagoPendiente { get; set; }
        public string SG_REFERENCE { get; set; } = "";

        //CAMPO PARA EL NUMCHECK CONSECUTIVO EN CUENTAS MULTIPLES
        public int NumCheck { get; set; }
        //CAMPO PAGO PENDIENTE 09-06-2023
        public Pagos_pendientes pago_Pendiente { get; set; }
    }
}
