using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Models.Aloha
{
    public class RequestCombineTables
    {
        public int IdEmpleado { get; set; }
        public int IdTerm { get; set; }
        public int IdTableOne { get; set; }
        public int IdTableTwo { get; set; }
        public int IdFinalTable
        {
            get
            {
                return IdFinalTable;
            }
            set
            {
                IdFinalTable = (value == 0 ? IdTableTwo : value);
            }
        }

    }
}
