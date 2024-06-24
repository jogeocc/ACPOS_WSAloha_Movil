using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Events
{
    public class EventsAloha
    {

        public int EOD { get; set; }

        public List<PriceChange> priceChanges { get; set; } = new List<PriceChange>();

    }
}
