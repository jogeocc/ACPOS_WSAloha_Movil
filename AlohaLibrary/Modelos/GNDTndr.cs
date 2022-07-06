using AlohaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class GNDTndr
    {
        public int EMPLOYEE { get; set; }
        public int CHECK { get; set; }
        public DateTime? DATE { get; set; }
        public DateTime? SYSDATE { get; set; }
        public int? TYPE { get; set; }
        public int? TYPEID { get; set; }
        public String IDENT { get; set; }
        public String AUTH { get; set; }
        public String EXP { get; set; }
        public String NAME { get; set; }
        public String UNIT { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TIP { get; set; }
        public int? NR { get; set; }
        public TipoLogicoALH TRACK { get; set; }
        public int? HOUSEID { get; set; }
        public int? TIPPABLE { get; set; }
        public int? MANAGER { get; set; }
        public int? HOUR { get; set; }
        public int? MINUTE { get; set; }
        public int? ID { get; set; }
        public int? AUTOGRAT { get; set; }
        public int? STRUNIT { get; set; }
        public int? REVENUE { get; set; }
        public int? OCCASION { get; set; }
        public int? SOURCE { get; set; }
        public int? PMSPOSTD { get; set; }
        public int? DRAWER { get; set; }
        public TimeSpan Hora { get; set; }
    }
}
