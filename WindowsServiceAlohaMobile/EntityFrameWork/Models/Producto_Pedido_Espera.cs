using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Models
{
    public class Producto_Pedido_Espera
    {
        [Key]
        public int ID { get; set; }
        public int IdTerminal { get; set; }
        public int IdEmpleado { get; set; }
        public int NumberCheck { get; set; }
        public int IdCheck { get; set; }
        public int IdTable { get; set; }
        public int IdProducto { get; set; }
        public int IdEntry { get; set; }
        public int IdOrderMode { get; set; }
        public DateTime HoldStart { get; set; }
        public DateTime HoldEnd { get; set; }
        public string HoldMinutes { get; set; }
        public byte IsOrdered { get; set; }
    }
}
