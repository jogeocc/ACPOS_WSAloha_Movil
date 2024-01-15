using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.SAP
{
    [JsonObject]
    public class TicketSapModel
    {
        public string Xml { get; set; }

        public List<DetallePago> socios { get; set; }

        public long Id_Rev { get; set; }

    }
    [JsonObject]
    public class DetallePago
    {
        public string CardCode { get; set; }

        public double Amount { get; set; }

        public long Id_Rev { get; set; }
    }
}
