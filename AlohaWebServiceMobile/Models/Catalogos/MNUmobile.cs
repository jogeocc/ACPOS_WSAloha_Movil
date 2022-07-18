using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Catalogos
{
    public class MNUmobile
    {
        public int id_menu { get; set; }
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }
        public string descripcion_personalizada { get; set; }
        public string imagen { get; set; }
        public byte status { get; set; } = 1;

        public List<SubMenu> subMenus { get; set; } = new List<SubMenu>();
    }
    public class SubMenu
    {
        public int id { get; set; }
        public List<Item> items { get; set; } = new List<Item>();
    }

    public class Item
    {
        public int id { get; set; }
        public double precio { get; set; }
        public List<Mods> mods { get; set; } = new List<Mods>();

    }

    public class Mods
    {
        public int id_mod { get; set; }
        public List<Item> items { get; set; } = new List<Item>();
    }

}
