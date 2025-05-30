using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.MonCocina
{
    public class OrdenRequest
    {
        public string FolioOrden { get; set; }
        public string id_forma_de_pago { get; set; }
        public string NumeroMesa { get; set; }
        public DatosMesero DatosMesero { get; set; }
        public string HoraOrden { get; set; }
        public string EstadoOrden { get; set; }
        public string Impreso { get; set; }
        public List<Producto> Productos { get; set; }
        public string referencia { get; set; }
    }
}
