using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.Utils;

namespace WindowsServiceAlohaMobile
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        public static AppConfig appConfig = new AppConfig();
        /// 
        static void Main()
        {

            ServiceBase[] ServicesToRun;
            var service = new ACPOS_SERVICE_MOBILE();
            ServicesToRun = new ServiceBase[]
            {
                service
            };

            //ServiceBase.Run(ServicesToRun);


            if (!Environment.UserInteractive)
            {
                ACPOS_SERVICE_MOBILE.logger.Info($"APLICACION EJECUTADA COMO SERVICIO");
                ServiceBase.Run(ServicesToRun);
                // Startup as service.
            }
            else
            {
                ACPOS_SERVICE_MOBILE.logger.Info($"APLICACION EJECUTADA COMO APP");
                service.inicio();
                // Startup as application
            }
        }
    }
}
