using Aloha.SDK.Common;
using AlohaWebServiceMobile.CodigosErrorAloha;
using AlohaWebServiceMobile.Enums;
using AlohaWebServiceMobile.Models.Aloha;
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
        public ResponseAloha login(int IdTerm, int IdEmpleado)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                xFunction.LogIn(IdTerm, IdEmpleado, "", "");
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.isClockIn = IsAlreadyClockIn(IdEmpleado);
                responseAloha.mensaje = "Login realizado con exito";
                responseAloha.Nombre_Empleado = NombreEmpleado(IdEmpleado);
                responseAloha.idJobs = IdsJobsEmpleado(IdEmpleado);
                responseAloha.mesas_empleado = RecuperarMesas(IdEmpleado);

            }
            catch (Exception ex)
            {
                string CodigoError = ex.Message.Substring(ex.Message.Count() - 10);
                responseAloha.mensaje = $"Error al intentar ingresar con el usuario {IdEmpleado} - {(ErroresAloha.MensajeMobile(CodigoError))}";
                App.logger.Error("Error al ingresar con el usuario tal", ex);
            }
            return responseAloha;
        }
        public bool logout(int IdTerm)
        {
            bool IsLogedOut = false;
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                xFunction.LogOut(IdTerm);
                IsLogedOut = true;
            }
            catch (Exception ex)
            {
                App.logger.Error("Error al ingresar con el usuario tal", ex);
            }
            return IsLogedOut;
        }
        public bool ClockIn(int IdTerm, int IdJobCode)
        {
            bool IsSuccess = false;
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                xFunction.ClockIn(IdTerm, IdJobCode);
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                App.logger.Error("Error", ex);
            }
            return IsSuccess;
        }
        public bool ClockOut(int IdTerm, double tips, double DeclaredCash)
        {
            bool IsSuccess = false;
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                xFunction.PerformCheckout(IdTerm, DeclaredCash);
                xFunction.ClockOut(IdTerm, tips);
            }
            catch (Exception ex)
            {
                App.logger.Error("Error en Clock Out", ex);
            }
            return IsSuccess;
        }
        public ResponseAloha OpenTable(int IdTerm, int idNumMesa, string NombreMesa, int NumInvitados)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                int IdMesaInterno = xFunction.AddTable(IdTerm, 0, idNumMesa, NombreMesa, NumInvitados);
                responseAloha.idMesa = IdMesaInterno;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Mesa abierta con exito";
            }
            catch (Exception ex)
            {
                responseAloha.mensaje = $"Error abriendo cuenta {idNumMesa}";
                App.logger.Error("Error al abrir mesa id = , ", ex);
            }
            return responseAloha;
        }
        public ResponseAloha OpenTab(int IdTerm, int idNumMesa, string NombreMesa, int NumInvitados)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                //Para abrir un tab, por defecto debe de ser el numero de mesa en 0
                int IdMesaInterno = xFunction.AddTable(IdTerm, 0, idNumMesa, NombreMesa, NumInvitados);
                responseAloha.idMesa = IdMesaInterno;
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Cuenta abierta";
            }
            catch (Exception ex)
            {
                responseAloha.mensaje = $"Error abriendo cuenta {NombreMesa}";
                App.logger.Error($"Error al abrir mesa id = {idNumMesa}, ", ex);
            }
            return responseAloha;
        }
        public ResponseAloha CloseTabTable(int IdTerm, int IdMesaInterno)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                xFunction.CloseTable(IdTerm, IdMesaInterno);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Mesa/Cuenta cerrada con éxito";
            }
            catch (Exception ex)
            {
                App.logger.Error("Error al cerrar mesa", ex);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Mesa/Cuenta con error al cerrar";

            }
            return responseAloha;
        }
        public ResponseAloha OpenCheck(int IdTerm, int IdMesaInterno)
        {
            ResponseAloha responseAloha = new ResponseAloha();

            try
            {
                int idChequeInterno = xFunction.AddCheck(IdTerm, IdMesaInterno);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = $"Cuenta abierda con id {idChequeInterno}";
                responseAloha.idMesa = idChequeInterno;
            }
            catch (Exception ex)
            {
                App.logger.Error($"Error al abrir cheque", ex);
            }
            return responseAloha;
        }
        public ResponseAloha CloseCheck(int IdTerm, int IdCheckInterno)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                xFunction.CloseCheck(IdTerm, IdCheckInterno);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = $"Cuenta Cerrada con id ";
            }
            catch (Exception ex)
            {
                responseAloha.mensaje = $"Error al cerrar cuenta con id ";
                App.logger.Error($"Error al cerrar cheque", ex);
            }
            return responseAloha;

        }
        public ResponseAloha AddItem(int IdTerm, int IdCheck, int IdItem, double Amount, int IdMod, double AmountMod)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                int idEntry = xFunction.BeginItem(IdTerm, IdCheck, IdItem, "", Amount);
                #region modificadores
                //xFunction.ModItem(IdTerm, idEntry, IdMod, "", AmountMod, 0);
                #endregion
                xFunction.EndItem(IdTerm);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Producto insertado con exito";
            }
            catch (Exception ex)
            {
                App.logger.Error("Error al agregar item", ex);
            };

            return responseAloha;
        }
        public ResponseAloha AddSpecialMessage(int IdTerm, int IdCheckId, int IdEntry, string Message)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
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
        public ResponseAloha ConfirmOrderMode(int IdTerm, int IdMesa, int IdModoPedido)
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                xFunction.OrderItems(IdTerm, IdMesa, IdModoPedido);
                responseAloha.Codigo = (int)CodigosError.NO_ERROR;
                responseAloha.mensaje = "Productos ordenados con exito";
            }
            catch (Exception ex)
            {
                App.logger.Error("Error al agregar item", ex);
            };
            return responseAloha;
        }
        public ResponseAloha AplicarPago(int IdTerm, int IdChequeId, int IdPago, double Amount, double Tip, string Digitos = "", string Expiracion = "", string Info = "", string Autorizacion = "")
        {
            ResponseAloha responseAloha = new ResponseAloha();
            try
            {
                xFunction.ApplyPayment(IdTerm, IdChequeId, IdPago, Amount, Tip, Digitos, Expiracion, Info, Autorizacion);
            }
            catch (Exception ex)
            {
                App.logger.Error("Error al aplicar pago", ex);
            }
            return responseAloha;
        }

        //FUNCIONES DE CONTROL DE DATOS
        private bool IsAlreadyClockIn(int IdEmpleado)
        {
            bool IsClocked = false;
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                IIberDepot depot = AlohaSdkFactory.GetIberDepotInstance();
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
        private string NombreEmpleado(int IdEmpleado)
        {
            string nombre = "";
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                IIberDepot depot = AlohaSdkFactory.GetIberDepotInstance();
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
        private List<int> IdsJobsEmpleado(int IdEmpleado)
        {
            List<int> ListaJobs = new List<int>();
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                IIberDepot depot = AlohaSdkFactory.GetIberDepotInstance();
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

        private List<MesaEmpleado> RecuperarMesas(int IdEmpleado)
        {
            List<MesaEmpleado> ListaMesas = new List<MesaEmpleado>();
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                IIberDepot depot = AlohaSdkFactory.GetIberDepotInstance();
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

                            //ITEMS
                            try
                            {
                                IberEnum ItemsEmpleado = ChequeAbierto.GetEnum((int)COMEnums.INTERNAL_CHECKS_ENTRIES);
                                IberObject ItemAbierto = ItemsEmpleado.First();
                                int IdPadre = 0;
                                while (ItemAbierto != null)
                                {
                                    Item item = new Item();
                                    item.IdEntry = ItemAbierto.GetLongVal("ID");
                                    item.Name = ItemAbierto.GetStringVal("DISP_NAME");
                                    item.Price = ItemAbierto.GetDoubleVal("PRICE");
                                    item.DisplayPrice = ItemAbierto.GetStringVal("DISP_PRICE").Trim(); ;
                                    item.NivelMod = ItemAbierto.GetLongVal("LEVEL");

                                    if (item.NivelMod == 0)
                                    {
                                        IdPadre = item.IdEntry;
                                        check.Items.Add(item);
                                    }
                                    else
                                    {
                                        check.Items.First(I => I.IdEntry == IdPadre).Mods.Add(item);
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

                            }
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
                App.logger.Error("Error recueprando mesas del empleado", ex);
            }
            return ListaMesas;
        }
    }

}
