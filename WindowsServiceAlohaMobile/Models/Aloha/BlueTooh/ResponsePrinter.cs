using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketGenerateAloha;

namespace WindowsServiceAlohaMobile.Models.Aloha.BlueTooh
{
    [JsonObject]
    public class ResponsePrinter
    {
        public string mensaje { get; set; }
        public Ticket ticket_precuenta { get; set; }
    }
}
