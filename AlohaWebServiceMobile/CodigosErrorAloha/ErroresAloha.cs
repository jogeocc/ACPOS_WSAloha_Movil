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
            {"0xC0068001","Id terminal no autorizado" },
            {"0xC0068002","Alguien mas se encuentra logueado en la terminal" },
            {"0xC0068003","No se puede encontrar al empleado" },
            {"0xC0068004","Password de empleado no valido" },
            {"0xC0068005","Tarjeta magnetica no valida" },
            {"0xC0068006","Empleado logueado en otra terminal" },
            {"0xC0068007","No hay nadie logueado en la terminal" },
            {"0xC0068008","Ya se ha registrado(Clock in) en el sistema" },
            {"0xC0068009","Demasiados turnos en el día" },
            {"0xC006800A","Perfil de trabajo no valido para empleado" },
            {"0xC006800B","Error empleado registrando salida" },
            {"0xC006800C","No ha registrado salida" },
            {"0xC006800D","Necesita realizar checkout previamente" },
            {"0xC006800E","Error empleado registrando salida" },
            {"0xC006800F","Checkout no realizado" },
            {"0xC0068010","Checkout ya realizado" },
            {"0xC0068011","Numero de cola invalido" },
            {"0xC0068012","Numero de mesa invalido" },
            {"0xC0068013","Cheques abiertos en mesa" },
            {"0xC0068014","La mesa ya se encuentra cerrada" },
            {"0xC0068015","Solo 1 cheque por mesa permitido" },
            {"0xC0068016","Numero de cheque invalido" },
            {"0xC0068017","Cheque tiene Articulos sin ordenar" },
            {"0xC0068018","Cheque no pagado en su totalidad*****" },
            {"0xC0068019","Pago invalido" },
            {"0xC006801A","No hay impresora local" },
            {"0xC006801B","ID - Forma de pago invalido" },
            {"0xC006801C","El cheque ya ha sido cerrado" },
            {"0xC006801D","El cheque se encuentra vacio" },
            {"0xC006801E","Empleado no asigado a cajon de dinero" },
            {"0xC006801F","Cajon de empleado no esta en terminal local" },
            {"0xC0068020","Mesa ocupada en otra terminal" },
            {"0xC0068021","Articulo invalido" },
            {"0xC0068022","Error en id entry*********" },
            {"0xC0068023","Modo de orden invalido" },
            {"0xC0068024","Razon de anulación invalida" },
            {"0xC0068025","Id entry invalido*********" },
            {"0xC0068026","Articulo no disponible" },
            {"0xC0068027","Codigo de modificador invalido" },
            {"0xC0068028","Modificador no permitido para articulo padre" },
            {"0xC0068029","Requisitos de modificadores no cumplidos" },
            {"0xC006802A","Articulo no es un articulo abierto" },
            {"0xC006802B","Es necesario mas informacion para el pago" },
            {"0xC006802C","Balance de cajon de dinero del empleado no confirmado" },
            {"0xC006802D","Perfil de trabajo no permite cerrar cheques" },
            {"0xC006802E","Información de seguimiento de pago erronea" },
            {"0xC006802F","Monto de propinas declarada ilega***" },
            {"0xC0068030","Monto de efectivo declarada ilegal***" },
            {"0xC0068031","Mesa no encontrada" },
            {"0xC0068032","Mesa ocupada" },
            {"0xC0068033","No hay cajero local" },
            {"0xC0068034","Empleado no puede pagar en efectivo" },
            {"0xC0068035","Cheque cuenta con items perdidos de las categorias requeridas" },
            {"0xC0068036","Cheque tiene pagos pendientes" },
            {"0xC0068037","El cheque esta completo" },
            {"0xC0068038","No podra ser posible imprimir el certificado de regalo en el back office" },
            {"0xC0068039","Demasiadas cuentas en la mesa" },
            {"0xC006803A","Entry ya esta seleccionado" },
            {"0xC006803B","Mensaje especial no valido" },
            {"0xC006803C","Menu invalido" },
            {"0xC006803D","No hay pivote de asiento" },
            {"0xC006803E","Demasiados asientos" },
            {"0xC006803F","***SIN USO***" },
            {"0xC0068040","No soportado en modalidad QS" },
            {"0xC0068041","No soportado en modalidad TS" },
            {"0xC0068042","No es posible ajustar el pago de la gift card finalizada" },
            {"0xC0068043","Finalizando pago de gift card, se debe esperar para cerrar el cheque" },
            {"0xC0068044","El cheque es un turno previo" },
            {"0xC0068045","El empleado ha realizado su salida" },
            {"0xC0068046","Modificador de grupo invalido" },
            {"0xC0068047","Articulo no en un modificador de grupo" },
            {"0xC0068048","Ningun articulo seleccionado" },
            {"0xC0068049","Ningun articulo en movimiento" },
            {"0xC006804A","Tipo de cortesia invalida"},
            {"0xC006804B","Promocion invalida"},
            {"0xC006804C","Cortesia invalida"},
            {"0xC006804D","Promo invalida"},
            {"0xC006804E","FOHCOM actualmente ocupado"},
            {"0xC006804F","FOHCOM Error en servidor"},
            {"0xC0068050","El cheque tiene pagos que no se han verificado la firma"},
            {"0xC0068051","Parametros enviados invalidos/No esperados "},
            {"0xC0068052","Peticion eFrequency invalida, deshabilitada o no iniciada"},
            {"0xC0068053","Numero de tarjeta eFrequency invalido"},
            {"0xC0068054","Prefijo invalido para numero de tarjta eFrequency"},
            {"0xC0068055","Gerente es requerido para sobrescribir"},
            {"0xC0068056","Error incierto"},
            {"0xC0068057","Gerente no se ha registrado en sistema"},
            {"0xC0068058","El tipo de descanso indicado no es valido"},
            {"0xC0068059","La combinacion de tipo de descanso y pago no es valida"},
            {"0xC006805A","El empleado no puede descansar con mesas abiertas"},
            {"0xC006805B","El empleado esta en un descanso"},
            {"0xC006805C","El tipo de descanso indicado no es valido para este empleado"},
            {"0xC006805D","El empleado no esta en un descanso"},
            {"0xC006805E","El empleado no puede terminar su descanso, desde que los tiempos de los descansos forzados no coinciden"},
            {"0xC006805F","El password indicado no es valido, ya que contiene caracteres no numericos"},
            {"0xC0068060","El sistema no esta indicado para usar passwords"},
            {"0xC0068061","El password indicado es mas pequeño que el miniimo permitido"},
            {"0xC0068062","El password indicado es mas grande que el maximo permitido"},
            {"0xC0068063","El empleado solicitado solo tiene acceso por tarjeta magnetica"},
            {"0xC0068064","Un gerente debe autorizar la salida de este empleado"},
            {"0xC0068065","La tarjeta magnetica indicada ya esta en uso"},
            {"0xC0068066",""},
            {"0xC0068067",""},
            {"0xC0068068",""},
            {"0xC0068069",""},
            {"0xC006806A",""},
            {"0xC006806B",""},
            {"0xC006806C",""},
            {"0xC006806D",""},
            {"0xC006806E",""},
            {"0xC006806F",""},

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
                    Mensaje = "Error interno, Error no encontrado";
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
