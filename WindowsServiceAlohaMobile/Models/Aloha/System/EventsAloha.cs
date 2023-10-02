using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.System
{
    [JsonObject]
    public class EventsAloha
    {
        public TimeSpan HOUR { get; set; }

        //FORMATO CONVERTIBLE
        public object TypeAlohaEvent { get; set; }
        public string NameEvent { get; set; }
    }
    [JsonObject]
    public class FOOTERMSGBYTERMINAL
    {
        public int IdTerminal { get; set; }
        public int IdGci { get; set; }
    }
}
