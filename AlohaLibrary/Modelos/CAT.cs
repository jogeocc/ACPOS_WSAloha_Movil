using AlohaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class CAT
    {
        public CAT()
        {
            Seleccionado = false;
            Items = new List<GNDITEM>();
        }

        public int ID { get; set; }
        public string NAME { get; set; }
        public TipoLogicoALH SALES { get; set; }
        public bool Seleccionado { get; set; }
        public List<GNDITEM> Items { get; set; }
    }
}
