using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Catalogos
{
    public class MODmobile
    {
        public int id_modificador { get; set; }
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }
        public string descripcion_personalizada { get; set; }
        public int num_min { get; set; }
        public int num_max { get; set; }
        public int num_gratis { get; set; }
        public byte status { get; set; }
        public List<itemModificador> items_modificador { get; set; } = new List<itemModificador>();
    }
    public class itemModificador
    {
        public int id_item { get; set; }
        public double precio { get; set; }
        public byte status { get; set; } = 1;
        public int ordenpos { get; set; } = 1;
    }
}
