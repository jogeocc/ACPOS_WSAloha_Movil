using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class RequestAddItem
    {
        public int IdTerm { get; set; }
        public int IdCheck { get; set; }
        public ItemAloha item { get; set; }
    }
    public class ItemAloha
    {
        public int IdItem { get; set; }
        public double Amount { get; set; }
        public List<ListsMods> Mods { get; set; }
        public string SpecialMessage { get; set; } = "";
    }
    public class ListsMods
    {
        public int IdMod { get; set; }
        public double Amount { get; set; }
    }
}
