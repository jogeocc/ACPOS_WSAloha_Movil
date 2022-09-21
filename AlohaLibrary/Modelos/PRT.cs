using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class PRT
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public string NAME { get; set; }
        public int TERMINAL { get; set; }
        public int PORT { get; set; }
        public int TYPE { get; set; }
        public int TIMEOUT { get; set; }
        public bool KITCHEN { get; set; }
        public int BEEPS { get; set; }
        public int BACKUP { get; set; }
        public bool STAR8340 { get; set; }
        public int GCCOPIES { get; set; }
        public string NETNAME { get; set; }
        public int WINLEFT { get; set; }
        public int WINRIGHT { get; set; }
        public int WINTOP { get; set; }
        public int WINBOTTOM { get; set; }
        public int WINCOLUMN { get; set; }
        public int NUMCOLUMN { get; set; }
        public int FONTSIZE { get; set; }
        public string FONTNAME { get; set; }
        public string OPOSNAME { get; set; }
        public int EPSONLOGO { get; set; }
        public int SLPREROUTE { get; set; }
        public int CODEPAGEID { get; set; }
        public bool SORTBYSEAT { get; set; }
        public bool PRTPRICES { get; set; }
        public bool PRTTOTAL { get; set; }
        public bool PRTTERM { get; set; }
        public bool NODELIVERY { get; set; }
        public bool NOSEATS { get; set; }
        public int FISCALPORT { get; set; }
        public string FISCALIP { get; set; }
        public int WINPRTTYPE { get; set; }
        public bool CHANGENAME { get; set; }
        public int DFTNAME { get; set; }
        public bool EXTERNAL { get; set; }
    }
}
