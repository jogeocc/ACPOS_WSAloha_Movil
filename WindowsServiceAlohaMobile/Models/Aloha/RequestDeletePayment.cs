using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    [JsonObject]
    public class RequestDeletePayment
    {
        public int IdTerm { get; set; }
        public int IdCheckId { get; set; }
        public int IdPayment { get; set; }
        public int IdEmpleado { get; set; }

    }
}
