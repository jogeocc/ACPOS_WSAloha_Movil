using Aloha.SDK.Common;
using AlohaWebServiceMobile.CodigosErrorAloha;
using AlohaWebServiceMobile.Enums;
using AlohaWebServiceMobile.Models.Aloha;
using AlohaWebServiceMobile.Models.Aloha.Desktop;
using AlohaWebServiceMobile.Models.Transacciones;
using LasaFOHLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Utils
{
    public class AlohaConnection
    {

        private SdkFunctions _sdkFunctions = new SdkFunctions();

        private IIberFuncs23 xFunction;
        private IIberDepot depot;
        private SdkFunctions SdkFunctions;
        public ResponseAloha login(int IdTerm, int IdEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                User UserInSesion = App.bdInterna.users.Find(u => u.IdEmpleado == IdEmpleado);

                if (UserInSesion == null)
                {
                    VerificarIber();
                    int IdSistema = xFunction.LogIn(IdTerm, IdEmpleado, IdEmpleado.ToString(), "");
                    responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                    responseAloha.isClockIn = IsAlreadyClockIn(IdSistema);
                    responseAloha.mensaje = "Login realizado con exito";
                    responseAloha.Nombre_Empleado = NombreEmpleado(IdSistema);
                    responseAloha.idJobs = IdsJobsEmpleado(IdSistema);
                    responseAloha.mesas_empleado = RecuperarMesas(IdSistema);
                    LogoutInterno(IdTerm);
                    App.bdInterna.users.Add(new User
                    {
                        IdEmpleado = IdEmpleado,
                        UserName = NombreEmpleado(IdEmpleado)
                    });
                }
                else
                {
                    responseAloha.mensaje = $"Usuario ya en sesion";
                    responseAloha.Codigo = (int)CodigosError.ERROR;
                    App.logger.Error($"El usuario ya esta en sesion");
                }

            }
            catch (Exception ex)
            {
                responseAloha.mensaje = $"Error al intentar ingresar con el usuario {IdEmpleado} - {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error("Error al ingresar con el usuario tal", ex);
                LogoutInterno(IdTerm);
            }
            return responseAloha;
        }

        public ResponseAloha ClockIn(int IdTerm, int IdJobCode, int idEmpleado)
        {
            ResponseAloha response = new ResponseAloha();
            try
            {
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                xFunction.ClockIn(IdTerm, IdJobCode);
                response.Estado = true;
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Codigo = (int)CodigosError.ERROR;
                response.mensaje = $"Error al intentar registrarse con el usuario {idEmpleado} - {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error("Error al intentar registrarse con el usuario", ex);
                LogoutInterno(IdTerm);
            }
            return response;
        }

        public ResponseAloha OpenTable(int IdTerm, int idNumMesa, string NombreMesa, int NumInvitados, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                int IdMesaInterno = xFunction.AddTable(IdTerm, 0, idNumMesa, NombreMesa, NumInvitados);
                responseAloha.idMesa = IdMesaInterno;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Mesa abierta con exito";
                responseAloha.NombreMesa = GetTabTableName(IdMesaInterno);
                //LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Estado = false;
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.mensaje = $"Error abriendo mesa {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error al abrir mesa id = {idNumMesa}", ex);
                LogoutInterno(IdTerm);
            }
            return responseAloha;
        }

        public ResponseAloha OpenTab(int IdTerm, int idNumMesa, string NombreMesa, int NumInvitados, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                //Para abrir un tab, por defecto debe de ser el numero de mesa en 0
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                int IdMesaInterno = xFunction.AddTable(IdTerm, 0, idNumMesa, NombreMesa, NumInvitados);
                responseAloha.idMesa = IdMesaInterno;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Cuenta abierta";
                responseAloha.NombreMesa = GetTabTableName(IdMesaInterno);

            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error abriendo mesa-cuenta {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error abriendo mesa-cuenta id = {idNumMesa}", ex);
                LogoutInterno(IdTerm);
            }
            return responseAloha;
        }

        public ResponseAloha OpenCheck(int IdTerm, int IdMesaInterno, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();

            try
            {
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                int idChequeInterno = xFunction.AddCheck(IdTerm, IdMesaInterno);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = $"Cuenta abierda con id {idChequeInterno}";
                responseAloha.idMesa = idChequeInterno;
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al abrir cheque {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error al abrir cheque", ex);
                LogoutInterno(IdTerm);
            }
            return responseAloha;
        }

        public ResponseAloha CloseCheck(int IdTerm, int IdCheckInterno, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                xFunction.CloseCheck(IdTerm, IdCheckInterno);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = $"Cuenta Cerrada con id ";
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al cerrar cheque {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error al cerrar cheque", ex);
                LogoutInterno(IdTerm);
            }
            return responseAloha;

        }

        public ResponseAloha CloseTabTable(int IdTerm, int IdMesaInterno, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                xFunction.CloseTable(IdTerm, IdMesaInterno);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Mesa/Cuenta cerrada con éxito";
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al cerrar mesa {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error al cerrar mesa", ex);
                LogoutInterno(IdTerm);
            }
            return responseAloha;
        }



        public ResponseAloha logout(int IdTerm, int idEmpleado)
        {
            ResponseAloha response = new ResponseAloha();
            try
            {

                VerificarIber();
                //xFunction.LogOut(IdTerm);
                User UserInSesion = App.bdInterna.users.Find(u => u.IdEmpleado == idEmpleado);
                if (UserInSesion != null)
                {
                    App.bdInterna.users.Remove(UserInSesion);
                }
                response.mensaje = "Salida realizada con éxito";

                response.Estado = true;
            }
            catch (Exception ex)
            {
                response.Codigo = (int)CodigosError.ERROR;
                response.Estado = false;
                response.mensaje = $"Error al salir de terminal {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error al salir de la terminal {IdTerm}", ex);
            }
            return response;
        }

        public ResponseAloha ClockOut(int IdTerm, double tips, double DeclaredCash)
        {
            ResponseAloha response = new ResponseAloha();
            try
            {
                VerificarIber();
                xFunction.PerformCheckout(IdTerm, DeclaredCash);
                xFunction.ClockOut(IdTerm, tips);
            }
            catch (Exception ex)
            {
                response.Codigo = (int)CodigosError.ERROR;
                response.Estado = false;
                response.mensaje = $"Error al hacer salida {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error al hacer salida {IdTerm}", ex);
                LogoutInterno(IdTerm);
            }
            return response;
        }

        public ResponseAloha AddItems(RequestAddItem requestAddItem)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                LoginInterno(requestAddItem.IdTerm, requestAddItem.IdEmpleado);

                foreach (var item in requestAddItem.items)
                {
                    int idEntry = xFunction.BeginItem(requestAddItem.IdTerm, requestAddItem.IdCheck, item.IdItem, "", item.Amount);
                    #region modificadores
                    foreach (var mod in item.Mods)
                    {
                        xFunction.ModItem(requestAddItem.IdTerm, idEntry, mod.IdMod, "", mod.Amount, mod.ModCode);
                    }
                    #endregion
                    xFunction.EndItem(requestAddItem.IdTerm);
                    if (!string.IsNullOrEmpty(item.SpecialMessage) || !string.IsNullOrEmpty(item.Unidad_Medida))
                    {
                        string Mensaje = "";

                        Mensaje += item.Cantidad_Peso > 0 ? item.Cantidad_Peso.ToString() : "";

                        Mensaje += !string.IsNullOrEmpty(item.Unidad_Medida) ? item.Unidad_Medida : "";

                        Mensaje += !string.IsNullOrEmpty(item.SpecialMessage) ? $" {item.SpecialMessage}" : "";

                        xFunction.ApplySpecialMessage(requestAddItem.IdTerm, requestAddItem.IdCheck, idEntry, Mensaje);
                    }
                    responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                    responseAloha.mensaje = "Producto insertado con exito";
                }

                LogoutInterno(requestAddItem.IdTerm);

            }
            catch (Exception ex)
            {
                App.logger.Error("Error al agregar item", ex);
            };

            return responseAloha;
        }


        public ResponseAloha AddItem(RequestAddItem requestAddItem)
        {
            ResponseAloha responseAloha = new ResponseAloha();

            bool error = false;
            while (!error)
            {
                try
                {

                    VerificarIber();
                    LoginInterno(requestAddItem.IdTerm, requestAddItem.IdEmpleado);

                    int idEntry = xFunction.BeginItem(requestAddItem.IdTerm, requestAddItem.IdCheck, requestAddItem.item.IdItem, "", requestAddItem.item.Amount);
                    #region modificadores
                    foreach (var mod in requestAddItem.item.Mods)
                    {
                        xFunction.ModItem(requestAddItem.IdTerm, idEntry, mod.IdMod, "", mod.Amount, mod.ModCode);
                    }
                    #endregion
                    xFunction.EndItem(requestAddItem.IdTerm);
                    if (!string.IsNullOrEmpty(requestAddItem.item.SpecialMessage) || !string.IsNullOrEmpty(requestAddItem.item.Unidad_Medida))
                    {
                        string Mensaje = "";

                        Mensaje += requestAddItem.item.Cantidad_Peso > 0 ? requestAddItem.item.Cantidad_Peso.ToString() : "";

                        Mensaje += !string.IsNullOrEmpty(requestAddItem.item.Unidad_Medida) ? requestAddItem.item.Unidad_Medida : "";

                        Mensaje += !string.IsNullOrEmpty(requestAddItem.item.SpecialMessage) ? $" {requestAddItem.item.SpecialMessage}" : "";

                        xFunction.ApplySpecialMessage(requestAddItem.IdTerm, requestAddItem.IdCheck, idEntry, Mensaje);
                    }
                    responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                    responseAloha.mensaje = "Producto insertado con exito";
                    error = true;
                    LogoutInterno(requestAddItem.IdTerm);
                }
                catch (Exception ex)
                {
                    responseAloha.Codigo = (int)CodigosError.ERROR;
                    responseAloha.Estado = false;
                    responseAloha.mensaje = $"Error al agregar item {(ErroresAloha.MensajeMobile(ex.Message))}";
                    App.logger.Error("Error al agregar item", ex);
                    LogoutInterno(requestAddItem.IdTerm);
                };
            }


            return responseAloha;
        }


        public ResponseAloha AddSpecialMessage(int IdTerm, int IdCheckId, int IdEntry, string Message)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                xFunction.ApplySpecialMessage(IdTerm, IdCheckId, IdEntry, Message);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Producto insertado con exito";
            }
            catch (Exception ex)
            {
                App.logger.Error("Error al agregar item", ex);
            };

            return responseAloha;
        }
        public ResponseAloha ConfirmOrderMode(int IdTerm, int IdMesa, int IdModoPedido, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                xFunction.OrderItems(IdTerm, IdMesa, IdModoPedido);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Productos ordenados con exito";
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al confirmar pedido {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error("Error al confirmar pedido", ex);

                LogoutInterno(IdTerm);
            };
            return responseAloha;
        }

        public ResponseAloha AplicarPago(int idEmpleado, int IdTerm, int IdCheckId, int IdTender, double Amount, double Tip, string Digitos = "", string Expiration = "", string Info = "", string authorization = "")
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                responseAloha.idPago = xFunction.ApplyPayment(IdTerm, IdCheckId, IdTender, Amount, Tip, Digitos, Expiration, Info, authorization);
                responseAloha.Estado = true;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Pago aplicado con exito";
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Estado = false;
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.mensaje = $"Error al aplicar pago,{ErroresAloha.MensajeMobile(ex.Message)}";
                App.logger.Error("Error al aplicar pago", ex);
                LogoutInterno(IdTerm);
            }
            return responseAloha;
        }

        public ResponseAloha EliminarPago(int IdTerm, int IdCheckId, int IdPayment, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                xFunction.DeletePayment(IdTerm, IdCheckId, IdPayment);
                responseAloha.Estado = true;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Forma de pago eliminada";
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Estado = false;
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.mensaje = $"Error al eliminar pago,{ErroresAloha.MensajeMobile(ex.Message)}";
                App.logger.Error("Error al eliminar pago", ex);
                LogoutInterno(IdTerm);
            }
            return responseAloha;
        }

        public ResponseAloha Print(int idTerm, int idCheck, int IdEmpleado, int idTermImpresora)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                LoginInterno(idTerm, IdEmpleado);
                xFunction.SetObjectAttribute((int)COMEnums.INTERNAL_CHECKS, idCheck, "ID_RUTEO", idTermImpresora.ToString());
                xFunction.PrintCheck(idTerm, idCheck);
                responseAloha.Estado = true;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Enviando tarea de impresión";
                LogoutInterno(idTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Estado = false;
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.mensaje = $"Error al imprimir,{ErroresAloha.MensajeMobile(ex.Message)}";
                App.logger.Error("Error al imprimir", ex);
                LogoutInterno(idTerm);
            }
            return responseAloha;
        }


        public object VoidItem()
        {
            try
            {
                VerificarIber();
                //LoginInterno();
            }
            catch (Exception ex)
            {

            }
            return new object();
        }


        //FUNCIONES DE CONTROL DE DATOS

        public ResponseAloha ListTables(int IdEmpleado)
        {
            VerificarIber();
            ResponseAloha responseAloha = new ResponseAloha();
            responseAloha.Codigo = (int)CodigosError.NO_ERROR;
            responseAloha.mensaje = "Mesas recuperadas con exito";
            responseAloha.Nombre_Empleado = NombreEmpleado(IdEmpleado);
            responseAloha.mesas_empleado = RecuperarMesas(IdEmpleado);
            return responseAloha;
        }

        private List<MesaEmpleado> RecuperarMesas(int IdEmpleado)
        {
            List<MesaEmpleado> ListaMesas = new List<MesaEmpleado>();
            try
            {
                IberEnum EnumEmpleados = depot.FindObjectFromId((int)COMEnums.INTERNAL_EMPLOYEES, IdEmpleado);
                IberObject empleado = EnumEmpleados.First();
                IberEnum MesasEmpleado = empleado.GetEnum((int)COMEnums.INTERNAL_EMP_OPEN_TABLES);
                IberObject MesaAbierta = MesasEmpleado.First();
                //MESAS ABIERTAS
                while (MesasEmpleado != null)
                {
                    MesaEmpleado mesaEmpleado = new MesaEmpleado();
                    mesaEmpleado.Id = MesaAbierta.GetLongVal("ID");
                    mesaEmpleado.Name = MesaAbierta.GetStringVal("NAME");
                    mesaEmpleado.IsTable = MesaAbierta.GetBoolVal("TYPE") == 0 ? false : true;
                    mesaEmpleado.IdMesa = MesaAbierta.GetLongVal("TABLEDEF_ID");


                    IberEnum ChequesEmpleado = MesaAbierta.GetEnum((int)COMEnums.INTERNAL_TABLES_CHECKS);
                    IberObject ChequeAbierto = ChequesEmpleado.First();
                    //CHEQUES ABIERTOS DE LA MESA
                    try
                    {
                        while (ChequesEmpleado != null)
                        {
                            Check check = new Check();
                            check.Id = ChequeAbierto.GetLongVal("ID");
                            check.ChceckNumber = SdkFunctions.GetCheckNumberFromCheckId(check.Id);
                            ////ITEMS
                            //try
                            //{
                            //    IberEnum ItemsEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_ENTRIES);
                            //    IberObject ItemAbierto = ItemsEmpleado.First();
                            //    int IdPadre = 0;
                            //    while (ItemAbierto != null)
                            //    {
                            //        Item item = new Item();
                            //        item.IdEntry = ItemAbierto.GetLongVal("ID");
                            //        item.Name = ItemAbierto.GetStringVal("DISP_NAME");
                            //        item.Price = ItemAbierto.GetDoubleVal("PRICE");
                            //        item.DisplayPrice = ItemAbierto.GetStringVal("DISP_PRICE").Trim(); ;
                            //        item.NivelMod = ItemAbierto.GetLongVal("LEVEL");

                            //        int IsMessage = ItemAbierto.GetLongVal("TYPE");



                            //        if (IsMessage == 0)
                            //        {
                            //            if (item.NivelMod == 0)
                            //            {
                            //                IdPadre = item.IdEntry;
                            //                check.Items.Add(item);
                            //            }
                            //            else
                            //            {
                            //                check.Items.First(I => I.IdEntry == IdPadre).Mods.Add(item);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            check.Items.First(I => I.IdEntry == IdPadre).SpecialMessage = item.Name;
                            //        }
                            //        ItemAbierto = ItemsEmpleado.Next();
                            //    }

                            //}
                            //catch (Exception ex)
                            //{

                            //}
                            ////PAGOS APLICADOS A LA MESA
                            //try
                            //{
                            //    IberEnum PagosEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_PAYMENTS);
                            //    IberObject PagoAplicado = PagosEmpleado.First();
                            //    while (PagoAplicado != null)
                            //    {
                            //        Payment payment = new Payment();
                            //        payment.IdPayment = PagoAplicado.GetLongVal("ID");
                            //        payment.IdTender = PagoAplicado.GetLongVal("TENDER_ID");
                            //        payment.Tip = PagoAplicado.GetDoubleVal("TIP");
                            //        payment.Amount = PagoAplicado.GetDoubleVal("AMOUNT");
                            //        check.Payments.Add(payment);
                            //        PagoAplicado = PagosEmpleado.Next();
                            //    }

                            //}

                            //catch (Exception ex)
                            //{

                            //}

                            ////Promociones aplicadas a la mesa
                            //try
                            //{
                            //    IberEnum PromocionesEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_PROMOS);
                            //    IberObject PromocionAbierto = PromocionesEmpleado.First();
                            //    while (PromocionAbierto != null)
                            //    {
                            //        Promotion promotion = new Promotion();
                            //        promotion.Id = PromocionAbierto.GetLongVal("ID");
                            //        promotion.IdPromo = PromocionAbierto.GetLongVal("PROMOTION_ID");
                            //        promotion.Name = PromocionAbierto.GetStringVal("IDENT");
                            //        promotion.AmountDiscount = PromocionAbierto.GetDoubleVal("AMOUNT");
                            //        check.Promotions.Add(promotion);
                            //        PromocionAbierto = PromocionesEmpleado.Next();
                            //    }
                            //}
                            //catch (Exception ex)
                            //{

                            //}
                            ////Cortesias aplicadas a la mesa
                            //try
                            //{
                            //    IberEnum CortesiasEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_COMPS);
                            //    IberObject CortesiaAbierta = CortesiasEmpleado.First();
                            //    while (CortesiaAbierta != null)
                            //    {
                            //        Comp Comp = new Comp();
                            //        Comp.Id = CortesiaAbierta.GetLongVal("ID");
                            //        Comp.IdComp = CortesiaAbierta.GetLongVal("COMPTYPE_ID");
                            //        Comp.Unit = CortesiaAbierta.GetStringVal("UNIT");
                            //        Comp.Name = CortesiaAbierta.GetStringVal("NAME");
                            //        Comp.AmountDiscount = CortesiaAbierta.GetDoubleVal("AMOUNT");
                            //        check.Comps.Add(Comp);
                            //        CortesiaAbierta = CortesiasEmpleado.Next();
                            //    }
                            //}
                            //catch (Exception ex)
                            //{

                            //}

                            mesaEmpleado.Checks.Add(check);
                            ChequeAbierto = ChequesEmpleado.Next();
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    ListaMesas.Add(mesaEmpleado);
                    MesaAbierta = MesasEmpleado.Next();
                }
            }
            catch (Exception ex)
            {
            }
            return ListaMesas;
        }

        public ResponseAloha GetCheck(int idCheck)
        {
            VerificarIber();
            ResponseAloha responseAloha = new ResponseAloha();
            responseAloha.check = RecuperarCheque(idCheck);

            if (responseAloha.check != null)
            {
                responseAloha.mensaje = "Cheque recueprado correctamente";
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
            }
            else
            {
                responseAloha.mensaje = "Error recuperando data del cheque";
                responseAloha.Codigo = (int)CodigosError.ERROR;
            }
            return responseAloha;
        }

        private Check RecuperarCheque(int IdCheck)
        {
            Check check = new Check();
            try
            {
                check.Id = IdCheck;

                IberObject ChequeAbierto = depot.FindObjectFromId((int)COMEnums.INTERNAL_CHECKS, IdCheck).First();
                //ITEMS
                check.Amount = ChequeAbierto.GetDoubleVal("SUBTOTAL");
                check.Tax = ChequeAbierto.GetDoubleVal("TAX");
                try
                {
                    IberEnum ItemsEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_ENTRIES);
                    IberObject ItemAbierto = ItemsEmpleado.First();
                    int IdPadre = 0;
                    while (ItemAbierto != null)
                    {
                        Item item = new Item();
                        item.Id = ItemAbierto.GetLongVal("DATA");
                        item.IdEntry = ItemAbierto.GetLongVal("ID");
                        item.Name = ItemAbierto.GetStringVal("DISP_NAME");
                        item.Price = ItemAbierto.GetDoubleVal("PRICE");
                        item.DisplayPrice = ItemAbierto.GetStringVal("DISP_PRICE").Trim();
                        item.ModCode = ItemAbierto.GetLongVal("MOD_CODE");
                        item.NivelMod = ItemAbierto.GetLongVal("LEVEL");
                        int IsMessage = ItemAbierto.GetLongVal("TYPE");
                        item.Ordered = ItemAbierto.GetBoolVal("SELECTED") > 0;
                        item.Ordered1 = ItemAbierto.GetLongVal("MODE");
                        item.Modstring = ItemAbierto.GetStringVal("MOD_STRING");

                        if (IsMessage == 0)
                        {
                            if (item.NivelMod == 0)
                            {
                                IdPadre = item.IdEntry;
                                check.Items.Add(item);
                            }
                            else
                            {
                                check.Items.First(I => I.IdEntry == IdPadre).Mods.Add(item);
                            }
                        }
                        else
                        {
                            check.Items.First(I => I.IdEntry == IdPadre).SpecialMessage = item.Name;
                        }
                        ItemAbierto = ItemsEmpleado.Next();
                    }

                }
                catch (Exception ex)
                {

                }
                //PAGOS APLICADOS A LA MESA
                try
                {
                    IberEnum PagosEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_PAYMENTS);
                    IberObject PagoAplicado = PagosEmpleado.First();
                    while (PagoAplicado != null)
                    {
                        Payment payment = new Payment();
                        payment.IdPayment = PagoAplicado.GetLongVal("ID");
                        payment.IdTender = PagoAplicado.GetLongVal("TENDER_ID");
                        payment.Tip = PagoAplicado.GetDoubleVal("TIP");
                        payment.Amount = PagoAplicado.GetDoubleVal("AMOUNT");
                        check.Payments.Add(payment);
                        PagoAplicado = PagosEmpleado.Next();
                    }

                }
                catch (Exception ex)
                {
                    App.logger.Error($"Error al obtener pagos aplicados en la cuenta");
                }

                //Promociones aplicadas a la mesa
                try
                {
                    IberEnum PromocionesEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_PROMOS);
                    IberObject PromocionAbierto = PromocionesEmpleado.First();
                    while (PromocionAbierto != null)
                    {
                        Promotion promotion = new Promotion();
                        promotion.Id = PromocionAbierto.GetLongVal("ID");
                        promotion.IdPromo = PromocionAbierto.GetLongVal("PROMOTION_ID");
                        promotion.Name = PromocionAbierto.GetStringVal("IDENT");
                        promotion.AmountDiscount = PromocionAbierto.GetDoubleVal("AMOUNT");
                        check.Promotions.Add(promotion);
                        PromocionAbierto = PromocionesEmpleado.Next();
                    }
                }
                catch (Exception ex)
                {

                }
                //Cortesias aplicadas a la mesa
                try
                {
                    IberEnum CortesiasEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_COMPS);
                    IberObject CortesiaAbierta = CortesiasEmpleado.First();
                    while (CortesiaAbierta != null)
                    {
                        Comp Comp = new Comp();
                        Comp.Id = CortesiaAbierta.GetLongVal("ID");
                        Comp.IdComp = CortesiaAbierta.GetLongVal("COMPTYPE_ID");
                        Comp.Unit = CortesiaAbierta.GetStringVal("UNIT");
                        Comp.Name = CortesiaAbierta.GetStringVal("NAME");
                        Comp.AmountDiscount = CortesiaAbierta.GetDoubleVal("AMOUNT");
                        check.Comps.Add(Comp);
                        CortesiaAbierta = CortesiasEmpleado.Next();
                    }
                }
                catch (Exception ex)
                {

                }
                double AmountPayed = 0;
                //Aritmetica para Monto pendiente de pagar
                if (check.Payments.Count > 0)
                {

                    check.Payments.ForEach((Pago) =>
                    {
                        AmountPayed += Pago.Amount;
                    });
                }
                check.AmountDue = check.Amount - AmountPayed;

            }
            catch (Exception ex)
            {
                check = null;
            }
            return check;
        }

        private List<int> IdsJobsEmpleado(int IdEmpleado)
        {
            List<int> ListaJobs = new List<int>();
            try
            {
                IberEnum EnumEmpleados = depot.FindObjectFromId((int)COMEnums.INTERNAL_EMPLOYEES, IdEmpleado);
                IberObject empleado = EnumEmpleados.First();
                for (int i = 0; i < 10; i++)
                {
                    string NameJob = $"JOBCODE{(i + 1)}";
                    int job = empleado.GetLongVal(NameJob);
                    if (job > 0)
                    {
                        ListaJobs.Add(job);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al recuperar los jobs del empelado {IdEmpleado}", ex);
            }
            return ListaJobs;
        }

        public string NombreEmpleado(int IdEmpleado)
        {
            string nombre = "";
            try
            {
                IberEnum EnumEmpleados = depot.FindObjectFromId((int)COMEnums.INTERNAL_EMPLOYEES, IdEmpleado);
                IberObject empleado = EnumEmpleados.First();
                nombre = empleado.GetStringVal("NICKNAME");
            }
            catch (Exception ex)
            {
                App.logger.Error("Error recuperando nombre del empleado", ex);
            }

            return nombre;
        }

        private bool IsAlreadyClockIn(int IdEmpleado)
        {
            bool IsClocked = false;
            try
            {
                IberEnum EnumEmpleados = depot.FindObjectFromId((int)COMEnums.INTERNAL_EMPLOYEES, IdEmpleado);
                IberObject empleado = EnumEmpleados.First();
                var clock = empleado.GetBoolVal("CLOCKED_IN");
                IsClocked = empleado.GetBoolVal("CLOCKED_IN") == 1 ? true : false;
            }
            catch (Exception ex)
            {
                App.logger.Error("", ex);
            }
            return IsClocked;

        }

        private string GetTabTableName(int idTableTab)
        {
            string Name = "Error_mesa";
            try
            {
                IberEnum Mesas = depot.FindObjectFromId((int)COMEnums.INTERNAL_TABLES, idTableTab);
                IberObject Mesa = Mesas.First();
                Name = Mesa.GetStringVal("NAME");
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al recuperar nombre de la mesa", ex);
            }

            return Name;
        }

        private void VerificarIber()
        {
            xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
            depot = AlohaSdkFactory.GetIberDepotInstance();
            SdkFunctions = new SdkFunctions();

        }

        private void ReRoutePrinter(int impresora)
        {
            try
            {
                IIberPrinter iberPrinter = AlohaSdkFactory.GetIberPrinterInstance();
                string info = iberPrinter.GetAllPrinters();
                iberPrinter.PrintStream($"<PRINT><PRINTER>{impresora}</PRINTER><COMMANDS><PRINTLINE>Hello XML World my name is ~*IberXML_KEYWORD EmpNickname ID*~</PRINTLINE><RED>1</RED><PRINTLEFTRIGHT><LEFT>Hello</LEFT><RIGHT>World</RIGHT></PRINTLEFTRIGHT><RED>0</RED><PRINTFILLED>*</PRINTFILLED><LINEFEED>3</LINEFEED><CUT>PARTIAL</CUT></COMMANDS></PRINT>");
            }
            catch (Exception ex)
            {

            }
        }
        //FUNCIONES DE ENCOLAMIENTO DE UN SOLO IBER

        private void LogoutInterno(int Idterm)
        {
            try
            {
                xFunction.LogOut(Idterm);
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al LOGOUT interno{ex.Message}");
            }
        }

        private void LoginInterno(int IdTerm, int IdEmpleado)
        {
            try
            {
                xFunction.LogIn(IdTerm, IdEmpleado, "", "");

            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al LOGIN interno{ex.Message}");
            }
        }


        //ACCIONES PARA APLICACION DE ESCRITORIO

        public BdInterna GetUsersInSession()
        {
            return App.bdInterna;
        }

        public ResponseDesktop ReleaseUser(int IdEmpleado)
        {
            ResponseDesktop responseDesktop = new ResponseDesktop();
            try
            {
                User UserInSesion = App.bdInterna.users.Find(u => u.IdEmpleado == IdEmpleado);
                if (UserInSesion != null)
                {
                    App.bdInterna.users.Remove(UserInSesion);
                    responseDesktop.Codigo = (int)CodigosError.NO_ERROR;
                    responseDesktop.Mensaje = "Usuario liberado correctamente";
                }
                else
                {
                    responseDesktop.Codigo = 0;
                    responseDesktop.Mensaje = "El usuario no se encuentra bloqueado";
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error en la liberacion del empleado {IdEmpleado}", ex);
                responseDesktop.Codigo = (int)CodigosError.ERROR;
                responseDesktop.Mensaje = $"Error durante la liberacion del usuario con id {IdEmpleado}";
            }

            return responseDesktop;
        }
    }
}
