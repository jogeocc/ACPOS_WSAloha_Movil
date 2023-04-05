using Aloha.SDK.Common;
using AlohaLibrary.Contexto;
using AlohaLibrary.Implementaciones;
using AlohaLibrary.Modelos;
using AlohaWebServiceMobile.Aloha;
using AlohaWebServiceMobile.CodigosErrorAloha;
using AlohaWebServiceMobile.Controllers;
using AlohaWebServiceMobile.EntityFrameWork.Context;
using AlohaWebServiceMobile.EntityFrameWork.Models;
using AlohaWebServiceMobile.Enums;
using AlohaWebServiceMobile.Models;
using AlohaWebServiceMobile.Models.Aloha;
using AlohaWebServiceMobile.Models.Aloha.BlueTooth;
using AlohaWebServiceMobile.Models.Aloha.Desktop;
using AlohaWebServiceMobile.Models.Aloha.System;
using AlohaWebServiceMobile.Models.Transacciones;
using LasaFOHLib;
using LecturaAppConfig;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TicketGenerateAloha;
using TicketGenerateAloha.Models;

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
                    Encolamiento();
                    App.IsBusy = true;

                    //string NumPassword = IdEmpleado.ToString();
                    //int digits = NumPassword.Length;
                    //int empleado = 0;
                    //string password = "";
                    //if (digits >= App.Aloha.MinNumLenghtEmployee)
                    //{
                    //    empleado = int.Parse(NumPassword.Substring(0, NumPassword.Length - App.Aloha.MinNumLenghtEmployee));
                    //    password = NumPassword.Substring(App.Aloha.MinNumLenghtEmployee);
                    //}
                    //else
                    //{
                    //    empleado = digits;
                    //    password = "";
                    //}

                    int IdSistema = xFunction.LogIn(IdTerm, IdEmpleado, "", "");
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
            App.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha ClockIn(int IdTerm, int IdJobCode, int idEmpleado)
        {
            ResponseAloha response = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
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
            App.IsBusy = false;
            return response;
        }

        public ResponseAloha OpenTable(int IdTerm, int idNumMesa, string NombreMesa, int NumInvitados, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
                App.logger.Info($"Creando mesa {idNumMesa} de usuario {idEmpleado}");
                LoginInterno(IdTerm, idEmpleado);
                int IdMesaInterno = xFunction.AddTable(IdTerm, 0, idNumMesa, NombreMesa, NumInvitados);
                responseAloha.idMesa = IdMesaInterno;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Mesa abierta con exito";
                responseAloha.NombreMesa = GetTabTableName(IdMesaInterno);
                int idChequeInterno = xFunction.AddCheck(IdTerm, IdMesaInterno);
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Estado = false;
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.mensaje = $"Error abriendo mesa {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error al abrir mesa id = {idNumMesa}", ex);
                LogoutInterno(IdTerm);
            }
            App.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha OpenTab(int IdTerm, int idNumMesa, string NombreMesa, int NumInvitados, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                //Para abrir un tab, por defecto debe de ser el numero de mesa en 0
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
                App.logger.Info($"Creando mesa {idNumMesa} de usuario {idEmpleado}");
                LoginInterno(IdTerm, idEmpleado);
                int IdMesaInterno = xFunction.AddTable(IdTerm, 0, idNumMesa, NombreMesa, NumInvitados);
                responseAloha.idMesa = IdMesaInterno;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Cuenta abierta";
                responseAloha.NombreMesa = GetTabTableName(IdMesaInterno);
                int idChequeInterno = xFunction.AddCheck(IdTerm, IdMesaInterno);
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error abriendo mesa-cuenta {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error abriendo mesa-cuenta id = {idNumMesa}", ex);
                LogoutInterno(IdTerm);
            }
            App.IsBusy = false;
            return responseAloha;
        }

        /// <summary>
        /// Para crear una nueva orden enviar el <paramref name="IdMesaInterno"/> con valor cero.
        /// 
        /// </summary>
        /// <param name="IdTerm"></param>
        /// <param name="IdMesaInterno"></param>
        /// <param name="idEmpleado"></param>
        /// <returns></returns>
        public ResponseAloha OpenCheck(int IdTerm, int IdMesaInterno, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();

            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
                //LoginInterno(IdTerm, idEmpleado);
                var Mesas = RecuperarMesas(idEmpleado);
                if (Mesas.Exists(M => M.Id == IdMesaInterno))
                {
                    var Mesa = Mesas.Find(M => M.Id == IdMesaInterno);
                    var id = Mesa.Checks.First().Id;
                    responseAloha.idMesa = id;
                }
                else
                {
                    App.logger.Info($"CHEQUE NO ENCONTRADO, CREANDO NUEVO CHEQUE");
                    responseAloha.idMesa = xFunction.AddCheck(IdTerm, IdMesaInterno);
                }

                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = $"Cuenta abierda con id {responseAloha.idMesa}";
                //responseAloha.idMesa = idChequeInterno;
                //LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al abrir cheque {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error($"Error al abrir cheque", ex);
                LogoutInterno(IdTerm);
            }
            App.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha CloseCheck(int IdTerm, int IdCheckInterno, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
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
            App.IsBusy = false;
            return responseAloha;

        }

        public ResponseAloha CloseTabTable(int IdTerm, int IdMesaInterno, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
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
            App.IsBusy = false;
            return responseAloha;
        }

        public ResponsePrinter PrintBluetooth(int idCheck, int idMesa, int idTerm)
        {
            //TODO PENSAR EN UN REEMPLAZO A FUTURO
            ResponsePrinter response = new ResponsePrinter();
            VerificarIber();
            Check cheque = RecuperarCheque(idCheck);
            MesaEmpleado mesa = RecuperarMesa(idMesa);

            DetallePedido detallePedido = GetDetallePedidoTicket(cheque, mesa, idTerm);

            response.ticket_precuenta = new Ticket(@"Design\config-ticket.txt", detallePedido);

            foreach (var pie in response.ticket_precuenta.pie)
            {
                foreach (var columna in pie.columnas)
                {
                    double propina10 = double.Parse(detallePedido.Total.ToString()) * .10;
                    double propina15 = double.Parse(detallePedido.Total.ToString()) * .15;
                    double propina20 = double.Parse(detallePedido.Total.ToString()) * .20;
                    columna.valor = columna.valor.Replace("[[10]]", $"{string.Format("{0:C2}", propina10)}");
                    columna.valor = columna.valor.Replace("[[15]]", $"{string.Format("{0:C2}", propina15)}");
                    columna.valor = columna.valor.Replace("[[20]]", $"{string.Format("{0:C2}", propina20)}");
                }
            }
            response.mensaje = "OK";
            return response;
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



        //VERSION 
        public ResponseAloha AddItems(RequestAddItem requestAddItem)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
                LoginInterno(requestAddItem.IdTerm, requestAddItem.IdEmpleado);
                List<int> IdsEntryes = new List<int>();
                foreach (ItemAloha item in requestAddItem.item)
                {
                    int IdEntryBase = xFunction.BeginItem(requestAddItem.IdTerm, requestAddItem.IdCheck, item.IdItem, "", item.Amount);
                    IdsEntryes.Add(IdEntryBase);
                    #region modificadores
                    //int NivelMod = 1;
                    List<int> EntrysLevels = new List<int>();
                    for (int i = 0; i < item.Mods.Count; i++)
                    {
                        ListsMods mod = item.Mods[i];

                        ListsMods modSiguientes = new ListsMods();
                        if (i == item.Mods.Count - 1)
                        {

                        }
                        else
                        {
                            modSiguientes = item.Mods[i + 1];
                        }

                        if (mod.LevelMode > 1)
                        {
                            if (mod.LevelMode < modSiguientes.LevelMode)
                            {
                                EntrysLevels.Add(xFunction.ModItemEx(requestAddItem.IdTerm, EntrysLevels[EntrysLevels.Count - 1], mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode));
                            }
                            else if (mod.LevelMode == modSiguientes.LevelMode)
                            {
                                xFunction.ModItemEx(requestAddItem.IdTerm, EntrysLevels[EntrysLevels.Count - 1], mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode);
                            }
                            else
                            {
                                xFunction.ModItemEx(requestAddItem.IdTerm, EntrysLevels[EntrysLevels.Count - 1], mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode);
                            }
                        }
                        else
                        {
                            EntrysLevels = new List<int>();
                            EntrysLevels.Add(xFunction.ModItemEx(requestAddItem.IdTerm, IdEntryBase, mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode));
                        }
                    }
                    #endregion
                    xFunction.EndItem(requestAddItem.IdTerm);
                    if (!string.IsNullOrEmpty(item.SpecialMessage) || !string.IsNullOrEmpty(item.Unidad_Medida))
                    {
                        string Mensaje = "";

                        Mensaje += item.Cantidad_Peso > 0 ? item.Cantidad_Peso.ToString() : "";

                        Mensaje += !string.IsNullOrEmpty(item.Unidad_Medida) ? item.Unidad_Medida : "";

                        Mensaje += !string.IsNullOrEmpty(item.SpecialMessage) ? $" {item.SpecialMessage}" : "";

                        xFunction.ApplySpecialMessage(requestAddItem.IdTerm, requestAddItem.IdCheck, IdEntryBase, Mensaje);
                    }
                }

                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Producto insertado con exito";
                responseAloha.check = RecuperarCheque(requestAddItem.IdCheck, IdsEntryes);

                LogoutInterno(requestAddItem.IdTerm);
                App.IsBusy = false;
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al agregar item {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error("Error al agregar item", ex);
                LogoutInterno(requestAddItem.IdTerm);
                App.IsBusy = false;
            };
            App.IsBusy = false;
            return responseAloha;
        }



        public ResponseAloha AddItemNivelesPruebas()
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                int idterm = 4;
                int IdCheck = 1048579;
                //nivel 0
                int IdEntryBase = xFunction.BeginItem(idterm, IdCheck, 9104, "", 0);
                var info = GetEntry(idterm);
                //nivel 1
                xFunction.ModItemEx(idterm, IdEntryBase, 16011, 2124, "", 111, 0);
                //nivel 1
                xFunction.ModItemEx(idterm, IdEntryBase, 10007, 16001, "", 111, 0);
                //nivel 1
                int IdNivel1 = xFunction.ModItemEx(idterm, IdEntryBase, 10001, 19004, "", 111, 0);
                //nivel 2
                int IdNivel2 = xFunction.ModItemEx(idterm, IdNivel1, 16011, 2123, "", 222, 0);
                //nivel 3
                int IdNivel3 = xFunction.ModItemEx(idterm, IdNivel2, 16002, 2059, "", 333, 0);

                //TRANSFERIR EL BUFFER DE MEMORIA AL POS PARA REFLEJAR PRODUCTO
                xFunction.EndItem(idterm);
                responseAloha.Estado = true;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Producto insertado con exito";
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al agregar item {(ErroresAloha.MensajeMobile(ex.Message))}";
            }
            return responseAloha;
        }

        public int GetEntry(int IdTerm)
        {
            int entry = 0;
            try
            {
                var iber = depot.FindObjectFromId((int)COMEnums.INTERNAL_LOCALSTATE, IdTerm).First();

            }
            catch
            {

            }
            return entry;
        }

        public ResponseAloha AddItem(RequestAddItem requestAddItem)
        {
            ResponseAloha responseAloha = new ResponseAloha();

            try
            {

                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
                LoginInterno(requestAddItem.IdTerm, requestAddItem.IdEmpleado);

                int idEntry = xFunction.BeginItem(requestAddItem.IdTerm, requestAddItem.IdCheck, requestAddItem._item.IdItem, "", requestAddItem._item.Amount);
                #region modificadores
                foreach (var mod in requestAddItem._item.Mods)
                {
                    xFunction.ModItem(requestAddItem.IdTerm, idEntry, mod.IdMod, "", mod.Amount, mod.ModCode);
                }
                #endregion
                xFunction.EndItem(requestAddItem.IdTerm);
                if (!string.IsNullOrEmpty(requestAddItem._item.SpecialMessage) || !string.IsNullOrEmpty(requestAddItem._item.Unidad_Medida))
                {
                    string Mensaje = "";

                    Mensaje += requestAddItem._item.Cantidad_Peso > 0 ? requestAddItem._item.Cantidad_Peso.ToString() : "";

                    Mensaje += !string.IsNullOrEmpty(requestAddItem._item.Unidad_Medida) ? requestAddItem._item.Unidad_Medida : "";

                    Mensaje += !string.IsNullOrEmpty(requestAddItem._item.SpecialMessage) ? $" {requestAddItem._item.SpecialMessage}" : "";

                    xFunction.ApplySpecialMessage(requestAddItem.IdTerm, requestAddItem.IdCheck, idEntry, Mensaje);
                }
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Producto insertado con exito";
                LogoutInterno(requestAddItem.IdTerm);
                App.IsBusy = false;
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al agregar item {(ErroresAloha.MensajeMobile(ex.Message))}";
                App.logger.Error("Error al agregar item", ex);
                LogoutInterno(requestAddItem.IdTerm);
                App.IsBusy = false;
            };
            return responseAloha;
        }

        public ResponseAloha ConfirmOrderMode(int IdTerm, int IdMesa, int IdModoPedido, int idEmpleado, List<EntryesMode> selectedEntries)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
                LoginInterno(IdTerm, idEmpleado);

                if (selectedEntries.Count > 0)
                {
                    foreach (var entry in selectedEntries)
                    {
                        xFunction.SelectEntryAndChildren(IdTerm, IdMesa, entry.EntrieId);
                    }
                }




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
            App.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha AplicarPago(int idEmpleado, int IdTerm, int IdCheckId, int IdTender, double Amount, double Tip, string Digitos = "", string Expiration = "", string Info = "", string authorization = "")
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
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
            App.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha EliminarPago(int IdTerm, int IdCheckId, int IdPayment, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
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
            App.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha Print(int idTerm, int idCheck, int IdEmpleado, int idTermImpresora)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
                LoginInterno(idTerm, IdEmpleado);

                #region Seccion que cambia el ruteo de impresoras
                xFunction.SetObjectAttribute((int)COMEnums.INTERNAL_CHECKS, idCheck, "ID_RUTEO", idTermImpresora.ToString());
                xFunction.PrintCheck(idTerm, idCheck);
                #endregion

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
            App.IsBusy = false;
            return responseAloha;
        }


        public ResponseAloha VoidItem(int idTerm, int idEmpleado, int idCheck, List<ItemAnulado> itemAnulados, int idVoidReason)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try

            {
                VerificarIber();
                Encolamiento();
                App.IsBusy = true;
                LoginInterno(idTerm, idEmpleado);
                foreach (var itemAnulado in itemAnulados)
                {
                    xFunction.VoidItem(idTerm, idCheck, itemAnulado.IdEntry, idVoidReason);
                }
                LogoutInterno(idTerm);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Producto eliminado con éxito";
                responseAloha.Estado = true;
            }
            catch (Exception ex)
            {
                responseAloha.Estado = false;
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.mensaje = $"Error al elimiar producto, {ErroresAloha.MensajeMobile(ex.Message)}";
                App.logger.Error("Error al elimiar producto", ex);
                LogoutInterno(idTerm);

            }
            App.IsBusy = false;
            return responseAloha;
        }

        public void ProcesarEOD()
        {
            try
            {
                App.logger.Info($"LIMPIANDO A TODOS LOS USUARIOS DEL SISTEMA");
                App.bdInterna.users.Clear();
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al liberar empleados del sistema", ex);
            }
        }

        public void DividirCuentas(RequestDividirCuenta requestDividirCuenta)
        {
            try
            {
                VerificarIber();
                LoginInterno(requestDividirCuenta.IdTerm, requestDividirCuenta.IdEmpleado);
                foreach (CheckOpen Cuentas in requestDividirCuenta.cheksOpen)
                {
                    int CheckId = xFunction.AddCheck(requestDividirCuenta.IdTerm, requestDividirCuenta.IdTable);
                    foreach (var entry in Cuentas.ListIdEntrys)
                    {
                        xFunction.SelectEntryAndChildren(requestDividirCuenta.IdTerm, requestDividirCuenta.IdCheck, entry);
                    }
                    xFunction.MoveSelectedEntries(requestDividirCuenta.IdTerm, requestDividirCuenta.IdManager, requestDividirCuenta.IdCheck, CheckId);
                }
                xFunction.DeselectAllEntries(requestDividirCuenta.IdTerm);
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al dividir cuentas", ex);
            }
            LogoutInterno(requestDividirCuenta.IdTerm);
        }



        public void CombineTables(RequestCombineTables requestCombineTables)
        {
            try
            {
                VerificarIber();
                LoginInterno(requestCombineTables.IdTerm, requestCombineTables.IdEmpleado);
                //OBTENER TODOS LOS ENTRYES A MOVER DE LA MESA 1
                List<ModelCombineTables> MesaChequesTransferir = GetTableAndChecks(requestCombineTables.IdTableOne, requestCombineTables.IdTerm);
                List<ModelCombineTables> MesaDestino = GetTableAndChecks(requestCombineTables.IdTableTwo, requestCombineTables.IdTerm);

                //TODO CORREGIR ESTOS CASOS
                //MOVER TODOS LOS ENTRYES RECUPERADOS A LA MESA DESTINO O LA MESA 2 POR DEFECTO
                foreach (var cheque in MesaChequesTransferir)
                {
                    foreach (var entry in cheque.ListIdEntrys)
                    {
                        xFunction.SelectEntryAndChildren(requestCombineTables.IdTerm, cheque.IdCheck, entry);
                    }
                    xFunction.MoveSelectedEntries(requestCombineTables.IdTerm, requestCombineTables.IdEmpleado, cheque.IdCheck, MesaDestino[0].IdCheck);
                }

            }
            catch (Exception ex)
            {

                App.logger.Error($"ERROR AL COMBINAR MESAS DEL SISTEMA", ex);
            }
            LogoutInterno(requestCombineTables.IdTerm);
        }
        public void SendCloseCheckSAP(RequestCloseCheckSAP requestCloseCheckSAP)
        {

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
        private MesaEmpleado RecuperarMesa(int idMesa)
        {
            MesaEmpleado mesaEmpleado = new MesaEmpleado();

            try
            {
                var mesa = depot.FindObjectFromId((int)COMEnums.INTERNAL_TABLES, idMesa).First();
                mesaEmpleado.Guests = mesa.GetLongVal("NUM_GUESTS");
                mesaEmpleado.Name = mesa.GetStringVal("NAME");
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al recuperar mesa");
            }

            return mesaEmpleado;
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
                double SubTotal = 0;
                double tax = 0;
                xFunction.GetCheckTotal(IdCheck, out SubTotal, out tax);
                check.Amount = SubTotal;
                check.Tax = tax;
                double MontoTotal = ChequeAbierto.GetDoubleVal("SUBTOTAL");

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
                        item.OrderMode = ItemAbierto.GetLongVal("MODE");
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
                check.AmountDue = MontoTotal - AmountPayed;

                check.Guests = ChequeAbierto.GetLongVal("GUESTS");
                check.ChceckNumber = SdkFunctions.GetCheckNumberFromCheckId(check.Id);
                check.TotalCheck = ChequeAbierto.GetDoubleVal("SUBTOTAL");

            }
            catch (Exception ex)
            {
                check = null;
            }
            return check;
        }

        private Check RecuperarCheque(int IdCheck, List<int> IdsEntrysInsertados)
        {
            Check check = new Check();
            try
            {
                check.Id = IdCheck;
                IberObject ChequeAbierto = depot.FindObjectFromId((int)COMEnums.INTERNAL_CHECKS, IdCheck).First();
                //ITEMS
                double SubTotal = 0;
                double tax = 0;
                xFunction.GetCheckTotal(IdCheck, out SubTotal, out tax);
                check.Amount = SubTotal;
                check.Tax = tax;
                double MontoTotal = ChequeAbierto.GetDoubleVal("SUBTOTAL");

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
                        item.OrderMode = ItemAbierto.GetLongVal("MODE");
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
                check.AmountDue = MontoTotal - AmountPayed;

                check.Guests = ChequeAbierto.GetLongVal("GUESTS");
                check.ChceckNumber = SdkFunctions.GetCheckNumberFromCheckId(check.Id);
                check.TotalCheck = ChequeAbierto.GetDoubleVal("SUBTOTAL");
                if (IdsEntrysInsertados.Count > 0)
                {
                    List<Item> NewItemList = new List<Item>();

                    check.Items.ForEach((Item) =>
                    {
                        foreach (var id in IdsEntrysInsertados)
                        {
                            if (Item.IdEntry == id)
                            {
                                NewItemList.Add(Item);
                            }
                        }
                    });

                    check.Items = NewItemList;
                }
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

        private DetallePedido GetDetallePedidoTicket(Check check, MesaEmpleado mesa, int idTerm)
        {
            DetallePedido detallePedido = new DetallePedido();
            try
            {
                detallePedido.Fecha = DateTime.Now;
                detallePedido.Invitados = mesa.Guests;
                detallePedido.NombreTerminal = GetPosName(idTerm);
                detallePedido.Mesa = mesa.Name;
                detallePedido.Articulos = new List<Articulo>();

                foreach (var item in check.Items)
                {
                    var articulo = new Articulo
                    {
                        Nombre = item.Name,
                        Importe = decimal.Parse(item.Price.ToString()),
                    };

                    articulo.SubArticulos = new List<Articulo>();
                    foreach (var mod in item.Mods)
                    {
                        articulo.SubArticulos.Add(new Articulo
                        {
                            Nombre = mod.Name.Trim(),
                            Importe = decimal.Parse(mod.Price.ToString())
                        });

                    }
                    detallePedido.Articulos.Add(articulo);
                }

                detallePedido.NumeroOrden = check.ChceckNumber;
                detallePedido.Subtotal = decimal.Parse(check.Amount.ToString());
                detallePedido.Impuestos = new List<Impuesto> { new Impuesto { Importe = decimal.Parse(check.Tax.ToString()) } };
                detallePedido.Total = decimal.Parse(check.TotalCheck.ToString());
                detallePedido.TotalItems = detallePedido.Articulos.Count;

            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al recuperar detalle de ticket", ex);
            }

            return detallePedido;
        }

        private string GetPosName(int idTerm)
        {
            string PosName = "";
            try
            {
                var term = depot.FindObjectFromId((int)COMEnums.INTERNAL_TERMINALS, idTerm).First();

            }
            catch (Exception ex)
            {

            }
            return PosName;
        }

        private List<ModelCombineTables> GetTableAndChecks(int idTableTab, int IdTerm)
        {
            List<ModelCombineTables> List = new List<ModelCombineTables>();
            try
            {
                IberEnum EnumMesasChecks = depot.FindObjectFromId((int)COMEnums.INTERNAL_TABLES_OPEN_CHECKS, idTableTab);
                IberObject Cheque = EnumMesasChecks.First();
                ModelCombineTables modelCombineTables = new ModelCombineTables();
                int CheckId = Cheque.GetLongVal("ID");
                modelCombineTables.IdCheck = CheckId;

                App.logger.Info($"mesas del empleado {EnumMesasChecks.Count}");
                for (int i = 0; i < EnumMesasChecks.Count; i++)
                {
                    ICheckEntryNewEx[] Entryes = xFunction.GetCheckEntriesNewEx(IdTerm, CheckId);
                    foreach (ICheckEntryNewEx entry in Entryes)
                    {
                        modelCombineTables.ListIdEntrys.Add(entry.entryIdEx);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"ERROR NO SE PUDO RECUPERAR CHEQUES DE LA MESA", ex);
            }

            return List;
        }


        public void RegistrarVariableALOHA(RequestCloseCheckSAP requestCloseCheckSAP)
        {
            try
            {
                VerificarIber();
                xFunction.SetObjectAttribute((int)COMEnums.INTERNAL_CHECKS, requestCloseCheckSAP.CheckId, "SAP", ((int)ActivadorSAP.ENVIAR_SAP).ToString());
                //xFunction.PrintCheck(idTerm, requestCloseCheckSAP.CheckId);
            }
            catch (Exception ex)
            {
                App.logger.Error($"ERROR REGISTRAR VARIABLE DE ALOHA EN SISTEMA", ex);
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


        private void Encolamiento()
        {
            while (true)
            {
                if (!App.IsBusy)
                {
                    break;
                }
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



        static RequestPagoPendiente temporal = new RequestPagoPendiente();

        // FUNCIONES PARA GUARADR Y CONSUTLAR PAGOS PENDIENTES DE INTEGRACION SMARTPAYMENTS
        public ResponseAloha GuardarPagoPendiente(Pagos_pendientes requestPagoPendiente)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            bool IsUpdadated;
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    if (requestPagoPendiente.id == 0)
                    {
                        var data = db.Pagos_pendientes.Add(requestPagoPendiente);
                        db.SaveChanges();
                        responseAloha.IdPagoPendiente = data.id;
                        responseAloha.SG_REFERENCE = data.SG_REFERENCE;
                    }
                    else
                    {
                        Pagos_pendientes PagoPendiente = db.Pagos_pendientes.ToList().FindLast(PP => PP.id == requestPagoPendiente.id);
                        PagoPendiente.infoPago = requestPagoPendiente.infoPago;
                        responseAloha.IdPagoPendiente = PagoPendiente.id;
                        responseAloha.SG_REFERENCE = PagoPendiente.SG_REFERENCE;
                        db.SaveChanges();

                    }

                }

                responseAloha.Estado = true;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Datos de pago pendiente guardados en BD con exito";
            }
            catch (Exception ex)
            {

            }
            return responseAloha;

        }

        public Pagos_pendientes RecuperarPagoPendiente(int Idempleado)
        {
            Pagos_pendientes pendiente = new Pagos_pendientes();
            try
            {

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    string fecha = DateTime.Now.Date.ToString("ddMMyyyy");
                    var pendientes = db.Pagos_pendientes.ToList();

                    pendiente = pendientes.FindLast(S => S.IdEmpleado == Idempleado && S.infoPago == 1 && S.Fecha == fecha);
                }
            }
            catch (Exception ex)
            {

            }

            return pendiente;
        }

        public Ticket_smart SaveTicketSmart(Ticket_smart requestPagoPendiente)
        {
            Ticket_smart ResponsePagoPendiente = new Ticket_smart();
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var data = db.Ticket_smart.Add(requestPagoPendiente);
                    db.SaveChanges();
                    ResponsePagoPendiente = data;
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"ERROR AL GUARDAR TICKET SMART PARA REIMPRESION", ex);
            }
            return ResponsePagoPendiente;
        }

        public Ticket_smart GETTicketSmart(int idEmpleado, int idCheck)
        {
            Ticket_smart ResponsePagosPendiente = new Ticket_smart();
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    string fecha = DateTime.Now.Date.ToString("ddMMyyyy");
                    var tickets = db.Ticket_smart.ToList();
                    ResponsePagosPendiente = tickets.First(P => P.CheckID == idCheck && P.IdEmpleado == idEmpleado && P.Fecha == fecha);
                }
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al obtener el ticket para reimpresion", ex);
            }
            return ResponsePagosPendiente;
        }







        public bool printXML(string XML, int CheckId)
        {
            int IdServer = LACSystem.GetInt("ID_TERM_SERVER");
            bool IsSuccess = false;
            try
            {
                VerificarIber();
                IIberPrinter iberPrinter = AlohaSdkFactory.GetIberPrinterInstance();

                xFunction.GetCheckTotal(CheckId, out double subtotal, out double tax);
                double total = subtotal + tax;



                List<string> Eventos = GetEvents(AlohaEvents.FOOTERMSGBYTERMINAL);
                List<EventsAloha> models = MakeModels(Eventos);
                EventsAloha eventoImpresion = GetPrinterEvent(models, IdServer);
                List<string> LineasMensajes = GetMsgs((eventoImpresion.TypeAlohaEvent as FOOTERMSGBYTERMINAL).IdGci);

                List<string> LineasAgregar = new List<string>();
                if (EventIsValid(IdServer, eventoImpresion))
                {
                    foreach (var linea in LineasMensajes.Where(L => L.Length > 0))
                    {
                        if (linea.Contains('%'))
                        {
                            double PorcentajePropina = ExtractCharact(linea);
                            string cadenaPropina = MensajePropina(linea);

                            string NuevaCadenaPropina = $"{cadenaPropina} {string.Format("{0:C2}", PorcentajePropina * total)}";

                            LineasAgregar.Add(NuevaCadenaPropina);
                        }
                        else
                        {
                            LineasAgregar.Add(linea);
                        }
                    }



                    foreach (var linea in LineasAgregar)
                    {
                        XML = AddTextAtFinal(XML, linea);
                    }




                    iberPrinter.PrintStream(XML);
                    App.logger.Info($"Iniciado proceso de impresión");
                }

            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al imprimir", ex);
            }
            return IsSuccess;
        }


        public bool EventIsValid(int IdServer, EventsAloha evento)
        {

            bool IsValid = (evento.TypeAlohaEvent as FOOTERMSGBYTERMINAL).IdTerminal == IdServer;
            return IsValid;
        }

        public List<EventsAloha> MakeModels(List<string> Lineas)
        {
            List<EventsAloha> models = new List<EventsAloha>();

            foreach (string line in Lineas)
            {
                List<string> Valores = line.Split(' ').ToList();
                EventsAloha evento = new EventsAloha
                {
                    HOUR = TimeSpan.ParseExact(Valores[0], @"h\:mm", null),
                    NameEvent = Valores[1],
                    TypeAlohaEvent = new FOOTERMSGBYTERMINAL
                    {
                        IdTerminal = int.Parse(Valores[2]),
                        IdGci = int.Parse(Valores[3])
                    }
                };
                models.Add(evento);
            }


            return models;
        }

        public EventsAloha GetPrinterEvent(List<EventsAloha> Eventos, int IdServer)
        {
            EventsAloha Event = null;


            Event = Eventos.First(E => (E.TypeAlohaEvent as FOOTERMSGBYTERMINAL).IdTerminal == IdServer);


            return Event;

        }

        public List<string> GetEvents(AlohaEvents tipo)
        {
            string Ruta = AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder();
            List<string> EventosAloha = File.ReadLines(Path.Combine(Ruta, AlohaFilename.EventosAloha)).ToList();
            EventosAloha = EventosAloha.FindAll(L => L.Contains(tipo.ToString()));
            return EventosAloha;
        }

        public List<string> GetMsgs(int idMensaje)
        {
            List<string> Msgs = new List<string>();
            string Ruta = AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder();
            List<GCI> GCI = new List<GCI>();
            using (AplicacionBdContextoALH contextoALH = new AplicacionBdContextoALH(Ruta))
            {
                GCI = new GCIServicio(contextoALH).GetAll();
            }
            foreach (GCI gci in GCI.Where(M => M.ID == idMensaje))
            {
                Msgs.Add(gci.MESSAGE1);
                Msgs.Add(gci.MESSAGE2);
                Msgs.Add(gci.MESSAGE3);
                Msgs.Add(gci.MESSAGE4);
                Msgs.Add(gci.MESSAGE5);
                Msgs.Add(gci.MESSAGE6);
                Msgs.Add(gci.MESSAGE7);
                Msgs.Add(gci.MESSAGE8);
                Msgs.Add(gci.MESSAGE9);
                Msgs.Add(gci.MESSAGE10);
                Msgs.Add(gci.MESSAGE11);
                Msgs.Add(gci.MESSAGE12);
            }
            return Msgs;
        }


        public double ExtractCharact(string Cadena)
        {
            double GetNum = 0.0;
            string Num;

            Match StrNum = Regex.Match(Cadena, @"\*\d+.\d{1,2}\)");
            if (StrNum.Success)
            {
                Num = StrNum.Value.Replace(")", "").Replace("*", "");
                GetNum = Convert.ToDouble(Num);
            }

            return GetNum;
        }

        public string MensajePropina(string MensajeAloha)
        {
            Match Mensaje = Regex.Match(MensajeAloha, @"[\w*\s]*]*\d*%");
            return Mensaje.Success ? Mensaje.Value : "";
        }

        public string AddTextAtFinal(string originalXml, string textLine)
        {
            string newXml = originalXml;
            try
            {
                string printLine = "<STOPJOURNAL/>";
                int pos = newXml.Trim().LastIndexOf(printLine);
                if (pos == -1)
                {
                    printLine = "<STOPJOURNAL />";
                    pos = newXml.LastIndexOf(printLine);
                }
                StringBuilder temp = new StringBuilder();
                temp.Append(String.Format("<PRINTCENTERED>{0}</PRINTCENTERED>", textLine));
                newXml = newXml.Insert(pos, temp.ToString());
            }
            catch
            {
                return originalXml;
            }
            return newXml;
        }


    }
}
