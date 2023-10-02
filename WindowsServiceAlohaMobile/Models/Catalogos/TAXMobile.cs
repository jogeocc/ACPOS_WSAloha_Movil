using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Catalogos
{
    [JsonObject]
    public class TAXMobile
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public string NAME { get; set; }
        public int SUBSTITUTE { get; set; }
        public bool EXCLUSIVE { get; set; }
        public bool INCLUSIVE { get; set; }
        public bool VENDOR { get; set; }
        public double RATE { get; set; }
    }
}
