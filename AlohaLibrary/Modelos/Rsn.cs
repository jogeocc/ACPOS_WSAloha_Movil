using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    /// <summary>
    /// Modelo de aloha para seleccionar las razones de anulacion de un producto comandado
    /// </summary>
    public class RSN
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public string NAME { get; set; }
        public bool TRACK { get; set; }
        public bool COMONLY { get; set; }
        public bool NOREPORT { get; set; }
    }
}
