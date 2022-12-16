using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.EntityFrameWork.Models
{
    public class Pagos_pendientes
    {
        [Key]
        public int id { get; set; }
        public int infoPago { get; set; }
        public int IdEmpleado { get; set; }
        public string Fecha { get; set; }
        public int CheckID { get; set; }
        public string NombreMesa { get; set; }
        public string Pago { get; set; }
        public string Tip { get; set; }
        public string ReferenciaUnica { get; set; }

    }
}
