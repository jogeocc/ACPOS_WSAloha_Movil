using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class ResponseAloha
    {
        public bool isClockIn { get; set; }
        public object mesas_empleado { get; set; }
        public int Codigo { get; set; }
        public string mensaje { get; set; }
    }
}
