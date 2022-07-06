using AlohaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class TDR
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public string NAME { get; set; }
        public TipoLogicoALH CASH { get; set; }        
        public TipoLogicoALH ACTIVE { get; set; }
        public bool Seleccionado { get; set; }

        public TipoLogicoALH TIPS { get; set; }
    }
}
