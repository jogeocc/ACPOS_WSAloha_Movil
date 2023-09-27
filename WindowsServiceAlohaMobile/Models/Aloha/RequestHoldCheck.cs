using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    public class RequestHoldCheck : RequestAddItem
    {
        public int IdOrderMode { get; set; }
        public int IdTable { get; set; }
        public DateTime HoldStart { get; set; }
        public TimeSpan HoldEnd { get; set; }

    }
}
