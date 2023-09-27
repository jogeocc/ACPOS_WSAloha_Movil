using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    public class RequestAplyPayment
    {
        public int IdTerm { get; set; }
        public int IdCheckId { get; set; }
        public int IdTender { get; set; }
        public double Amount { get; set; }
        public double Tip { get; set; }
        public string Digitos { get; set; }
        public string Expiration { get; set; }
        public string Info { get; set; }
        public string authorization { get; set; }
        public int IdEmpleado { get; set; }
    }
}
