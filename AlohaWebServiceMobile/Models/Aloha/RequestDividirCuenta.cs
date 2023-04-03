using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class RequestDividirCuenta
    {
        public int IdEmpleado { get; set; }
        public int IdCheck { get; set; }
        public int IdTable { get; set; }
        public int IdTerm { get; set; }
        public int IdManager { get; set; }

        public List<CheckOpen> cheksOpen { get; set; }
    }

    public class CheckOpen
    {
        public List<int> ListIdEntrys { get; set; }
    }
}
