using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using WindowsServiceAlohaMobile.EntityFrameWork.Context;
using WindowsServiceAlohaMobile.EntityFrameWork.Interface;
using WindowsServiceAlohaMobile.EntityFrameWork.Models;
using WindowsServiceAlohaMobile.Models.Aloha;

namespace WindowsServiceAlohaMobile.Utils
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
                        ProductoEspera.IdTable = requestHoldCheck.IdTable;
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
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL AGREGAR REGISROS A LA TABLA DE PRODUCTOS DE ESPERA.", ex);
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
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL OBTENER LA LISTA DE PRODUCTOS EN ESPERA DEL DOB", ex);
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
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL ACTUALIZAR PRODUCTOS EN ESPERA", ex);
            }
        }

        public List<Producto_Pedido_Espera> GetProductosTiempoEspera(int IdCheck)
        {
            List<Producto_Pedido_Espera> ListaProductos = new List<Producto_Pedido_Espera>();
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    ListaProductos = db.Productos_Espera.Where(P => P.IsOrdered == 0 && P.IdCheck == IdCheck).ToList();
                    ListaProductos = ListaProductos.Where(P => P.HoldEnd.Date == DateTime.Now.Date).ToList();
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL RECUPERAR PRODUCTOS PARA APPLICACION MOVIL", ex);
            }
            return ListaProductos;
        }

        //FUNCION PARA ACTUALIZAR EL REGISTRO DURANTE UN MOVIMIENTO ENTRE CUENTAS.
        public void UpdateProductosEnEspera(int idCheckOrigen, int idCheckDestino, int idEntry)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var DbProducto = db.Productos_Espera.Where(P => P.IdCheck == idCheckOrigen && P.IdEntry == idEntry).ToList();
                    Producto_Pedido_Espera producto = DbProducto.Where(P => P.HoldStart.Date == DateTime.Now.Date).First();
                    producto.IdCheck = idCheckDestino;
                    db.Productos_Espera.AddOrUpdate(producto);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL ACTUALIZAR REGISTRO", ex);
            }
        }

        public void UpdateProductosEnEspera(List<EntryesMode> selectedEntries, int idMesa, int idTerm)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var DbProducto = db.Productos_Espera.Where(P => P.IdTable == idMesa && P.IdTerminal == idTerm).ToList();
                    List<Producto_Pedido_Espera> ListaProductos = DbProducto.Where(P => P.HoldStart.Date == DateTime.Now.Date).ToList();

                    foreach (var entry in selectedEntries)
                    {
                        bool IsMatch = false;
                        foreach (var producto in ListaProductos)
                        {
                            if (producto.IdEntry == entry.EntrieId)
                            {
                                producto.IsOrdered = 1;
                                db.Productos_Espera.AddOrUpdate(producto);
                                db.SaveChanges();
                                break;
                            }
                        }
                        if (IsMatch) { break; }
                    }
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL ACTUALIZAR TABLA DE PRODUCTOS EN ESPERA", ex);
            }
        }



        //FUNCIONES PARA TABLA DE PAGOS PENDIENTES

        //retorna 0 si no existe, cualquier otro numero > 0 si existe.
        public Pagos_pendientes ValidarPagoPendiente(int EntryId)
        {
            Pagos_pendientes PagoPendiente = null;

            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    PagoPendiente = db.Pagos_pendientes.Where(P => P.EntryId == EntryId).First();
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL VALIDAR EXISTENCIA DE PAGO PENDIENTE", ex);
            }
            return PagoPendiente;

        }
        public bool UpdatePagoPendiente(int IdPagoBd, int EntryId, int infoPago, string transactionNumber, string transactionAuth)
        {
            bool ISsuccess = false;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Pagos_pendientes PagoPendiente = db.Pagos_pendientes.Where(P => P.id == IdPagoBd).First();
                    PagoPendiente.EntryId = EntryId;
                    PagoPendiente.infoPago = infoPago;
                    PagoPendiente.TransactionNumber = transactionNumber;
                    PagoPendiente.TransactionAuth = transactionAuth;
                    db.Pagos_pendientes.AddOrUpdate(PagoPendiente);
                    db.SaveChanges();
                    ISsuccess = true;
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL VALIDAR EXISTENCIA DE PAGO PENDIENTE", ex);
            }
            return ISsuccess;
        }


        public bool IsDeviceValid(string IdDevice)
        {
            bool IsValid = false;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var device = db.device.Where(D => D.Id_Device == IdDevice).First();
                    if (device != null && device.Id_Device != "")
                    {
                        IsValid = true;
                    }
                }

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar datos de tabla", ex);
            }
            return IsValid;
        }

        public bool CheckNumMaxLicencias()
        {
            bool IsValid = false;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var device = db.device.ToList();

                    int NumDevices = device.Count;
                    if (NumDevices >= 3)
                    {
                        IsValid = false;
                    }
                    else
                    {
                        IsValid = true;
                    }
                }

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar datos de tabla", ex);
            }
            return IsValid;
        }

        public bool AddDeviceLicencia(string Id_device, string Id_name)
        {
            bool IsSuccess = false;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Device device = new Device();

                    device.Id_Device = Id_device;
                    device.Device_name = Id_name;


                    db.device.Add(device);
                    db.SaveChanges();
                    IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar datos de tabla", ex);
            }
            return IsSuccess;
        }
    }
}
