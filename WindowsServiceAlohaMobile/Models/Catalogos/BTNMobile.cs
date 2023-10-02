using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Catalogos
{
    [JsonObject]
    public class BTNMobile
    {
        public int ID { get; set; }
        public int QSTSMODE { get; set; }
        public int PANELID { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int CX { get; set; }
        public int CY { get; set; }
        public string TEXT { get; set; }
        public string TEXTFACE { get; set; }
        public int TEXTPOINTS { get; set; }
        public int TEXTWEIGHT { get; set; }
        public int RED { get; set; }
        public int GREEN { get; set; }
        public int BLUE { get; set; }
        public int FUNC { get; set; }
        public string PARAMS { get; set; }

        public int BKRED { get; set; }
        public int BKGREEN { get; set; }
        public int BKBLUE { get; set; }
        public bool ALOHAMOBLE { get; set; }
        public int AMPOSITION { get; set; }
    }
}
