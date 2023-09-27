using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    public class RequestOpenTable
    {
        public int IdEmpleado { get; set; }
        public int IdTerm { get; set; }
        public int IdMesa { get; set; }
        public string NombreMesa { get; set; }
        public int NumInvitados { get; set; }
    }
}
