using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Catalogos
{
    [JsonObject]
    public class MODCODEmobile
    {
        public int ID { get; set; }
        public string DESC { get; set; }
        public string MOD_NAME { get; set; }
        public string INDICATOR { get; set; }
        public bool ACTIVE { get; set; }
        public double QUANTITY { get; set; }
    }
}
