using EncryptDataJson;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Windows;
using WindowsServiceAlohaMobile.Models.Aloha.Transacciones;
using WindowsServiceAlohaMobile.Rest;
using WindowsServiceAlohaMobile.Utils;

namespace WindowsServiceAlohaMobile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class ACPOS_SERVICE_MOBILE : ServiceBase
    {
        public static string Version = "Versión 21";
        public static readonly ILog logger = LogManager.GetLogger("Aloha_vapiano");
        public bool iniciar = false;
        public bool IsError = false;
        public static EstructurarData Catalogos = new EstructurarData();
        public static AlohaConnection AlohaConnection = new AlohaConnection();
        public static BdInterna bdInterna = new BdInterna();
        public static FuncionesArchivo funcionesArchivo = new FuncionesArchivo();
        public static bool IsBusy = false;
        public static LecturaINI iniAloha = new LecturaINI(AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder() + @"\aloha.ini");
        public static InfoAloha AlohaIni = new InfoAloha();
        public static EncryptJSON EncryptDataJson = new EncryptJSON();
        public static RestSAP restSAP = new RestSAP();
        public static DbManager DbManager = new DbManager();
        public static Thread HiloProductoPendiente;


        public ACPOS_SERVICE_MOBILE()
        {
            InitializeComponent();
        }
        HttpSelfHostServer server;
        public void inicio()
        {
            OnStart(new string[] { });
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                var thisProcess = Process.GetCurrentProcess();
                if ((Process.GetProcessesByName(thisProcess.ProcessName).Count() > 1))
                    Environment.Exit(0);
                logger.Info("------------------------------------------------");
                logger.Info($"Iniciando sistema {Version}");

                IniciarWebService();
                bdInterna.users = funcionesArchivo.ReadTrans();
                CargarInfoAlohaIni();
                if (IsError)
                {
                    Environment.Exit(0);
                }

                logger.Info($"CARGANDO SUBPROCESOS");
                ProcesarOrdenPendiente();
                logger.Info($"SISTEMA CARGADO CON EXITO");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

        protected override void OnStop()
        {
            try
            {
                server.CloseAsync();
                Environment.Exit(0);

            }
            catch (Exception ex)
            {
                logger.Error($"ERROR CERRANDO SERVICIO", ex);
            }
        }

        private void IniciarWebService()
        {

            string url_base = String.Format(Program.appConfig.URL_BASE, Program.appConfig.IP, Program.appConfig.PORT);
            HttpSelfHostConfiguration config_server = new HttpSelfHostConfiguration(url_base);
            config_server.MaxReceivedMessageSize = 2147483647;
            config_server.MapHttpAttributeRoutes();
            server = new HttpSelfHostServer(config_server);
            var task = server.OpenAsync();
            task.Wait();
        }

        private void CargarInfoAlohaIni()
        {
            int.TryParse(iniAloha.Read("NUMEMPDIGITS", "Ibertech"), out int NumMinEmp);
            AlohaIni.MinNumLenghtEmployee = NumMinEmp;
            int.TryParse(iniAloha.Read("MAXPASSWORD", "Ibertech"), out int NumMaxPassEmp);
            AlohaIni.MinNumLenghtEmployee = NumMaxPassEmp;

        }

        public static void ProcesarOrdenPendiente()
        {
            HiloProductoPendiente = new Thread(() =>
            {
                while (true)
                {
                    AlohaConnection.ProcesarProductosEnEspera();
                    Thread.Sleep(1000);
                }
            });
            HiloProductoPendiente.Start();
        }


    }
}
