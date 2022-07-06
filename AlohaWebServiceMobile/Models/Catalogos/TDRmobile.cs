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
        public byte status { get; set; }
        public byte acepta_propina { get; set; }
    }
}
