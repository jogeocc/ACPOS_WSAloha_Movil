using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class GNDITEM
    {
        public int TYPE { get; set; }
        public int EMPLOYEE { get; set; }
        public int CHECK { get; set; }
        public int ITEM { get; set; }
        public int CATEGORY { get; set; }
        public int MODE { get; set; }
        public int PERIOD { get; set; }
        public decimal PRICE { get; set; }
        public decimal QUANTITY { get; set; }
        public decimal DISCPRIC { get; set; }
        public decimal INCLTAX { get; set; }
        public int TAXID   { get; set; }
        public int ENTRYID { get; set; }
    }
}
