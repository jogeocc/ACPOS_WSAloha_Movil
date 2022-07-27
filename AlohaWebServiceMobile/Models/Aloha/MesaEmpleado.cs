using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class MesaEmpleado
    {
        public bool IsTab { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public int IdMesa { get; set; }
        public List<object> Checks { get; set; }
        public List<object> Items { get; set; }
        public List<object> Payments { get; set; }
        public List<object> Promotions { get; set; }
        public List<object> Comps { get; set; }
    }
}
