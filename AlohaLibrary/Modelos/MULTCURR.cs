using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class MULTCURR
    {

        public int ID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public string NAME { get; set; }
        public string SYMBOL { get; set; }
        public int EXCHNGRATE { get; set; }
        public int DCMLPLACES { get; set; }
        public bool DCMLSEPRTR { get; set; }
        public bool THSNDSEPTR { get; set; }
        public int POSFORMAT { get; set; }
        public int NEGFORMAT { get; set; }
        public string RATEPREFIX { get; set; }
        public string RATESUFFIX { get; set; }
        public string PAYMPREFIX { get; set; }
        public string PAYMSUFFIX { get; set; }
        public bool BALANCEDUE { get; set; }
        public bool CHANGEDUE { get; set; }
    }
}
