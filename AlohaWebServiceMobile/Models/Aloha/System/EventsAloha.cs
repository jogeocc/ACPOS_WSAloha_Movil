using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha.System
{
    public class EventsAloha
    {
        public TimeSpan HOUR { get; set; }

        //FORMATO CONVERTIBLE
        public object TypeAlohaEvent { get; set; }
        public string NameEvent { get; set; }
    }

    public class FOOTERMSGBYTERMINAL
    {
        public int IdTerminal { get; set; }
        public int IdGci { get; set; }
    }
}
