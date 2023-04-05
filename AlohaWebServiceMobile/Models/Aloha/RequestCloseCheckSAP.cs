using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class RequestCloseCheckSAP
    {
        public int EmployeeId { get; set; }
        public int QueueId { get; set; }
        public int TableId { get; set; }
        public int CheckId { get; set; }

        public string SAP_XML { get; set; }
    }
}
