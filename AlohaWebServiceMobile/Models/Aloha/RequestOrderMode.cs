using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class RequestOrderMode
    {

        public int IdTerm { get; set; }
        public int IdEmpleado { get; set; }
        public int IdMesa { get; set; }
        public int IdModoPedido { get; set; }
        public List<EntryesMode> SelectedEntries { get; set; } = new List<EntryesMode>();
    }

    public class EntryesMode
    {
        public int EntrieId { get; set; }
        public string EntrieName { get; set; }
    }
}
