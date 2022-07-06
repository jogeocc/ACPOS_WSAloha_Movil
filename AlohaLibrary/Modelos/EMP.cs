using AlohaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class EMP
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public int SEC_NUM { get; set; }
        public int SSN { get; set; }
        public String SSNTEXT { get; set; }
        public String FIRSTNAME { get; set; }
        public String MIDDLENAME { get; set; }
        public String LASTNAME { get; set; }
        public String NICKNAME { get; set; }
        public int JOBCODE1 { get; set; }
        public TipoLogicoALH TERMINATED { get; set; }
    }
}
