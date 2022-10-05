using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Catalogos
{
    public class TDRmobile
    {
        public int id_forma_de_pago { get; set; }
        public string descripcion { get; set; }
        public bool status { get; set; }
        public bool acepta_propina { get; set; }

        public bool pin_pad { get; set; }
        public int Etiqueta_min { get; set; }
        public int Etiqueta_max { get; set; }
        public string Etiqueta_nombre { get; internal set; }
    }
}
