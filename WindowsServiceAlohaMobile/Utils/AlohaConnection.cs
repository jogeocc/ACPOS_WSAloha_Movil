using Aloha.SDK.Common;
using AlohaLibrary.Contexto;
using AlohaLibrary.Implementaciones;
using AlohaLibrary.Modelos;
using LasaFOHLib;
using LecturaAppConfig;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Windows;
using TicketGenerateAloha;
using TicketGenerateAloha.Models;
using WindowsServiceAlohaMobile.Aloha;
using WindowsServiceAlohaMobile.AlohaExtractInfo;
using WindowsServiceAlohaMobile.CodigosErrorAloha;
using WindowsServiceAlohaMobile.EntityFrameWork.Context;
using WindowsServiceAlohaMobile.EntityFrameWork.Models;
using WindowsServiceAlohaMobile.Enums;
using WindowsServiceAlohaMobile.Models.Aloha;
using WindowsServiceAlohaMobile.Models.Aloha.BlueTooh;
using WindowsServiceAlohaMobile.Models.Aloha.Desktop;
using WindowsServiceAlohaMobile.Models.Aloha.SAP;
using WindowsServiceAlohaMobile.Models.Aloha.System;
using WindowsServiceAlohaMobile.Models.Aloha.Transacciones;

namespace WindowsServiceAlohaMobile.Utils
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




                User UserInSesion = ACPOS_SERVICE_MOBILE.bdInterna.users.Find(u => u.IdEmpleado == IdEmpleado);

                if (UserInSesion == null)
                {
                    
                    VerificarIber();
                    Encolamiento();
                    LiberaTerminalApagada(IdEmpleado, IdTerm);
                    ACPOS_SERVICE_MOBILE.IsBusy = true;

                    //string NumPassword = IdEmpleado.ToString();
                    //int digits = NumPassword.Length;
                    //int empleado = 0;
                    //string password = "";
                    //if (digits >= Service1.Aloha.MinNumLenghtEmployee)
                    //{
                    //    empleado = int.Parse(NumPassword.Substring(0, NumPassword.Length - Service1.Aloha.MinNumLenghtEmployee));
                    //    password = NumPassword.Substring(Service1.Aloha.MinNumLenghtEmployee);
                    //}
                    //else
                    //{
                    //    empleado = digits;
                    //    password = "";
                    //}

                    string Password = "";
                    if (IdEmpleado >= ACPOS_SERVICE_MOBILE.AlohaIni.MinNumLenghtEmployee)
                    {
                        ACPOS_SERVICE_MOBILE.logger.Info($"DIGITOS EMP:{ACPOS_SERVICE_MOBILE.AlohaIni.MinNumLenghtEmployee}");
                        string DataComplete = IdEmpleado.ToString();
                        string emp = DataComplete.Substring(0, ACPOS_SERVICE_MOBILE.AlohaIni.MinNumLenghtEmployee);
                        IdEmpleado = int.Parse(emp);
                        Password = DataComplete.Substring(ACPOS_SERVICE_MOBILE.AlohaIni.MinNumLenghtEmployee);
                    }

                    int IdSistema = xFunction.LogIn(IdTerm, IdEmpleado, Password, "");
                    responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                    responseAloha.IdEmpleadoSistema = IdSistema;
                    responseAloha.isClockIn = IsAlreadyClockIn(IdSistema);
                    responseAloha.mensaje = "Login realizado con exito";
                    responseAloha.Nombre_Empleado = NombreEmpleado(IdSistema);
                    responseAloha.idJobs = IdsJobsEmpleado(IdSistema);
                    responseAloha.mesas_empleado = RecuperarMesas(IdSistema, IdTerm);
                    LogoutInterno(IdTerm);
                    ACPOS_SERVICE_MOBILE.bdInterna.users.Add(new User
                    {
                        IdEmpleado = IdEmpleado,
                        UserName = NombreEmpleado(IdEmpleado)
                    });
                    xFunction.SetObjectAttribute((int)COMEnums.INTERNAL_EMPLOYEES, IdEmpleado, IdEmpleado.ToString(), "SI");
                }
                else
                {
                    responseAloha.mensaje = $"Usuario ya en sesion";
                    responseAloha.Codigo = (int)CodigosError.ERROR;
                    ACPOS_SERVICE_MOBILE.logger.Error($"El usuario ya esta en sesion");
                }

            }
            catch (Exception ex)
            {
                responseAloha.mensaje = $"Error al intentar ingresar con el usuario {IdEmpleado} - {(ErroresAloha.MensajeMobile(ex.Message))}";
                ACPOS_SERVICE_MOBILE.logger.Error("Error al ingresar con el usuario tal", ex);
                LogoutInterno(IdTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;
        }

      
        public ResponseAloha ClockIn(int IdTerm, int IdJobCode, int idEmpleado)
        {
            ResponseAloha response = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                ACPOS_SERVICE_MOBILE.IsBusy = true;
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
                ACPOS_SERVICE_MOBILE.logger.Error("Error al intentar registrarse con el usuario", ex);
                LogoutInterno(IdTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return response;
        }

        public ResponseAloha OpenTable(int IdTerm, int idNumMesa, string NombreMesa, int NumInvitados, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                ACPOS_SERVICE_MOBILE.IsBusy = true;
                ACPOS_SERVICE_MOBILE.logger.Info($"Creando mesa {idNumMesa} de usuario {idEmpleado}");
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
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al abrir mesa id = {idNumMesa}", ex);
                LogoutInterno(IdTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
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
                ACPOS_SERVICE_MOBILE.IsBusy = true;
                ACPOS_SERVICE_MOBILE.logger.Info($"Creando mesa {idNumMesa} de usuario {idEmpleado}");
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
                ACPOS_SERVICE_MOBILE.logger.Error($"Error abriendo mesa-cuenta id = {idNumMesa}", ex);
                LogoutInterno(IdTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
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
        public ResponseAloha OpenCheck(int IdTerm, int IdMesaInterno, int idEmpleado, bool isNewCheck)
        {
            ResponseAloha responseAloha = new ResponseAloha();

            try
            {
                VerificarIber();
                Encolamiento();
                ACPOS_SERVICE_MOBILE.IsBusy = true;
                LoginInterno(IdTerm, idEmpleado);
                var Mesas = RecuperarMesas(idEmpleado, IdTerm);
                if (Mesas.Exists(M => M.Id == IdMesaInterno))
                {
                    var Mesa = Mesas.Find(M => M.Id == IdMesaInterno);
                    var id = Mesa.Checks.First().Id;
                    if (isNewCheck)
                    {
                        id = xFunction.AddCheck(IdTerm, IdMesaInterno);
                        responseAloha.NumCheck = RecuperarCheque(id).NumCheck;
                    }
                    responseAloha.idMesa = id;
                    responseAloha.Mesa = Mesa;
                }
                else
                {
                    ACPOS_SERVICE_MOBILE.logger.Info($"CHEQUE NO ENCONTRADO, CREANDO NUEVO CHEQUE");
                    responseAloha.idMesa = xFunction.AddCheck(IdTerm, IdMesaInterno);
                }

                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = $"Cuenta abierda con id {responseAloha.idMesa}";
                //responseAloha.idMesa = idChequeInterno;
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al abrir cheque {(ErroresAloha.MensajeMobile(ex.Message))}";
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al abrir cheque", ex);
                LogoutInterno(IdTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha CloseCheck(int IdTerm, int IdCheckInterno, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                ACPOS_SERVICE_MOBILE.IsBusy = true;
                VerificarIber();
                LoginInterno(IdTerm, idEmpleado);
                xFunction.CloseCheck(IdTerm, IdCheckInterno);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = $"Cuenta Cerrada con id {IdCheckInterno}";
                LogoutInterno(IdTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al cerrar cheque {(ErroresAloha.MensajeMobile(ex.Message))}";
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al cerrar cheque", ex);
                LogoutInterno(IdTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;

        }

        public ResponseAloha CloseTabTable(int IdTerm, int IdMesaInterno, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                ACPOS_SERVICE_MOBILE.IsBusy = true;
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
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al cerrar mesa", ex);
                LogoutInterno(IdTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;
        }

        public ResponsePrinter PrintBluetooth(int idCheck, int idMesa, int idTerm, int idEmpleado)
        {
            //TODO PENSAR EN UN REEMPLAZO A FUTURO
            ResponsePrinter response = new ResponsePrinter();
            VerificarIber();
            Check cheque = RecuperarCheque(idCheck);
            MesaEmpleado mesa = RecuperarMesa(idMesa);

            DetallePedido detallePedido = GetDetallePedidoTicket(cheque, mesa, idTerm, idMesa, idEmpleado);

            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Design\config-ticket.txt");


            response.ticket_precuenta = new Ticket(ruta, detallePedido);

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
                User UserInSesion = ACPOS_SERVICE_MOBILE.bdInterna.users.Find(u => u.IdEmpleado == idEmpleado);
                if (UserInSesion != null)
                {
                    ACPOS_SERVICE_MOBILE.bdInterna.users.Remove(UserInSesion);
                }
                response.mensaje = "Salida realizada con éxito";

                response.Estado = true;
                xFunction.LogOut(IdTerm);
                xFunction.SetObjectAttribute((int)COMEnums.INTERNAL_EMPLOYEES, idEmpleado, idEmpleado.ToString(), "NO");
            }
            catch (Exception ex)
            {
                response.Codigo = (int)CodigosError.ERROR;
                response.Estado = false;
                response.mensaje = $"Error al salir de terminal {(ErroresAloha.MensajeMobile(ex.Message))}";
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al salir de la terminal {IdTerm}", ex);
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
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al hacer salida {IdTerm}", ex);
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
                ACPOS_SERVICE_MOBILE.IsBusy = true;
                LoginInterno(requestAddItem.IdTerm, requestAddItem.IdEmpleado);
                List<int> IdsEntryes = new List<int>();
                foreach (ItemAloha item in requestAddItem.item)
                {               

                    var NameItem = ACPOS_SERVICE_MOBILE.itemsAskDesc.Any(itm => itm.ID == item.IdItem) ? item.DescItem : "";

                    int IdEntryBase = 0;

                    if (item.NumSilla > 0)
                    {
                        IdEntryBase = xFunction.BeginPivotSeatItem(requestAddItem.IdTerm, requestAddItem.IdCheck, item.IdItem, NameItem, item.Amount, item.NumSilla);
                    }
                    else
                    {
                        IdEntryBase = xFunction.BeginItem(requestAddItem.IdTerm, requestAddItem.IdCheck, item.IdItem, NameItem, item.Amount);
                    }

                    IdsEntryes.Add(IdEntryBase);
                    #region modificadores
                    //int NivelMod = 1;
                    List<int> EntrysLevels = new List<int>();

                    foreach (var mod in item.Mods)
                    {
                        RecursividadModificadores(requestAddItem.IdTerm, mod, IdEntryBase);
                    }

                    #region Antigua Version

                    //for (int i = 0; i < item.Mods.Count; i++)
                    //{
                    //    ListsMods mod = item.Mods[i];

                    //    ListsMods modSiguientes = new ListsMods();
                    //    if (i == item.Mods.Count - 1)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        modSiguientes = item.Mods[i + 1];
                    //    }

                    //    if (mod.LevelMode > 1)
                    //    {
                    //        if (mod.LevelMode < modSiguientes.LevelMode)
                    //        {
                    //            EntrysLevels.Add(xFunction.ModItemEx(requestAddItem.IdTerm, EntrysLevels[EntrysLevels.Count - 1], mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode));
                    //        }
                    //        else if (mod.LevelMode == modSiguientes.LevelMode)
                    //        {
                    //            xFunction.ModItemEx(requestAddItem.IdTerm, EntrysLevels[EntrysLevels.Count - 1], mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode);
                    //        }
                    //        else
                    //        {
                    //            xFunction.ModItemEx(requestAddItem.IdTerm, EntrysLevels[EntrysLevels.Count - 1], mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        EntrysLevels = new List<int>();
                    //        EntrysLevels.Add(xFunction.ModItemEx(requestAddItem.IdTerm, IdEntryBase, mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode));
                    //    }
                    //}

                    #endregion

                    #endregion
                    //xFunction.
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
                ACPOS_SERVICE_MOBILE.IsBusy = false;
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al agregar item {(ErroresAloha.MensajeMobile(ex.Message))}";
                ACPOS_SERVICE_MOBILE.logger.Error("Error al agregar item", ex);
                LogoutInterno(requestAddItem.IdTerm);
                ACPOS_SERVICE_MOBILE.IsBusy = false;
            };
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;
        }

        public void RecursividadModificadores(int Idterm, Mod Modificador, int idEntryBase)
        {

            var NameItem = ACPOS_SERVICE_MOBILE.itemsAskDesc.Any(itm => itm.ID == Modificador.IdMod) ? Modificador.DescName : "";

            int IdEntry = xFunction.ModItemEx(Idterm, idEntryBase, Modificador.IdGrupo, Modificador.IdMod, NameItem, Modificador.Amount, Modificador.ModCode);

            if (Modificador.Mods.Count > 0)
            {
                foreach (Mod mod in Modificador.Mods)
                {
                    RecursividadModificadores(Idterm, mod, IdEntry);

                }

            }

        }

        public ResponseAloha AddItemNivelesPruebas()
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                int idterm = 1;
                int IdCheck = 1048579;
                //nivel 0
                int IdEntryBase = xFunction.BeginItem(idterm, IdCheck, 209, "", 0);
                xFunction.ModItemEx(idterm, IdEntryBase, 10012, 5138, "", 111, 0);
                xFunction.ModItemEx(idterm, IdEntryBase, 10001, 5003, "", 111, 0);


                var info = GetEntry(idterm);
                //nivel 1
                var Mod1 = xFunction.ModItemEx(idterm, IdEntryBase, 10072, 108, "", 111, 0);

                for (int i = 0; i < 5; i++)
                {

                    Mod1 = xFunction.ModItemEx(idterm, Mod1, 10000, 108, "", 111, 0);
                }






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


        #region Version vieja
        //public ResponseAloha AddItem(RequestAddItem requestAddItem)
        //{
        //    ResponseAloha responseAloha = new ResponseAloha();

        //    try
        //    {

        //        VerificarIber();
        //        Encolamiento();
        //        Service1.IsBusy = true;
        //        LoginInterno(requestAddItem.IdTerm, requestAddItem.IdEmpleado);

        //        int idEntry = xFunction.BeginItem(requestAddItem.IdTerm, requestAddItem.IdCheck, requestAddItem._item.IdItem, "", requestAddItem._item.Amount);
        //        #region modificadores
        //        foreach (var mod in requestAddItem._item.Mods)
        //        {
        //            xFunction.ModItem(requestAddItem.IdTerm, idEntry, mod.IdMod, "", mod.Amount, mod.ModCode);
        //        }
        //        #endregion
        //        xFunction.EndItem(requestAddItem.IdTerm);
        //        if (!string.IsNullOrEmpty(requestAddItem._item.SpecialMessage) || !string.IsNullOrEmpty(requestAddItem._item.Unidad_Medida))
        //        {
        //            string Mensaje = "";

        //            Mensaje += requestAddItem._item.Cantidad_Peso > 0 ? requestAddItem._item.Cantidad_Peso.ToString() : "";

        //            Mensaje += !string.IsNullOrEmpty(requestAddItem._item.Unidad_Medida) ? requestAddItem._item.Unidad_Medida : "";

        //            Mensaje += !string.IsNullOrEmpty(requestAddItem._item.SpecialMessage) ? $" {requestAddItem._item.SpecialMessage}" : "";

        //            xFunction.Service1lySpecialMessage(requestAddItem.IdTerm, requestAddItem.IdCheck, idEntry, Mensaje);
        //        }
        //        responseAloha.Codigo = (int)CodigosError.NO_ERROR;
        //        responseAloha.mensaje = "Producto insertado con exito";
        //        LogoutInterno(requestAddItem.IdTerm);
        //        Service1.IsBusy = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        responseAloha.Codigo = (int)CodigosError.ERROR;
        //        responseAloha.Estado = false;
        //        responseAloha.mensaje = $"Error al agregar item {(ErroresAloha.MensajeMobile(ex.Message))}";
        //        Service1.logger.Error("Error al agregar item", ex);
        //        LogoutInterno(requestAddItem.IdTerm);
        //        Service1.IsBusy = false;
        //    };
        //    return responseAloha;
        //}

        #endregion


        public ResponseAloha ConfirmOrderMode(int IdTerm, int IdMesa, int IdModoPedido, int idEmpleado, List<EntryesMode> selectedEntries, int idCheck)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                ACPOS_SERVICE_MOBILE.IsBusy = true;
                LoginInterno(IdTerm, idEmpleado);
                bool isSelectedEntryes = false;
                if (selectedEntries.Count > 0)
                {
                    foreach (var entry in selectedEntries)
                    {
                        xFunction.DeselectAllEntries(IdTerm);
                        xFunction.SelectEntryAndChildren(IdTerm, idCheck, entry.EntrieId);
                        isSelectedEntryes = true;
                    }
                }
                xFunction.OrderItems(IdTerm, IdMesa, IdModoPedido);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Productos ordenados con exito";
                LogoutInterno(IdTerm);
                xFunction.DeselectAllEntries(IdTerm);
                if (isSelectedEntryes)
                {
                    ACPOS_SERVICE_MOBILE.DbManager.UpdateProductosEnEspera(selectedEntries, IdMesa, IdTerm);
                }
            }
            catch (Exception ex)
            {
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.Estado = false;
                responseAloha.mensaje = $"Error al confirmar pedido {(ErroresAloha.MensajeMobile(ex.Message))}";
                ACPOS_SERVICE_MOBILE.logger.Error("Error al confirmar pedido", ex);

                LogoutInterno(IdTerm);
            };
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha AplicarPago(int idEmpleado, int IdTerm, int IdCheckId, int IdTender, double Amount, double Tip, string Digitos = "", string Expiration = "", string Info = "", string authorization = "")
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                ACPOS_SERVICE_MOBILE.IsBusy = true;
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
                ACPOS_SERVICE_MOBILE.logger.Error("Error al aplicar pago", ex);
                LogoutInterno(IdTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha EliminarPago(int IdTerm, int IdCheckId, int IdPayment, int idEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                VerificarIber();
                Encolamiento();
                ACPOS_SERVICE_MOBILE.IsBusy = true;
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
                ACPOS_SERVICE_MOBILE.logger.Error("Error al eliminar pago", ex);
                LogoutInterno(IdTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;
        }

        public ResponseAloha Print(int idTerm, int idCheck, int IdEmpleado, int idTermImpresora)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                ACPOS_SERVICE_MOBILE.logger.Info($"[Print] Iniciando impresión desde tableta ID: {idTerm} hacia impresora ID: {idTermImpresora}");

                ACPOS_SERVICE_MOBILE.logger.Info("[Print] Verificando conexión con Iber...");
                VerificarIber();

                ACPOS_SERVICE_MOBILE.logger.Info("[Print] Encolando proceso...");
                Encolamiento();

                ACPOS_SERVICE_MOBILE.IsBusy = true;

                ACPOS_SERVICE_MOBILE.logger.Info($"[Print] Realizando login interno con terminal ID: {idTerm} y empleado ID: {IdEmpleado}");
                LoginInterno(idTerm, IdEmpleado);

                #region Sección que cambia el ruteo de impresoras
                ACPOS_SERVICE_MOBILE.logger.Info($"[Print] Cambiando ID_RUTEO del cheque {idCheck} a {idTermImpresora}");
                xFunction.SetObjectAttribute((int)COMEnums.INTERNAL_CHECKS, idCheck, "ID_RUTEO", idTermImpresora.ToString());

                ACPOS_SERVICE_MOBILE.logger.Info($"[Print] Enviando impresión del cheque {idCheck} desde la terminal {idTerm}");
                xFunction.PrintCheck(idTerm, idCheck);
                #endregion

                responseAloha.Estado = true;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Enviando tarea de impresión";
                ACPOS_SERVICE_MOBILE.logger.Info($"[Print] Impresión enviada correctamente. Cheque ID: {idCheck}");

                ACPOS_SERVICE_MOBILE.logger.Info($"[Print] Cerrando sesión interna para terminal ID: {idTerm}");
                LogoutInterno(idTerm);
            }
            catch (Exception ex)
            {
                responseAloha.Estado = false;
                responseAloha.Codigo = (int)CodigosError.ERROR;
                responseAloha.mensaje = $"Error al imprimir,{ErroresAloha.MensajeMobile(ex.Message)}";
                ACPOS_SERVICE_MOBILE.logger.Error("Error al imprimir", ex);
                LogoutInterno(idTerm);
            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;
        }


        public ResponseAloha VoidItem(int idTerm, int idEmpleado, int idCheck, List<ItemAnulado> itemAnulados, int idVoidReason)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try

            {
                VerificarIber();
                Encolamiento();
                ACPOS_SERVICE_MOBILE.IsBusy = true;
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
                ACPOS_SERVICE_MOBILE.logger.Error("Error al elimiar producto", ex);
                LogoutInterno(idTerm);

            }
            ACPOS_SERVICE_MOBILE.IsBusy = false;
            return responseAloha;
        }

        public void ProcesarEOD()
        {
            try
            {
                ACPOS_SERVICE_MOBILE.logger.Info($"LIMPIANDO A TODOS LOS USUARIOS DEL SISTEMA");
                ACPOS_SERVICE_MOBILE.bdInterna.users.Clear();
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al liberar empleados del sistema", ex);
            }
        }

        public void DividirCuentas(RequestDividirCuenta requestDividirCuenta)
        {
            int iteracion = 0;

            try
            {
                VerificarIber();
                LoginInterno(requestDividirCuenta.IdTerm, requestDividirCuenta.IdEmpleado);



                xFunction.DeselectAllEntries(requestDividirCuenta.IdTerm);
                foreach (CheckOpen Cuenta in requestDividirCuenta.cheksOpen)
                {
                    if (Cuenta.IdCheckDestino == Cuenta.IdCheckOrigen) continue;
                    //var checs = RecuperarCheque(Cuenta.IdCheckOrigen);
                    bool IsHold = IsEntryInHold(requestDividirCuenta.IdTerm, Cuenta.IdCheckOrigen, Cuenta.IdEntry);
                    xFunction.SelectEntryAndChildren(requestDividirCuenta.IdTerm, Cuenta.IdCheckOrigen, Cuenta.IdEntry);
                    xFunction.MoveSelectedEntries(requestDividirCuenta.IdTerm, requestDividirCuenta.IdManager, Cuenta.IdCheckOrigen, Cuenta.IdCheckDestino);
                    xFunction.DeselectAllEntries(requestDividirCuenta.IdTerm);
                    if (IsHold)
                    {
                        ACPOS_SERVICE_MOBILE.DbManager.UpdateProductosEnEspera(Cuenta.IdCheckOrigen, Cuenta.IdCheckDestino, Cuenta.IdEntry);
                    }
                    iteracion++;
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al dividir cuentas", ex);

            }
            //var xchecs = RecuperarCheque(1048586);

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

                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL COMBINAR MESAS DEL SISTEMA", ex);
            }
            LogoutInterno(requestCombineTables.IdTerm);
        }
        public void PrintTicketSap(RequestPrintCheckSap requestCloseCheckSAP)
        {
            ACPOS_SERVICE_MOBILE.logger.Info($"POR ENVIAR INFO HACIA SAP");
            ACPOS_SERVICE_MOBILE.logger.Info($"{JsonConvert.SerializeObject(requestCloseCheckSAP)}");
            ACPOS_SERVICE_MOBILE.logger.Info($"RECUPERANDO CENTRO DE CONSUMO DE: {requestCloseCheckSAP.CheckId}");
            int IdRev = recuperarIdRev(requestCloseCheckSAP.CheckId);
            ACPOS_SERVICE_MOBILE.logger.Info($"CENTRO DE CONSUMO RECUPERADO: {IdRev}");
            ACPOS_SERVICE_MOBILE.restSAP.SendXmlSAP(requestCloseCheckSAP.SAP_XML, IdRev);

        }

        public void GetPagosTicketSap(int checkId)
        {
            List<DetallePago> detallePago = new ExtraccionCuenta().MonitoreoCuenta(checkId);
            ACPOS_SERVICE_MOBILE.restSAP.SendPagosSocioSap(detallePago);

        }

        /// <summary>
        /// La funcion realiza el agregar los items y ademas ponerlos en la tabla de espera para mantenerlos en espera.
        /// </summary>
        /// <param name="idEmpleado"></param>
        /// <param name="idTerm"></param>
        /// <param name="idCheck"></param>
        /// <param name="selectedEntries"></param>
        /// <param name="time"></param>
        public void SetHoldItemsSelected(RequestHoldCheck requestHoldCheck)
        {
            try
            {
                VerificarIber();
                LoginInterno(requestHoldCheck.IdTerm, requestHoldCheck.IdEmpleado);
                List<int> IdsEntryes = new List<int>();
                foreach (ItemAloha item in requestHoldCheck.item)
                {

                    int IdEntryBase = 0;

                    var NameItem = ACPOS_SERVICE_MOBILE.itemsAskDesc.Any(itm => itm.ID == item.IdItem) ? item.DescItem : "";


                    if (item.NumSilla > 0)
                    {
                        IdEntryBase = xFunction.BeginPivotSeatItem(requestHoldCheck.IdTerm, requestHoldCheck.IdCheck, item.IdItem, NameItem, item.Amount, item.NumSilla);
                    }
                    else
                    {
                        IdEntryBase = xFunction.BeginItem(requestHoldCheck.IdTerm, requestHoldCheck.IdCheck, item.IdItem, NameItem, item.Amount);
                    }
                    IdsEntryes.Add(IdEntryBase);
                    #region SECCION DE MODIFICADORES.


                    foreach (var mod in item.Mods)
                    {
                        RecursividadModificadores(requestHoldCheck.IdTerm, mod, IdEntryBase);
                    }

                    #region Version vieja


                    //List<int> EntrysLevels = new List<int>();

                    //for (int i = 0; i < item.Mods.Count; i++)
                    //{



                    //    ListsMods mod = item.Mods[i];

                    //    ListsMods modSiguientes = new ListsMods();
                    //    if (i == item.Mods.Count - 1)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        modSiguientes = item.Mods[i + 1];
                    //    }

                    //    if (mod.LevelMode > 1)
                    //    {
                    //        if (mod.LevelMode < modSiguientes.LevelMode)
                    //        {
                    //            EntrysLevels.Add(xFunction.ModItemEx(requestHoldCheck.IdTerm, EntrysLevels[EntrysLevels.Count - 1], mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode));
                    //        }
                    //        else if (mod.LevelMode == modSiguientes.LevelMode)
                    //        {
                    //            xFunction.ModItemEx(requestHoldCheck.IdTerm, EntrysLevels[EntrysLevels.Count - 1], mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode);
                    //        }
                    //        else
                    //        {
                    //            xFunction.ModItemEx(requestHoldCheck.IdTerm, EntrysLevels[EntrysLevels.Count - 1], mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        EntrysLevels = new List<int>();
                    //        EntrysLevels.Add(xFunction.ModItemEx(requestHoldCheck.IdTerm, IdEntryBase, mod.IdGrupo, mod.IdMod, "", mod.Amount, mod.ModCode));
                    //    }
                    //}


                    #endregion


                    #endregion
                    xFunction.EndItem(requestHoldCheck.IdTerm);
                    if (!string.IsNullOrEmpty(item.SpecialMessage) || !string.IsNullOrEmpty(item.Unidad_Medida))
                    {
                        string Mensaje = "";
                        Mensaje += item.Cantidad_Peso > 0 ? item.Cantidad_Peso.ToString() : "";
                        Mensaje += !string.IsNullOrEmpty(item.Unidad_Medida) ? item.Unidad_Medida : "";
                        Mensaje += !string.IsNullOrEmpty(item.SpecialMessage) ? $" {item.SpecialMessage}" : "";
                        xFunction.ApplySpecialMessage(requestHoldCheck.IdTerm, requestHoldCheck.IdCheck, IdEntryBase, Mensaje);
                    }
                    item.IdEntry = IdEntryBase;
                    #region HOLD ENTRY
                    xFunction.DeselectAllEntries(requestHoldCheck.IdTerm);
                    xFunction.SelectEntryAndChildren(requestHoldCheck.IdTerm, requestHoldCheck.IdCheck, IdEntryBase);
                    //1 => es igual a true, 0 => es igual a false
                    xFunction.HoldUnorderedEntriesOnCheck(requestHoldCheck.IdTerm, requestHoldCheck.IdCheck, 1);
                    xFunction.DeselectAllEntries(requestHoldCheck.IdTerm);
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL COLOCAR PRODUCTOS EN HOLD", ex);
            }
            #region GUARDAR EN BD
            ACPOS_SERVICE_MOBILE.DbManager.AddProductoEspera(requestHoldCheck);
            #endregion
            LogoutInterno(requestHoldCheck.IdTerm);

        }
        //FUNCIONES DE CONTROL DE DATOS

        public ResponseAloha ListTables(int IdEmpleado, int idTerm)
        {
            VerificarIber();
            ResponseAloha responseAloha = new ResponseAloha();
            responseAloha.Codigo = (int)CodigosError.NO_ERROR;
            responseAloha.mensaje = "Mesas recuperadas con exito";
            responseAloha.Nombre_Empleado = NombreEmpleado(IdEmpleado);
            responseAloha.mesas_empleado = RecuperarMesas(IdEmpleado, idTerm);
            return responseAloha;
        }

        private List<MesaEmpleado> RecuperarMesas(int IdEmpleado, int idTerm)
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
                    mesaEmpleado.Revenue = MesaAbierta.GetLongVal("REV_ID");
                    mesaEmpleado.IsTable = MesaAbierta.GetBoolVal("TYPE") == 0 ? false : true;
                    mesaEmpleado.IdMesa = MesaAbierta.GetLongVal("TABLEDEF_ID");
                    mesaEmpleado.NumSeats = MesaAbierta.GetLongVal("NUM_SEATS");
                    mesaEmpleado.NumSeats = mesaEmpleado.NumSeats - 1;
                    if (mesaEmpleado.NumSeats < 0)
                    {
                        mesaEmpleado.NumSeats = 0;

                    }
                    mesaEmpleado.IsHold = IsAnyCheckHold(mesaEmpleado.Id, idTerm);
                    mesaEmpleado.IsHoldVERSION2 = mesaEmpleado.IsHold ? "SI" : "NO";
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
                            check.NumCheck = ChequeAbierto.GetLongVal($"NUMBER") + 1;

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
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar mesa", ex);
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
                check.IdRev = ChequeAbierto.GetLongVal("REV_ID");
                int TableId = ChequeAbierto.GetLongVal("TABLE_ID");
                IberObject table = depot.FindObjectFromId((int)COMEnums.INTERNAL_TABLES, TableId).First();
                check.NumSeats = table.GetLongVal("NUM_SEATS") - 1;

                if (check.NumSeats < 0)
                {
                    check.NumSeats = 0;

                }
                //check.NumSilas = GetSillas();
                double MontoTotal = ChequeAbierto.GetDoubleVal("SUBTOTAL");
                //ITEMS DEL CHEQUE
                List<Producto_Pedido_Espera> ListaPedidos = ACPOS_SERVICE_MOBILE.DbManager.GetProductosTiempoEspera(IdCheck);
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
                        foreach (var ProductoEspera in ListaPedidos)
                        {
                            if (item.IdEntry == ProductoEspera.IdEntry)
                            {
                                if (item.Ordered)
                                {
                                    //TODO AGREGAR NUEVOS DATOS AL SISTEMA
                                    //Service1.DbManager.UpdateProductosEnEspera();
                                }
                                else
                                {
                                    item.HoldTime = ProductoEspera.HoldEnd.ToString("HH:mm:ss");
                                    item.HoldOrderMode = ProductoEspera.IdOrderMode;
                                }

                                break;
                            }
                        }
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
                        item.NumSilla = ItemAbierto.GetLongVal("SEAT");
                        ItemAbierto = ItemsEmpleado.Next();
                    }

                }
                catch (Exception ex)
                {
                   // ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar items {IdCheck}", ex);
                }
                //PAGOS APLICADOS A LA MESA
                try
                {
                    IberEnum PagosEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_PAYMENTS);
                    IberObject PagoAplicado = PagosEmpleado.First();
                    EstructurarData estructurarData = new EstructurarData();
                    while (PagoAplicado != null)
                    {


                        Payment payment = new Payment();
                        payment.IdPayment = PagoAplicado.GetLongVal("ID");
                        payment.IdTender = PagoAplicado.GetLongVal("TENDER_ID");
                        payment.Tip = PagoAplicado.GetDoubleVal("TIP");
                        payment.Amount = PagoAplicado.GetDoubleVal("AMOUNT");
                        payment.LabelPayment = estructurarData.NombreTender(PagoAplicado.GetLongVal("TENDER_ID")) + "_" + PagoAplicado.GetStringVal("IDENT");
                        check.Payments.Add(payment);
                        PagoAplicado = PagosEmpleado.Next();
                    }

                }
                catch (Exception ex)
                {
                   // ACPOS_SERVICE_MOBILE.logger.Error($"Error al obtener pagos aplicados en la cuenta {IdCheck}",ex);
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
                    //ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar Promociones cuenta: {IdCheck}", ex);
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
                  //  ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar Cortesias cuenta: {IdCheck}", ex);
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


                check.AmountDue = ChequeAbierto.GetDoubleVal("COMPLETETOTAL") - AmountPayed;
                //CAMBIO PARA EL CLUB DE GOLF
                    check.AmountDue = MontoTotal - AmountPayed;
                    check.AmountDue = SubTotal - AmountPayed;
                //-----------------------------


                check.Guests = ChequeAbierto.GetLongVal("GUESTS");
                check.ChceckNumber = SdkFunctions.GetCheckNumberFromCheckId(check.Id);
                check.TotalCheck = ChequeAbierto.GetDoubleVal("COMPLETETOTAL");
                
                
                //CAMBIO PARA EL CLUB DE GOLF
                    check.TotalCheck = MontoTotal;
                    check.TotalCheck = SubTotal;
                //-----------------------------



                //se agrega un mas 1, ya que empieza a contar a partir del cero 0
                check.NumCheck = ChequeAbierto.GetLongVal($"NUMBER") + 1;

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar la cuenta: {IdCheck}", ex);
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
                    ACPOS_SERVICE_MOBILE.logger.Error($"Error al obtener pagos aplicados en la cuenta");
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
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar los jobs del empelado {IdEmpleado}", ex);
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
                ACPOS_SERVICE_MOBILE.logger.Error("Error recuperando nombre del empleado", ex);
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
                ACPOS_SERVICE_MOBILE.logger.Error("", ex);
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
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar nombre de la mesa", ex);
            }

            return Name;
        }

        private void VerificarIber()
        {
            xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
            depot = AlohaSdkFactory.GetIberDepotInstance();
            SdkFunctions = new SdkFunctions();

        }

        private DetallePedido GetDetallePedidoTicket(Check check, MesaEmpleado mesa, int idTerm, int idMesa, int idEmpleado)
        {
            DetallePedido detallePedido = new DetallePedido();
            try
            {
                detallePedido.Fecha = DateTime.Now;
                detallePedido.Invitados = mesa.Guests;
                detallePedido.NombreTerminal = GetEmployeeNameByEmpId(idEmpleado);
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
                        var dato = new Articulo
                        {
                            Nombre = mod.Name.Trim(),
                        };
                        if (mod.Price == 0)
                        {
                            dato.Importe = null;
                        }
                        else
                        {
                            dato.Importe = decimal.Parse(mod.Price.ToString());
                        }
                        articulo.SubArticulos.Add(dato);
                    }
                    detallePedido.Articulos.Add(articulo);
                }


                foreach (var pago in check.Payments)
                {
                    FormaPago formaPago = new FormaPago();


                    formaPago.Propina = (decimal)pago.Tip;
                    formaPago.Total = (decimal)pago.Amount;
                    formaPago.Nombre = pago.LabelPayment;
                    formaPago.MostrarTotal = false;
                    formaPago.MostrarPropina = false;
                    formaPago.Importe = (decimal)pago.Amount;


                    detallePedido.FormasPago.Add(formaPago);
                }


                detallePedido.NumeroOrden = check.ChceckNumber;
                detallePedido.Subtotal = decimal.Parse(check.Amount.ToString());
                detallePedido.Impuestos = new List<Impuesto> { new Impuesto { Importe = decimal.Parse(check.Tax.ToString()) } };
                detallePedido.Total = decimal.Parse(check.TotalCheck.ToString());
                detallePedido.TotalItems = detallePedido.Articulos.Count;

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar detalle de ticket", ex);
            }

            return detallePedido;
        }

        private string GetEmployeeNameByEmpId(int idEmpleado)
        {
            string PosName = "Mesero";
            try
            {
                IberObject IObjectEmployee = depot.FindObjectFromId((int)COMEnums.INTERNAL_EMPLOYEES, idEmpleado).First();
                string EmployeeNick = IObjectEmployee.GetStringVal("NICKNAME");
                PosName = $"{EmployeeNick}";
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL RECUPERAR NOMBRE DE MESERO", ex);
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

                ACPOS_SERVICE_MOBILE.logger.Info($"mesas del empleado {EnumMesasChecks.Count}");
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
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR NO SE PUDO RECUPERAR CHEQUES DE LA MESA", ex);
            }

            return List;
        }


        public void RegistrarVariableALOHA(RequestPrintCheckSap requestCloseCheckSAP)
        {
            try
            {
                VerificarIber();
                xFunction.PrintCheck(3, requestCloseCheckSAP.CheckId);

                xFunction.SetObjectAttribute((int)COMEnums.INTERNAL_CHECKS, requestCloseCheckSAP.CheckId, "SAP", ((int)ActivadorSAP.ENVIAR_SAP).ToString());
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR REGISTRAR VARIABLE DE ALOHA EN SISTEMA", ex);
            }
        }

        public bool IsAnyCheckHold(int IdTable, int idTerm)
        {
            bool isHold = false;
            List<int> ChecksInTable = GetChecksFromTable(IdTable);
            try
            {
                foreach (var IdCheck in ChecksInTable)
                {
                    var Entrys = xFunction.GetCheckEntriesNewEx(idTerm, IdCheck);

                    if (Entrys != null) {
                        foreach (var entry in Entrys)
                        {
                            if (entry.modeEx == (int)OrderModesAloha.HOLD)
                            {
                                isHold = true;
                                break;
                            }
                        }
                        if (isHold)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR FUNCION 34, RECUPERANDO ALGUN HOLD DE LOS CHEQUES DE LA MESA", ex);
            }
            return isHold;
        }

        public List<int> GetChecksFromTable(int IdTable)
        {
            List<int> ListaCheques = new List<int>();
            try
            {
                VerificarIber();
                IberObject Mesa = depot.FindObjectFromId((int)COMEnums.INTERNAL_TABLES, IdTable).First();
                var EnumCheques = Mesa.GetEnum((int)COMEnums.INTERNAL_TABLES_CHECKS);
                IberObject ChequeActual = EnumCheques.First();
                for (int i = 0; i < EnumCheques.Count; i++)
                {
                    ListaCheques.Add(ChequeActual.GetLongVal("ID"));
                    if (i < EnumCheques.Count - 1)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR OBTENIENDO LOS CHEQUES DE LA MESA", ex);
            }
            return ListaCheques;
        }

        public bool IsEntryInHold(int idTerm, int idCheckOrigen, int idEntry)
        {
            bool isHold = false;
            try
            {
                var items = xFunction.GetCheckEntriesNewEx(idTerm, idCheckOrigen);
                foreach (var item in items)
                {
                    if (item.modeEx == (int)OrderModesAloha.HOLD && item.entryIdEx == idEntry)
                    {
                        isHold = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL DETERMINAR QUE EL PRODUCTO ES HOLD", ex);
            }
            return isHold;
        }

        //FUNCIONES DE ENCOLAMIENTO DE UN SOLO IBER

        private void LogoutInterno(int Idterm)
        {
            //try
            //{
            //    xFunction.LogOut(Idterm);
            //}
            //catch (Exception ex)
            //{
            //    Service1.logger.Error($"Error al LOGOUT interno{ex.Message}");
            //}
        }
        private void LoginInternoHold(int IdTerm, int IdEmpleado)
        {
            try
            {
                xFunction.LogIn(IdTerm, IdEmpleado, "", "");

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al LOGIN interno{ex.Message}");
            }
        }
        private void LoginInterno(int IdTerm, int IdEmpleado)
        {
            //try
            //{
            //    xFunction.LogIn(IdTerm, IdEmpleado, "", "");

            //}
            //catch (Exception ex)
            //{
            //    Service1.logger.Error($"Error al LOGIN interno{ex.Message}");
            //}
        }


        private void Encolamiento()
        {
            //while (true)
            //{
            //    if (!Service1.IsBusy)
            //    {
            //        break;
            //    }
            //}
        }


        //ACCIONES PARA APLICACION DE ESCRITORIO

        public BdInterna GetUsersInSession()
        {
            return ACPOS_SERVICE_MOBILE.bdInterna;
        }

        public ResponseDesktop ReleaseUser(int IdEmpleado)
        {
            ResponseDesktop responseDesktop = new ResponseDesktop();
            try
            {
                User UserInSesion = ACPOS_SERVICE_MOBILE.bdInterna.users.Find(u => u.IdEmpleado == IdEmpleado);
                if (UserInSesion != null)
                {
                    ACPOS_SERVICE_MOBILE.bdInterna.users.Remove(UserInSesion);
                    xFunction.SetObjectAttribute((int)COMEnums.INTERNAL_EMPLOYEES, IdEmpleado, IdEmpleado.ToString(), "NO");
                    LiberaTerminalApagada(IdEmpleado);
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
                ACPOS_SERVICE_MOBILE.logger.Error($"Error en la liberacion del empleado {IdEmpleado}", ex);
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
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL GUARDAR TICKET SMART PARA REIMPRESION", ex);
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
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al obtener el ticket para reimpresion", ex);
            }
            return ResponsePagosPendiente;
        }




        //FUNCIONES DE IMPRESION INTELIGENTE CON EVENTOS DE ALOHA


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
                    ACPOS_SERVICE_MOBILE.logger.Info($"Iniciado proceso de impresión");
                }

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al imprimir", ex);
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

        //FUNCIONES DE IMAGENES
        public HttpResponseMessage RecuperarBMPLogoALoha()
        {
            byte[] bytes;
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                bytes = BmpManager.ObtenerFotoSocio($"{LACSystem.GetString("DIR_BMP_ALOHA")}");


                // Asignar los bytes de la imagen BMP al contenido de la respuesta
                response.Content = new ByteArrayContent(bytes);

                // Establecer el tipo de contenido y la longitud de la respuesta
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                response.Content.Headers.ContentLength = bytes.Length;

                // Devolver la respuesta HTTP

            }
            catch
            {
                return response;
            }
            return response;
        }

        //FUNCIONES DE PROCESOS EN SEGUNDO PLANO

        public void ProcesarProductosEnEspera()
        {
            VerificarIber();
            var lista = ACPOS_SERVICE_MOBILE.DbManager.GETProductosEnEspera();

            foreach (Producto_Pedido_Espera producto in lista)
            {
                try
                {
                    //var user = Service1.bdInterna.users.Find(u => u.IdEmpleado == producto.IdEmpleado);
                    //if (user == null)
                    //{
                    //}
                    LoginInternoHold(producto.IdTerminal, producto.IdEmpleado);

                    xFunction.DeselectAllEntries(producto.IdTerminal);
                    xFunction.SelectEntryAndChildren(producto.IdTerminal, producto.IdCheck, producto.IdEntry);
                    xFunction.OrderItems(producto.IdTerminal, producto.IdTable, producto.IdOrderMode);
                    xFunction.DeselectAllEntries(producto.IdTerminal);
                    producto.IsOrdered = 1;
                }
                catch (Exception ex)
                {
                    ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL ENVIAR PRODUCTO EN ESPERA A ORDENAR", ex);
                }
                LogoutInterno(producto.IdTerminal);
            }
            lista = lista.Where(P => P.IsOrdered == 1).ToList();
            if (lista.Count > 0)
            {
                ACPOS_SERVICE_MOBILE.DbManager.UpdateProductosEnEspera(lista);

            }
        }

        //FUNCION DE RECUPERAR ESTADO DE LA TERMINAL SOLICITADA.
        public void GetLocalState()
        {
            try
            {
                VerificarIber();
                var EnumTerminales = depot.GetEnum((int)COMEnums.INTERNAL_LOCALSTATE);
                var cantidad = EnumTerminales.Count;
                IberObject InstanciaTerminal = EnumTerminales.First();
                for (int i = 0; i < cantidad; i++)
                {
                    var terminal = InstanciaTerminal.GetLongVal("TERMINAL_NUM");
                    var terminalIdentificador = InstanciaTerminal.GetLongVal("TERMINAL_ID");
                    bool isloggedin = InstanciaTerminal.GetBoolVal("LOGGED_IN") == 1;
                    var Curr_Emp = InstanciaTerminal.GetEnum((int)COMEnums.INTERNAL_LOCALSTATE_CUR_EMP);
                    int idemp = InstanciaTerminal.GetLongVal("CURRENT_EMPLOYEE");
                    if (i < cantidad - 1)
                    {
                        InstanciaTerminal = EnumTerminales.Next();
                    }
                }
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR RECUPERANDO LOCALSTATE", ex);
            }
        }

        public ResponseAloha UpdatePayment(Pagos_pendientes requestPagoPendiente)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                bool IsSuccess = ACPOS_SERVICE_MOBILE.DbManager.UpdatePagoPendiente(requestPagoPendiente.id, requestPagoPendiente.EntryId, requestPagoPendiente.infoPago, requestPagoPendiente.TransactionNumber, requestPagoPendiente.TransactionAuth);
                if (IsSuccess)
                {
                    responseAloha.Estado = true;
                    responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                    responseAloha.mensaje = "REGISTRO VALIDO Y ACTUALIZADO";
                }
                else
                {
                    responseAloha.Estado = false;
                    responseAloha.Codigo = (int)CodigosError.ERROR;
                    responseAloha.mensaje = "REGISTRO INVALIDO, NO INSERTADO, NO EXISTE EN BD";
                }

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL ACTUALIZADO ENTRY ID", ex);
            }
            return responseAloha;
        }

        public ResponseAloha ValidarPagoPendiente(Pagos_pendientes requestPagoPendiente)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                Pagos_pendientes pagos_Pendientes = ACPOS_SERVICE_MOBILE.DbManager.ValidarPagoPendiente(requestPagoPendiente.EntryId);
                if (pagos_Pendientes != null)
                {
                    responseAloha.pago_Pendiente = pagos_Pendientes;
                    responseAloha.Estado = true;
                    responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                    responseAloha.mensaje = "REGISTRO VALIDO";
                }
                else
                {
                    responseAloha.pago_Pendiente = null;
                    responseAloha.Estado = false;
                    responseAloha.Codigo = (int)CodigosError.ERROR;
                    responseAloha.mensaje = "REGISTRO INVALIDO, NO EXISTE EN BD";
                }

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL VALIDAR PAGO PENDIENTE", ex);
            }
            return responseAloha;
        }
        public void LiberaTerminalApagada(int IdEmpleado, int IdTerm = 0)
        {
            try
            {
                if (IdTerm == 0)
                {
                    IIberDepot depot = AlohaSdkFactory.GetIberDepotInstance();
                    IberObject Empleado = depot.FindObjectFromId((int)COMEnums.INTERNAL_EMPLOYEES, IdEmpleado).First();
                    int IdTerminal = Empleado.GetLongVal($"LOGINTERMINAL");
                    if (IdTerm == IdTerminal)
                    {
                        //xFunction.LogOut(IdTerminal);
                    }
                    xFunction.LogOut(IdTerminal);

                }
                else
                {
                    IIberDepot depot = AlohaSdkFactory.GetIberDepotInstance();
                    IberObject Empleado = depot.FindObjectFromId((int)COMEnums.INTERNAL_EMPLOYEES, IdEmpleado).First();
                    int IdTerminal = Empleado.GetLongVal($"LOGINTERMINAL");
                    xFunction.LogOut(IdTerminal);

                }

            }
            catch (Exception ex)
            {

            }

        }

        //16-10-2023

        public List<int> TablesUsed()
        {
            List<int> List = new List<int>();
            try
            {

            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"", ex);
            }
            return List;
        }



        public int recuperarIdRev(int IdCheck) {

            int IdRev = 0;
            try
            {
                IberObject ChequeAbierto = depot.FindObjectFromId((int)COMEnums.INTERNAL_CHECKS, IdCheck).First();
                IdRev = ChequeAbierto.GetLongVal("REV_ID");
            }
            catch (Exception ex) {
                ACPOS_SERVICE_MOBILE.logger.Error($"", ex);

            }


            return IdRev;

        }


#region

        public void DivisionPrueba()
        {
            VerificarIber();
            var xxxFunction = AlohaSdkFactory.GetIberFuncs28Instance();
        }

#endregion



        public ResponseAloha LockTable(int idTerm, int idCheck)
        {
            VerificarIber();
            ResponseAloha responseAloha = new ResponseAloha();
            responseAloha.Estado = BloquearMesa(idTerm, idCheck);

            if (responseAloha.Estado)
            {
                responseAloha.mensaje = "Mesa Bloqueada correctamente";
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
            }
            else
            {
                responseAloha.mensaje = "Error al bloquear la mesa";
                responseAloha.Codigo = (int)CodigosError.ERROR;
            }
            return responseAloha;
        }


        public ResponseAloha UnLockTable(int idTerm, int idCheck)
        {
            VerificarIber();
            ResponseAloha responseAloha = new ResponseAloha();
            responseAloha.Estado = DesbloquearMesa(idTerm, idCheck);

            if (responseAloha.Estado)
            {
                responseAloha.mensaje = "Mesa Desbloqueada correctamente";
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
            }
            else
            {
                responseAloha.mensaje = "Error al desbloquear la mesa";
                responseAloha.Codigo = (int)CodigosError.ERROR;
            }
            return responseAloha;
        }


        public bool BloquearMesa(int IdTerm, int IdTable)
        {
            bool isLockTable = true;
            ACPOS_SERVICE_MOBILE.logger.Info($"BLOQUEAR LA MESA - TERM: {IdTerm} MESA: {IdTable}");
            try
            {

                xFunction.LockTable(IdTerm, IdTable);
            }
            catch (Exception ex)
            {
                isLockTable = false;
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL BLOQUEAR LA MESA", ex);
            }
            return isLockTable;
        }



        public bool DesbloquearMesa(int IdTerm, int IdTable)
        {

            bool isLockTable = true;
            ACPOS_SERVICE_MOBILE.logger.Info($"DESBLOQUEAR LA MESA - TERM: {IdTerm} MESA: {IdTable}");
            try
            {
                xFunction.UnlockTable(IdTerm, IdTable);
            }
            catch (Exception ex)
            {
                isLockTable = false;
                ACPOS_SERVICE_MOBILE.logger.Error($"ERROR AL DESBLOQUEAR LA MESA", ex);
            }
            return isLockTable;
        }


        public ResponseAloha GetCloseCheck(RequestCloseCheckSap requestCloseCheckSap)
        {
            VerificarIber();
            ResponseAloha responseAloha = new ResponseAloha();
            responseAloha.check = RecuperarChequeCerrado(requestCloseCheckSap);

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

        private Check RecuperarChequeCerrado(RequestCloseCheckSap requestCloseCheckSap)
        {
            Check check = new Check();
            try
            {
                check.Id = requestCloseCheckSap.CheckId;

                // 1. Obtener el empleado por ID
                IberEnum enumEmpleado = depot.FindObjectFromId((int)COMEnums.INTERNAL_EMPLOYEES, requestCloseCheckSap.EmployeeId);
                IberObject empleado = enumEmpleado.First();

                if (empleado == null)
                    throw new Exception("Empleado no encontrado con ID: " + requestCloseCheckSap.EmployeeId);

                // 2. Obtener el cheque cerrado desde el objeto del empleado
                IberEnum chequesCerradosEnum = depot.FindObjectFromId((int)COMEnums.INTERNAL_CHECKS, requestCloseCheckSap.CheckId);
                IberObject ChequeCerrado = chequesCerradosEnum.First();

                if (ChequeCerrado == null)
                    throw new Exception("Cheque cerrado no encontrado con ID: " + requestCloseCheckSap.CheckId);

                // 3. Obtener la mesa
                IberEnum MesasCerradasEnum = depot.FindObjectFromId((int)COMEnums.INTERNAL_TABLES, requestCloseCheckSap.TableId);
                IberObject MesaCerrada = MesasCerradasEnum.First();

               
                if (MesaCerrada == null)
                    throw new Exception("Mesa cerrada no encontrada con ID: " + requestCloseCheckSap.TableId);


                // Calcular totales
                double SubTotal = 0;
                double tax = 0;
                xFunction.GetCheckTotal(requestCloseCheckSap.CheckId, out SubTotal, out tax);
                check.Amount = SubTotal;
                check.Tax = tax;
                check.IdRev = ChequeCerrado.GetLongVal("REV_ID");

                check.NumMesa = MesaCerrada.GetLongVal("TABLEDEF_ID") + "";
                check.NumSeats = 0;
                //check.NumSeats = table.GetLongVal("NUM_SEATS") - 1;
                if (check.NumSeats < 0) check.NumSeats = 0;

                double MontoTotal = ChequeCerrado.GetDoubleVal("SUBTOTAL");

                // Obtener productos en espera
                List<Producto_Pedido_Espera> ListaPedidos = ACPOS_SERVICE_MOBILE.DbManager.GetProductosTiempoEspera(requestCloseCheckSap.CheckId);

                // ITEMS DEL CHEQUE
                try
                {
                    IberEnum ItemsEmpleado = ChequeCerrado.GetEnum((int)COMEnums.INTERNAL_CHECKS_ENTRIES);
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
                        item.NumSilla = ItemAbierto.GetLongVal("SEAT");

                        foreach (var ProductoEspera in ListaPedidos)
                        {
                            if (item.IdEntry == ProductoEspera.IdEntry)
                            {
                                if (item.Ordered)
                                {
                                    // Actualizar si es necesario
                                }
                                else
                                {
                                    item.HoldTime = ProductoEspera.HoldEnd.ToString("HH:mm:ss");
                                    item.HoldOrderMode = ProductoEspera.IdOrderMode;
                                }
                                break;
                            }
                        }

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
                   // ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar items del cheque {requestCloseCheckSap.CheckId}", ex);
                }

                // PAGOS APLICADOS
                try
                {
                    IberEnum PagosEmpleado = ChequeCerrado.GetEnum((int)COMEnums.INTERNAL_CHECKS_PAYMENTS);
                    IberObject PagoAplicado = PagosEmpleado.First();
                    EstructurarData estructurarData = new EstructurarData();

                    while (PagoAplicado != null)
                    {
                        Payment payment = new Payment();
                        payment.IdPayment = PagoAplicado.GetLongVal("ID");
                        payment.IdTender = PagoAplicado.GetLongVal("TENDER_ID");
                        payment.Tip = PagoAplicado.GetDoubleVal("TIP");
                        payment.Amount = PagoAplicado.GetDoubleVal("AMOUNT");
                        payment.LabelPayment = PagoAplicado.GetStringVal("IDENT");
                        check.Payments.Add(payment);
                        PagoAplicado = PagosEmpleado.Next();
                    }
                }
                catch (Exception ex)
                {
                    // ACPOS_SERVICE_MOBILE.logger.Error($"Error al obtener pagos en cheque cerrado {requestCloseCheckSap.CheckId}", ex);
                }

                // PROMOCIONES
                try
                {
                    IberEnum PromocionesEmpleado = ChequeCerrado.GetEnum((int)COMEnums.INTERNAL_CHECKS_PROMOS);
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
                    // ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar promociones en cheque cerrado {requestCloseCheckSap.CheckId}", ex);
                }

                // CORTESÍAS
                try
                {
                    IberEnum CortesiasEmpleado = ChequeCerrado.GetEnum((int)COMEnums.INTERNAL_CHECKS_COMPS);
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
                    // ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar cortesías en cheque cerrado {requestCloseCheckSap.CheckId}", ex);
                }

                // CALCULAR MONTO PENDIENTE
                double AmountPayed = check.Payments.Sum(p => p.Amount);
                check.AmountDue = ChequeCerrado.GetDoubleVal("COMPLETETOTAL") - AmountPayed;

               
                check.Guests = ChequeCerrado.GetLongVal("GUESTS");
                check.ChceckNumber = SdkFunctions.GetCheckNumberFromCheckId(check.Id);
                check.TotalCheck =    SubTotal;
                check.NumCheck = ChequeCerrado.GetLongVal("NUMBER") + 1;
            }
            catch (Exception ex)
            {
                ACPOS_SERVICE_MOBILE.logger.Error($"Error al recuperar la cuenta cerrada {requestCloseCheckSap.CheckId}", ex);
                check = null;
            }

            return check;
        }


    }
}
