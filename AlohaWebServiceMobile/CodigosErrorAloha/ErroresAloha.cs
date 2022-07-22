using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.CodigosErrorAloha
{
    public class ErroresAloha
    {
        public static Dictionary<string, string> Errores = new Dictionary<string, string>()
        {
            {"0x01","Id terminal no autorizado" },
            {"0x02","Alguien mas se encuentra logueado en la terminal" },
            {"0x03","No se puede encontrar al empleado" },
            {"0x04","Password de empleado no valido" },
            {"0x05","Tarjeta magnetica no valida" },
            {"0x06","Empleado logueado en otra terminal" },
            {"0x07","No hay nadie logueado en la terminal" },
            {"0x08","Ya se ha registrado(Clock in) en el sistema" },
            {"0x09","Demasiados turnos en el día" },
            {"0x0A","Perfil de trabajo no valido para empleado" },
            {"0x0B","Error empleado registrando salida" },
            {"0x0C","No ha registrado salida" },
            {"0x0D","Necesita realizar checkout previamente" },
            {"0x0E","Error empleado registrando salida" },
            {"0x0F","Checkout no realizado" },
            {"0x10","Checkout ya realizado" },
            {"0x11","Numero de cola invalido" },
            {"0x12","Numero de mesa invalido" },
            {"0x13","Cheques abiertos en mesa" },
            {"0x14","La mesa ya se encuentra cerrada" },
            {"0x15","Solo 1 cheque por mesa permitido" },
            {"0x16","Numero de cheque invalido" },
            {"0x17","Cheque tiene Articulos sin ordenar" },
            {"0x18","Cheque no pagado en su totalidad*****" },
            {"0x19","Pago invalido" },
            {"0x1A","No hay impresora local" },
            {"0x1B","ID - Forma de pago invalido" },
            {"0x1C","El cheque ya ha sido cerrado" },
            {"0x1D","El cheque se encuentra vacio" },
            {"0x1E","Empleado no asigado a cajon de dinero" },
            {"0x1F","Cajon de empleado no esta en terminal local" },
            {"0x20","Mesa ocupada en otra terminal" },
            {"0x21","Articulo invalido" },
            {"0x22","Error en id entry*********" },
            {"0x23","Modo de orden invalido" },
            {"0x24","Razon de anulación invalida" },
            {"0x25","Id entry invalido*********" },
            {"0x26","Articulo no disponible" },
            {"0x27","Codigo de modificador invalido" },
            {"0x28","Modificador no permitido para articulo padre" },
            {"0x29","Requisitos de modificadores no cumplidos" },
            {"0x2A","Articulo no es un articulo abierto" },
            {"0x2B","Es necesario mas informacion para el pago" },
            {"0x2C","Balance de cajon de dinero del empleado no confirmado" },
            {"0x2D","Perfil de trabajo no permite cerrar cheques" },
            {"0x2E","Información de seguimiento de pago erronea" },
            {"0x2F","Monto de propinas declarada ilega***" },
            {"0x30","Monto de efectivo declarada ilegal***" },
            {"0x31","Mesa no encontrada" },
            {"0x32","Mesa ocupada" },
            {"0x33","No hay cajero local" },
            {"0x34","Empleado no puede pagar en efectivo" },
            {"0x35","Cheque cuenta con items perdidos de las categorias requeridas" },
            {"0x36","Cheque tiene pagos pendientes" },
            {"0x37","El cheque esta completo" },
            {"0x38","No podra ser posible imprimir el certificado de regalo en el back office" },
            {"0x39","Demasiadas cuentas en la mesa" },
            {"0x3A","Entry ya esta seleccionado" },
            {"0x3B","Mensaje especial no valido" },
            {"0x3C","Menu invalido" },
            {"0x3D","No hay pivote de asiento" },
            {"0x3E","Demasiados asientos" },
            {"0x3F","***SIN USO***" },
            {"0x40","No soportado en modalidad QS" },
            {"0x41","No soportado en modalidad TS" },
            {"0x42","No es posible ajustar el pago de la gift card finalizada" },
            {"0x43","Finalizando pago de gift card, se debe esperar para cerrar el cheque" },
            {"0x44","El cheque es un turno previo" },
            {"0x45","" },
            {"0x46","" },
            {"0x47","" },
            {"0x48","" },
            {"0x49","" },
            {"0x4A","" },

        };

        public static string MensajeMobile(string CodigoError)
        {
            string Mensaje = "{0} - {1}";
            try
            {
                var messaje = Errores.FirstOrDefault(K => K.Key.Contains(CodigoError));
                if (messaje.Key != null)
                {
                    Mensaje = String.Format(Mensaje, messaje.Key.Trim(), messaje.Value.Trim());
                }
                else
                {
                    Mensaje = "Error interno, reportar al área de sistemas";
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error interno, reportar al área de sistemas";
            }
            return Mensaje;

        }
    }
}
