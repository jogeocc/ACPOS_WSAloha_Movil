using AlohaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class JOB
    {
        public int ID { get; set; }
        public string SHORTNAME { get; set; }
        public string LONGNAME { get; set; }
        public TipoLogicoALH ORDERENTRY { get; set; }
    }
}
