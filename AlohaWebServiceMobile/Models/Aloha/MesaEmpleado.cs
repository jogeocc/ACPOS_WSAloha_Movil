using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class MesaEmpleado
    {
        public bool IsTable { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int IdMesa { get; set; }
        public List<Check> Checks { get; set; } = new List<Check>();
    }
    public class Check
    {
        public int Id { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public List<object> Payments { get; set; }
        public List<object> Promotions { get; set; }
        public List<object> Comps { get; set; }
    }
    public class Item
    {
        public int IdEntry { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string DisplayPrice { get; set; }
        public int NivelMod { get; set; }
        public List<Item> Mods { get; set; } = new List<Item>();
    }

    public class Mod
    {
        public int IdEntry { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string DisplayPrice { get; set; }
        public int NivelMod { get; set; }

    }
}
