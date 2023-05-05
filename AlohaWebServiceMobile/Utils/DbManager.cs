using AlohaWebServiceMobile.EntityFrameWork.Context;
using AlohaWebServiceMobile.EntityFrameWork.Models;
using AlohaWebServiceMobile.Models.Aloha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AlohaWebServiceMobile.Utils
{
    public class DbManager
    {
        public bool AddProductoEspera(RequestHoldCheck requestHoldCheck)
        {
            bool IsAdded = false;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    int secondsHold = requestHoldCheck.HoldEnd.Seconds;
                    foreach (var item in requestHoldCheck.item)
                    {
                        Producto_Pedido_Espera ProductoEspera = new Producto_Pedido_Espera();
                        ProductoEspera.IdTerminal = requestHoldCheck.IdTerm;
                        ProductoEspera.IdEmpleado = requestHoldCheck.IdEmpleado;
                        ProductoEspera.NumberCheck = requestHoldCheck.IdCheck;
                        ProductoEspera.IdCheck = requestHoldCheck.IdCheck;
                        ProductoEspera.IdProducto = item.IdItem;
                        ProductoEspera.IdEntry = item.IdEntry;
                        ProductoEspera.IdOrderMode = requestHoldCheck.IdOrderMode;
                        ProductoEspera.HoldStart = DateTime.Now;
                        ProductoEspera.HoldEnd = ProductoEspera.HoldStart.AddSeconds(secondsHold);
                        ProductoEspera.HoldMinutes = requestHoldCheck.HoldEnd.ToString();
                        db.Productos_Espera.Add(ProductoEspera);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"ERROR AL AGREGAR REGISROS A LA TABLA DE PRODUCTOS DE ESPERA.", ex);
            }
            return IsAdded;
        }
    }
}
