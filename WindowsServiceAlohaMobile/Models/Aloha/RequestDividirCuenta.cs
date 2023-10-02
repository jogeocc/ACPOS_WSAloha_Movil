using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    [JsonObject]
    public class RequestDividirCuenta
    {
        public int IdEmpleado { get; set; }
        public int IdTable { get; set; }
        public int IdTerm { get; set; }
        public int IdManager { get; set; }

        public List<CheckOpen> cheksOpen { get; set; }
    }

    [JsonObject]
    public class CheckOpen
    {
        public int IdCheckOrigen { get; set; }
        public int IdCheckDestino { get; set; }
        public int IdEntry { get; set; }
    }
}
