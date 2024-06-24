using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    [Description("PCID.DBF=>LISTA DE IDS DE LOS ITEMS Y PRECIO A CAMBIAR JUNTO A SU PRICE ID CHANGE PADRE")]
    public class PCID
    {
        public int ID { get; set; }
        public int ITEMID { get; set; }
        public int OWNERID { get; set; }
        public double PRICE { get; set; }
        public bool ALLOWEDIT { get; set; }
        public double MINPRICE { get; set; }
        public double RECPRICE { get; set; }
    }
}
