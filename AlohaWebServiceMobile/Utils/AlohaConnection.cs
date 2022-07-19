using Aloha.SDK.Common;
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

        public bool login(int IdTerm, int IdEmpleado)
        {
            bool isLoged = false;
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                IsAlreadyClockIn(IdEmpleado);
                xFunction.LogIn(IdTerm, IdEmpleado, "", "");
                isLoged = true;
            }
            catch (Exception ex)
            {
                App.logger.Error("Error al ingresar con el usuario tal", ex);
            }
            return isLoged;
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
    }
}
