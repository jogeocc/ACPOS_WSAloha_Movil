using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string SG_REFERENCE { get; set; } = "";

        //CAMPOS NUEVOS
        public int TipoPago { get; set; }
        //08-06-2023
        public int EntryId { get; set; }
        //09-06-2023
        public int TableId { get; set; }

        public string TransactionNumber { get; set; }
        public string TransactionAuth { get; set; }
    }
}
