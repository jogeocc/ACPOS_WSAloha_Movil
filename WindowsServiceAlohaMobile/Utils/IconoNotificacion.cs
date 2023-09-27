using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsServiceAlohaMobile.Utils
{
    public static class IconoNotificacion
    {
        static NotifyIcon notifyIcon;
        static Icon icono;

        public static void CrearNotifyIcon()
        {
            string location = Assembly.GetEntryAssembly().Location;
            string DirectorioEjecucion = Path.GetDirectoryName(location);
            string pathIco = DirectorioEjecucion + @"\Resource\Icons\icono.ico";
            icono = new Icon(pathIco);
            notifyIcon = new NotifyIcon
            {
                Icon = icono,
                Visible = true,
                Text = $"Agente {App.Version}",
            };
            ContextMenu contextMenu = new ContextMenu();
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenu.MenuItems.Add("Información", Informacion);
            contextMenu.MenuItems.Add("Salir", Salir);
            contextMenu.MenuItems.Add("Configuración", Configuration);
            notifyIcon.ContextMenu = contextMenu;
            DesplegarNotificacion("Agente", $"Funcionando con exito {App.Version}");
            App.logger.Info("Icono contextual creado correctamente");
        }
        private static void Salir(object sender, EventArgs e)
        {
            DesplegarNotificacion("Salir", "Cerrando agente");
            notifyIcon.Dispose();
            Environment.Exit(0);
        }

        private static void Informacion(object sender, EventArgs e)
        {
            DesplegarNotificacion("Agente", $"Agente funcionando correctamente - {App.Version}");
        }
        public static void DesplegarNotificacion(string titulo, string mensaje)
        {
            notifyIcon.ShowBalloonTip(3000, titulo, mensaje, ToolTipIcon.Info);
        }

        private static void Configuration(object sender, EventArgs e)
        {
            App.VentanaPrincipal.Show();
        }
    }
}
