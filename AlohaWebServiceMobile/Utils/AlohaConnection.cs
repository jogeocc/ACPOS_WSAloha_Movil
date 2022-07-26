using Aloha.SDK.Common;
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

            }
            catch (Exception ex)
            {
                responseAloha.mensaje = $"Error al intentar ingresar con el usuario {IdEmpleado} - {ex.Message}";
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
            return ListaJobs;
        }
    }
}
