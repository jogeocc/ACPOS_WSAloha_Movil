using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Catalogos
{
    [JsonObject]
    public class VOIDMobile
    {
        public int ID { get; set; }

        public string NAME { get; set; }
    }
}
