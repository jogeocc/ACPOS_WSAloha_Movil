using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.Enums;

namespace WindowsServiceAlohaMobile.Models.Catalogos
{
    public class EVENTSmobile
    {
        public AlohaActivation TipoActivacion { get; set; }
        //SI ES 0 NO APLICA, SOLO APLICA SI ES Weekday EL TIPO DE ACTIVACION
        public int dia { get; set; }
        // SI ESTA SIN FECHA NO APLICA
        public DateTime fecha { get; set; }
        public AlohaEvents TipoEvento { get; set; }

        public int IdPriceChange { get; set; }
        public int IdRevenueCenter { get; set; }

    }
}
