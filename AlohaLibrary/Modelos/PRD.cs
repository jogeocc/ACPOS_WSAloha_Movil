using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class PRD
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public string NAME { get; set; }
        public int STARTHOUR { get; set; }
        public int STARTMIN { get; set; }
        public TimeSpan HoraInicio { get; set; }
    }
}
