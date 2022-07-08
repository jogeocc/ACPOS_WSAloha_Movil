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
        public bool login(int CodigoEmpleado)
        {
            bool isLoged = false;
            try
            {
                xFunction = AlohaSdkFactory.GetIberFuncs23Instance();
                xFunction.LogIn(3, CodigoEmpleado, "", "");
                xFunction.LogOut(3);
                isLoged = true;
            }
            catch (Exception ex)
            {

            }
            return isLoged;
        }

    }
}
