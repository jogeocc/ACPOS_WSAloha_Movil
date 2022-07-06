using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Enums;

namespace AlohaLibrary.Modelos
{
    public class OrdenALH
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public bool Seleccionado { get; set; }
        public TipoLogicoALH ACTIVE { get; set; }
    }
}
