using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class MesaEmpleado
    {
        public bool IsTable { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int IdMesa { get; set; }
        public List<Check> Checks { get; set; }
    }
    public class Check
    {
        public int Id { get; set; }
        public List<object> Items { get; set; }
        public List<object> Payments { get; set; }
        public List<object> Promotions { get; set; }
        public List<object> Comps { get; set; }
    }
}
