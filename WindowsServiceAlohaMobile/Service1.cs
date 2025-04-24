using AlohaLibrary.Contexto;
using AlohaLibrary.Implementaciones;
using AlohaLibrary.Modelos;
using EncryptDataJson;
using log4net;
using Newtonsoft.Json;
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
        public static string Version = "Versión 1.6.0";
        public static readonly ILog logger = LogManager.GetLogger("Comandero_Movil");
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
        public static bool isServicio = true;
        public static Licencia Licencia = new Licencia();
        public static List<ITM> itemsAskDesc;

        public ACPOS_SERVICE_MOBILE()
        {
            InitializeComponent();
        }
        HttpSelfHostServer server;
        public void inicio()
        {
            isServicio = false;
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


                BloqueoLicencia();

                IniciarWebService();
                bdInterna.users = funcionesArchivo.ReadTrans();
                CargarInfoAlohaIni();
                if (IsError)
                {
                    Environment.Exit(0);
                }

                logger.Info($"CARGANDO SUBPROCESOS");
                ProcesarOrdenPendiente();

                //CARGANDO ITEMS DE ALOHA
                logger.Info($"CARGANDO ITEMS DE ALOHA QUE SI SOLICTAN NOMBRE");

                var pathALoha = AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder();

                using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
                {
                    itemsAskDesc = new ITMServicio(contextoAlh).GetAll().Where(itm => itm.ASKDESC == AlohaLibrary.Enums.TipoLogicoALH.Y ).ToList();


                    string itemsAskDescJson = JsonConvert.SerializeObject(itemsAskDesc, Formatting.Indented);

                    // Registrar la información en el log
                    logger.Info($"PRODUCTOS QUE SOLICITAN NOMBRE: \n{itemsAskDescJson}");

                }



                logger.Info($"SISTEMA CARGADO CON EXITO");
                if (!isServicio)
                {
                    logger.Info($"EJECUCION COMO APP LOGRADA");
                    while (true)
                    {
                    }
                }
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
            AlohaIni.MaxPassLenghtEmployee = NumMaxPassEmp;
            string IsOnlyTables = iniAloha.Read("PIVOTSEATING", "Ibertech");

            if (IsOnlyTables.ToLower() == "false")
            {
                AlohaIni.UseSeats = false;
            }
            else
            {
                AlohaIni.UseSeats = true;

            }
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


        private void BloqueoLicencia()
        {

            /*logger.Error("Error en la licencia, favor de contactar al administrador");
            if (DateTime.Now > new DateTime(2025, 1, 1, 0, 0, 1)) {
                Environment.Exit(0);
            } */
        }


    }
}
