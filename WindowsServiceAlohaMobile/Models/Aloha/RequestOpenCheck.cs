using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    [JsonObject]
    public class RequestOpenCheck
    {
        public int IdEmpleado { get; set; }

        public int IdTerm { get; set; }
        public int IdMesaInterno { get; set; }

        public bool IsNewCheck { get; set; }
    }
}
