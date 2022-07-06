using AlohaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class FormaPagoALH
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public int OWNERID { get; set; }
        public TipoLogicoALH CASH { get; set; }

        public bool Seleccionado { get; set; }
    }
}
