using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Models
{
    public class Ticket_smart
    {
        [Key]
        public int idTicket { get; set; }
        public int IdEmpleado { get; set; }
        public string Fecha { get; set; }
        public int CheckID { get; set; }
        public string NombreMesa { get; set; }
        public string ReferenciaUnica { get; set; }
        public string SG_REFERENCE { get; set; }
    }
}
