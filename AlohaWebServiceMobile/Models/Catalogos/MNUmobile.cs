using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Catalogos
{
    public class MNUmobile
    {
        public int id_menu { get; set; }
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }
        public string descripcion_personalizada { get; set; }
        public string imagen { get; set; }
        public byte status { get; set; } = 1;

    }
}
