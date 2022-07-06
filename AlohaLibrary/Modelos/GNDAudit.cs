using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class GNDAudit
    {
        public int AUDITTYPE { get; set; }
        public DateTime DOB { get; set; }
        public int HOUR { get; set; }
        public int MINUTE { get; set; }
        public int EMPLOYEE { get; set; }
        public int CHECK { get; set; }
        public int ITEM { get; set; }
        public decimal QUANTITY { get; set; }
        public decimal AMOUNT { get; set; }
        public int PREVCHECK { get; set; }
        public int PREVEMP { get; set; }
        public int ORIGCHECK { get; set; }
        public int ORIGEMP { get; set; }
        public int MANAGER { get; set; }
        public int REASON { get; set; }
        public int DATA1 { get; set; }
        public int DATA2 { get; set; }
        public int OCCASION { get; set; }
    }
}
