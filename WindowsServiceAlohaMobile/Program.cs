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
            var service = new Service1();
            ServicesToRun = new ServiceBase[]
            {

            };

            //ServiceBase.Run(ServicesToRun);


            if (!Environment.UserInteractive)
            {
                Service1.logger.Info($"APLICACION EJECUTADA COMO SERVICIO");
                ServiceBase.Run(ServicesToRun);
                // Startup as service.
            }
            else
            {
                Service1.logger.Info($"APLICACION EJECUTADA COMO APP");
                service.inicio();
                // Startup as application
            }
        }
    }
}
