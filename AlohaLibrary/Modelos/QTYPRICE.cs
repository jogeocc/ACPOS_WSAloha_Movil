using AlohaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class QTYPRICE
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public int ITEMID { get; set; }
        public int TAREID { get; set; }
        public string UNITNAME { get; set; }
        public int DECIMALS { get; set; }
        public double UNITPRICE { get; set; }
        public bool AFFECTINVT { get; set; }
        public bool ITEMQTY { get; set; }
    }
}
