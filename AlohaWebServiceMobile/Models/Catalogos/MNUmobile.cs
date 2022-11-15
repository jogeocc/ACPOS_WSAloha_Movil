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
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }

        public int panel_id { get; set; }
        public bool UsePanels
        {
            get { return panel_id > 0; }
        }

        public List<Item> items { get; set; } = new List<Item>();
        public List<Item> Btns { get; set; } = new List<Item>();
    }

    public class Item
    {
        public int id { get; set; }
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }
        public double submenu_precio_sub { get; set; }
        public int submenu_precio_metodo { get; set; }
        public int submenu_precio_nivel { get; set; }
        public decimal item_precio { get; set; }
        public int item_precio_ID { get; set; }
        public List<Mod> mods { get; set; } = new List<Mod>();
        //13/09/2022 CAMPOS EXTRAS PARA IDENTIFICAR LOS RPODUCTOS DE TIPO KILOS
        public bool is_cantidad { get; set; }
        public string unidad_medida { get; set; } = "";
        public int unidad_decimales { get; set; }

        //CAMPOS PARA USO DE PANELES 
        public int id_panel { get; set; }
        public bool is_boton_panel { get { return id_panel > 0; } }
        public SubMenu PanelTransicion { get; set; } = new SubMenu();
    }

    public class Mod
    {
        public int id_modificador { get; set; }
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }
        public int num_min { get; set; }
        public int num_max { get; set; }
        public int num_gratis { get; set; }
        public List<Item> item_mod { get; set; } = new List<Item>();
    }

}
