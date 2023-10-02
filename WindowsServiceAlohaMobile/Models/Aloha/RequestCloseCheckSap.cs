using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    [JsonObject]
    public class RequestCloseCheckSap
    {
        public int EmployeeId { get; set; }
        public int QueueId { get; set; }
        public int TableId { get; set; }
        public int CheckId { get; set; }
    }
}
