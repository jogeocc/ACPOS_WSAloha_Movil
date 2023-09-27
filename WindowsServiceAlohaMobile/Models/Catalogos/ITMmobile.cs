using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Catalogos
{
    public class ITMmobile
    {
        public int id_item { get; set; }
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }
        public string descripcion_personalizada { get; set; } = "";
        public double precio { get; set; }
        public string imagen { get; set; }
        public byte status { get; set; }
        public int id_cat { get; set; }
        public List<relacionMods> modificadores { get; set; } = new List<relacionMods>();
        public List<int> impuestos { get; set; } = new List<int>();
        public List<int> categorias { get; set; } = new List<int>();
        public int price_id { get; set; }
    }
    public class relacionMods
    {
        public int idmod { get; set; }
        public int ordenpos { get; set; }
    }
}
