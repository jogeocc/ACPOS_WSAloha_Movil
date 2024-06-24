using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WindowsServiceAlohaMobile.Models.Catalogos
{
    [JsonObject]
    public class PCIDmobile
    {
        public int ID { get; set; }
        public int ITEMID { get; set; }
        public int OWNERID { get; set; }
        public decimal PRICE { get; set; }
        public bool ALLOWEDIT { get; set; }
        public decimal MINPRICE { get; set; }
        public decimal RECPRICE { get; set; }
    }
}
