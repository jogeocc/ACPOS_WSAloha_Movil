using AlohaWebServiceMobile.EntityFrameWork.Context;
using AlohaWebServiceMobile.EntityFrameWork.Models;
using AlohaWebServiceMobile.Models.Aloha;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AlohaWebServiceMobile.Utils
{
    public class DbManager
    {

        //TABLA DE PRODUCTOS EN ESPERA,ACCIONES
        public bool AddProductoEspera(RequestHoldCheck requestHoldCheck)
        {
            bool IsAdded = false;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    double secondsHold = requestHoldCheck.HoldEnd.TotalSeconds;
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

        public List<Producto_Pedido_Espera> GETProductosEnEspera()
        {
            List<Producto_Pedido_Espera> ListaProductosEspera = new List<Producto_Pedido_Espera>();
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    //OBTENER PRODUCTOS NO ORDENADOS
                    ListaProductosEspera = db.Productos_Espera.Where(P => P.IsOrdered == 0).ToList();

                    //OBTENER PRODUCTOS DEL DOB Y QUE YA ESTEN LISTOS PARA ORDENAR
                    ListaProductosEspera = ListaProductosEspera.Where(P => P.HoldStart.Date == DateTime.Now.Date
                    && DateTime.Now >= P.HoldEnd).ToList();
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"ERROR AL OBTENER LA LISTA DE PRODUCTOS EN ESPERA DEL DOB", ex);
            }
            return ListaProductosEspera;
        }

        public void UpdateProductosEnEspera(List<Producto_Pedido_Espera> ListProductosEspera)
        {
            try
            {

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    foreach (Producto_Pedido_Espera producto in ListProductosEspera)
                    {
                        Producto_Pedido_Espera dbProducto = db.Productos_Espera.Find(producto.ID);
                        dbProducto.IsOrdered = 1;
                        db.Productos_Espera.AddOrUpdate(dbProducto);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"ERROR AL ACTUALIZAR PRODUCTOS EN ESPERA", ex);
            }
        }
    }
}
