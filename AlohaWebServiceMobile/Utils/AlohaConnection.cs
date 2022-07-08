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
    }
}
