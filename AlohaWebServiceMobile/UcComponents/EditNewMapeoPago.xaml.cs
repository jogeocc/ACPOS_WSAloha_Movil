using AlohaWebServiceMobile.Enums;
using AlohaWebServiceMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlohaWebServiceMobile.UcComponents
{
    /// <summary>
    /// Interaction logic for EditNewMapeoPago.xaml
    /// </summary>
    public partial class EditNewMapeoPago : UserControl
    {
        public VMMapeoPagos vMMapeoPagos { get; set; }

        public bool Editar { get; set; }
        public EditNewMapeoPago(VMMapeoPagos mapeoPagos)
        {
            InitializeComponent();

            vMMapeoPagos = mapeoPagos;

            Editar = vMMapeoPagos.Editar;

            DataContext = vMMapeoPagos;

        }

        private void BtnCancelarMapeo(object sender, RoutedEventArgs e)
        {
            //Close();
        }


        private void BtnGuardar(object sender, RoutedEventArgs e)
        {

            IsEnabled = false;

            TipoRespuesta respuesta = TipoRespuesta.VACIO;

            respuesta = (TipoRespuesta)vMMapeoPagos.Guardar();

            if (respuesta == TipoRespuesta.HECHO)
            {
                vMMapeoPagos.Listar();
            }

            //loading.Close();

            IsEnabled = true;

            string mensaje = "";
            string titulo = "";
            MessageBoxImage boxImage = MessageBoxImage.Information;

            if (respuesta == TipoRespuesta.HECHO)
            {
                //Close();
            }
            else if (respuesta == TipoRespuesta.ERROR_SISTEMA)
            {
                titulo = "Error";
                mensaje = "Hubo un error al intentar guardar los cambios, por favor intente de nuevo";
                boxImage = MessageBoxImage.Error;
            }

            if (!(string.IsNullOrEmpty(mensaje) && string.IsNullOrEmpty(titulo)))
            {
                MessageBox.Show(mensaje, titulo, MessageBoxButton.OK, boxImage);
            }

        }
    }
}
