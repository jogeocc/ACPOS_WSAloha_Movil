using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    public class RequestPrintCheckSap
    {
        public int EmployeeId { get; set; }
        public int QueueId { get; set; }
        public int TableId { get; set; }
        public int CheckId { get; set; }

        //BELLAVISTA SAP
        public string SAP_XML { get; set; }
        public int CheckIdSap { get; set; }
    }
}
