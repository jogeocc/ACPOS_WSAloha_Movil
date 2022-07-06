using AlohaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class REV
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public string NAME { get; set; }
        public TipoLogicoALH AUTOGRAT { get; set; }
        public TipoLogicoALH NUMTABS { get; set; }
        public decimal MINGRAT { get; set; }
        public int PRINTCHK { get; set; }
        public TipoLogicoALH WAITFORAUT { get; set; }
        public string TIPLINE { get; set; }
        public string ROOMLINE { get; set; }
        public TipoLogicoALH NOTIPS { get; set; }
        public string GRATTEXT { get; set; }
        public decimal GRATPERCNT { get; set; }
        public decimal GRATDOLLAR { get; set; }
        public TipoLogicoALH PIVOTSEAT { get; set; }
        public TipoLogicoALH PIVOTGST { get; set; }
        public int PIVOTCAT { get; set; }
        public TipoLogicoALH ENTREE { get; set; }
        public int ENTREECAT { get; set; }
        public TipoLogicoALH BARGUEST { get; set; }
        public TipoLogicoALH MEMBINCNUM { get; set; }
        public int DISPFLD1 { get; set; }
        public int DISPFLD2 { get; set; }
        public int DISPFLD3 { get; set; }
        public int GRATTAXID { get; set; }
        public int MINGSTGRTX { get; set; }
        public TipoLogicoALH VERPMSINFO { get; set; }
        public TipoLogicoALH ITMGRATTAX { get; set; }
    }
}
