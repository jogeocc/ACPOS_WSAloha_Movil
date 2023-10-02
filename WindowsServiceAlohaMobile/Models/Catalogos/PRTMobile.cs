using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Catalogos
{
    [JsonObject]
    public class PRTMobile
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public int TERMINAL { get; set; }
    }
}
