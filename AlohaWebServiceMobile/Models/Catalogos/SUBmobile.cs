using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Catalogos
{
    public class SUBmobile
    {
        public int id_submenu { get; set; }
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }
        public string descripcion_personalizada { get; set; }
        public string imagen { get; set; }
        public byte status { get; set; }
    }
}
