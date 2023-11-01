using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Utils
{
    public class Licencia
    {

        public string RevisarLicenciaSistema()
        {
            string licencia = "";

            return licencia;
        }

        public bool ValidarLimiteLicencias(Models.Licencia.DeviceLicencia device)
        {
            bool IsValid = false;

            if (IsFechaValida())
            {
                if (ValidarLicenciaActiva(device.IdDevice))
                {
                    IsValid = true;
                }
                else
                {
                    bool IsLicenciasMax = ACPOS_SERVICE_MOBILE.DbManager.CheckNumMaxLicencias();
                    if (IsLicenciasMax)
                    {
                        IsValid = ACPOS_SERVICE_MOBILE.DbManager.AddDeviceLicencia(device.IdDevice, device.DeviceName);
                    }
                }
            }
            return IsValid;
        }

        public bool ValidarLicenciaActiva(string IdDevice)
        {
            bool Exists = ACPOS_SERVICE_MOBILE.DbManager.IsDeviceValid(IdDevice);

            return Exists;
        }

        public bool IsFechaValida()
        {
            bool IsValid = false;

            IsValid = ACPOS_SERVICE_MOBILE.DbManager.IsFechaValida();

            return IsValid;
        }
    }
}
