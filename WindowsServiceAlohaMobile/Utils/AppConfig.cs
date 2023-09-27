using LecturaAppConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Utils
{
    public class AppConfig
    {
        public AppConfig()
        {
            IP = LACSystem.GetString("IP_SERVER_API");
            PORT = LACSystem.GetString("PUERTO");
            URL_BASE = LACSystem.GetString("API_URL_BASE");
            ID_MENU_MOVIL = LACSystem.GetInt("ID_MENU_MOVIL");
            DIRECCION_SAP = LACSystem.GetString("DIRECCION_SAP");
        }
        public string IP { get; set; }
        public string PORT { get; set; }
        public string URL_BASE { get; set; }
        public int ID_MENU_MOVIL { get; set; }
        public string DIRECCION_SAP { get; set; }
    }
}
