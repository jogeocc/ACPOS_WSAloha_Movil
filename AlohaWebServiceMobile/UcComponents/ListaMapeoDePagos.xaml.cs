using AlohaWebServiceMobile.EntityFrameWork.Enums;
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
    /// Lógica de interacción para ListaMapeoDePagos.xaml
    /// </summary>
    public partial class ListaMapeoDePagos : UserControl
    {
        private VMMapeoPagos vMMapeoPagos { get; set; }
        public ListaMapeoDePagos()
        {
            InitializeComponent();
            vMMapeoPagos = new VMMapeoPagos();
            vMMapeoPagos.Listar();
            DataContext = vMMapeoPagos;
        }

        private void BtnCancelarMapeo(object sender, RoutedEventArgs e)
        {
            //App.CambiarPantalla((int)EnumPantalla.ListaMapeo);
            App.VentanaPrincipal.Hide();
        }

        private void BtnAgregarMapeo(object sender, RoutedEventArgs e)
        {
            AbrirVentanaFormulario();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!vMMapeoPagos.ValidaEliminacion()) return;

            string titulo = "Eliminar";
            string mensaje = "¿Seguro que desea eliminar el registro?";

            MessageBoxImage boxImage = MessageBoxImage.Question;

            var result = MessageBox.Show(mensaje, titulo, MessageBoxButton.YesNo, boxImage);

            if (result == MessageBoxResult.No) return;

            EntityFrameWork.Enums.TipoRespuesta respuesta = EntityFrameWork.Enums.TipoRespuesta.VACIO;


            respuesta = vMMapeoPagos.Eliminar();

            if (respuesta == EntityFrameWork.Enums.TipoRespuesta.HECHO)
            {
                vMMapeoPagos.Listar();
            }

            IsEnabled = true;

            mensaje = "";
            titulo = "";
            boxImage = MessageBoxImage.Information;

            if (respuesta == EntityFrameWork.Enums.TipoRespuesta.ERROR_SISTEMA)
            {
                titulo = "Error";
                mensaje = "Hubo un error al intentar eliminar el registro, por favor intente de nuevo";
                boxImage = MessageBoxImage.Error;
            }

            if (!(string.IsNullOrEmpty(mensaje) && string.IsNullOrEmpty(titulo)))
            {
                MessageBox.Show(mensaje, titulo, MessageBoxButton.OK, boxImage);

            }
        }

        private void AbrirVentanaFormulario(bool editar = false)
        {
            IsEnabled = false;

            vMMapeoPagos.InicializarFormulario(editar);
            App.CambiarPantalla((int)EnumPantalla.EditMapeo);

            //NuevoEditMap nuevoEdit = new NuevoEditMap(vMMapeoPagos);
            //nuevoEdit.Closed += Form_Closed;
            //nuevoEdit.Show();
        }

        private void Form_Closed(object sender, EventArgs e)
        {
            IsEnabled = true;
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            AbrirVentanaFormulario(true);
        }
    }
   

}
