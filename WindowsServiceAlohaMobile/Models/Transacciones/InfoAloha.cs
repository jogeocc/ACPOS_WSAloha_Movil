using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Transacciones
{
    [JsonObject]
    public class InfoAloha
    {
        public int MinNumLenghtEmployee { get; set; }
        public int MaxPassLenghtEmployee { get; set; }
        public bool UseSeats { get; set; }
    }
}
