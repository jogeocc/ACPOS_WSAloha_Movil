using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class TAX
    {
        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public string NAME { get; set; }
        public string SUBSTITUTE { get; set; }
        public string EXCLUSIVE { get; set; }
        public string INCLUSIVE { get; set; }
        public string VENDOR { get; set; }
        public decimal RATE { get; set; }
    }
}
