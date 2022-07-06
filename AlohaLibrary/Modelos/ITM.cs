using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class ITM
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public String SHORTNAME { get; set; }
        public String CHITNAME { get; set; }
        public String LONGNAME { get; set; }
        public String LONGNAME2 { get; set; }
        public String BOHNAME { get; set; }
        public String ABBREV { get; set; }
        public int TAXID { get; set; }
        public int TAXID2 { get; set; }
        public int VTAXID { get; set; }
        public int PRIORITY { get; set; }
        public int ROUTING { get; set; }
        public int PRINTONCHK { get; set; }
        public int COMBINE { get; set; }
        public int HIGHLIGHT { get; set; }
        public int SURCHARGE { get; set; }
        public String SURCHRGMOD { get; set; }
        public decimal COST { get; set; }
        public int MOD1 { get; set; }
        public int MOD2 { get; set; }
        public int MOD3 { get; set; }
        public int MOD4 { get; set; }
        public int MOD5 { get; set; }
        public int MOD6 { get; set; }
        public int MOD7 { get; set; }
        public int MOD8 { get; set; }
        public int MOD9 { get; set; }
        public int MOD10 { get; set; }
        public String ASKDESC { get; set; }
        public String ASKPRICE { get; set; }
        public String ISREFILL { get; set; }
        public int VROUTING { get; set; }
        public int IS_KVI { get; set; }
        public String TRACKFOH { get; set; }
        public int PRICE_ID { get; set; }
        public decimal PRICE { get; set; }
    }
}
