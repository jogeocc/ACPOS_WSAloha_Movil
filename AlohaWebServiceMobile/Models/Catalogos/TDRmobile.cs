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

        /// <summary>
        /// INICIO 03/01/2022.
        /// FIN 03/01/2022.
        /// Campo de monto por defecto para formas de pago exactas
        /// </summary>
        /// <value>
        /// The monto defecto.
        /// </value>
        public double Monto_Defecto { get; set; }

        public bool Requiere_Firma { get; set; }
        
        public bool Requiere_Expiracion { get; set; }
    }
}
