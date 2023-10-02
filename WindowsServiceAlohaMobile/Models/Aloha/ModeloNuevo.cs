using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    [JsonObject]
    public class ModeloNuevo
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string NameDisplay { get { return ID + " - " + NAME; } }

    }
}
