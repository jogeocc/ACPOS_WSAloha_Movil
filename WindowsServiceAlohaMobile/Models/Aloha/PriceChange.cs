using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha
{
    public class PriceChange
    {
        public int IdPriceChange { get; set; }

        public double Price { get; set; }

        public TimeSpan StartHour { get; set; }
    }
}
