using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    [JsonObject]
    public class ModelCombineTables
    {
        public int IdCheck { get; set; }
        public List<int> ListIdEntrys { get; set; }

    }
}
