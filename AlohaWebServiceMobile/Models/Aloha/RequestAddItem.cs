using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class RequestAddItem
    {
        public int IdEmpleado { get; set; }
        public int IdTerm { get; set; }
        public int IdCheck { get; set; }
        public List<ItemAloha> item { get; set; } = new List<ItemAloha>();
    }

    public class ItemAloha
    {
        public int IdItem { get; set; }
        public int IdEntry { get; set; }
        public double Amount { get; set; }
        public List<Mod> Mods { get; set; } = new List<Mod>();
        public string SpecialMessage { get; set; } = "";
        //13/09/2022 CAMPOS ADICIONALES PARA OBTENCION DE PRODUCTOS DE PESO KG,LT, LB
        public string Unidad_Medida { get; set; }
        public double Cantidad_Peso { get; set; }
    }
    public class Mod
    {
        public int IdMod { get; set; }
        public double Amount { get; set; }
        public int IdGrupo { get; set; }
        public int ModCode { get; set; }

        public List<Mod> Mods { get; set; } = new List<Mod>();
    }
}
