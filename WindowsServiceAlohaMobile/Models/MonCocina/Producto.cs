using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.MonCocina
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Comentario { get; set; }
        public string NombreCorto { get; set; }
        public int Movimiento { get; set; }
        public string IdProductoCompuesto { get; set; }
        public List<Producto> Modificadores { get; set; } = new List<Producto>();
        public string PrecioDisplay { get; set; }
        public double Precio { get; set; }
    }
}
