using AlohaWebServiceMobile.Models.Transacciones;
using AlohaWebServiceMobile.Utils;
using AlohaWebServiceMobile.Views.Modals;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.Windows;

namespace AlohaWebServiceMobile
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        public static string Version = "Versión 4";
        public static readonly ILog logger = LogManager.GetLogger("Aloha_vapiano");
        public bool iniciar = false;
        public bool IsError = false;
        public ViewLoading splash = new ViewLoading();
        public static EstructurarData Catalogos = new EstructurarData();
        public static AlohaConnection AlohaConnection = new AlohaConnection();
        public static AppConfig appConfig = new AppConfig();
        public static BdInterna bdInterna = new BdInterna();
        public static FuncionesArchivo funcionesArchivo = new FuncionesArchivo();
        public static bool IsBusy = false;
        public static LecturaINI iniAloha = new LecturaINI(AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder() + @"\aloha.ini");
        public static InfoAloha Aloha = new InfoAloha();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var thisProcess = Process.GetCurrentProcess();
                if ((Process.GetProcessesByName(thisProcess.ProcessName).Count() > 1))
                    Environment.Exit(0);
                logger.Info("Iniciando sistema...");
                splash.Show();

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        IniciarWebService();
                        bdInterna.users = funcionesArchivo.ReadTrans();
                        CargarInfoAlohaIni();
                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        logger.Error(ex);
                    }
                }).ContinueWith(task =>
                {
                    if (IsError)
                    {
                        //splash.Close();
                        Environment.Exit(0);
                    }

                    CargaIcono();

                    //splash.Hide();
                }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        private void CargaIcono()
        {
            IconoNotificacion.CrearNotifyIcon();
            splash.Hide();

        }
        private void IniciarWebService()
        {

            string url_base = String.Format(appConfig.URL_BASE, appConfig.IP, appConfig.PORT);
            HttpSelfHostConfiguration config_server = new HttpSelfHostConfiguration(url_base);
            config_server.MapHttpAttributeRoutes();
            var server = new HttpSelfHostServer(config_server);
            var task = server.OpenAsync();
            task.Wait();
        }
        private void CargarInfoAlohaIni()
        {
            int.TryParse(iniAloha.Read("NUMEMPDIGITS", "Ibertech"), out int NumMinEmp);
            Aloha.MinNumLenghtEmployee = NumMinEmp;
        }
    }
}