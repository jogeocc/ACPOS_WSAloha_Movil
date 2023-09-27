using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.SAP
{
    public class TicketSapModel
    {
        public string Xml { get; set; }

        public List<DetallePago> socios { get; set; }

    }

    public class DetallePago
    {
        public string CardCode { get; set; }

        public double Amount { get; set; }
    }
}
