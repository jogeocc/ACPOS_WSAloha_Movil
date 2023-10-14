using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.EntityFrameWork.Infraestructura;
using WindowsServiceAlohaMobile.EntityFrameWork.Interface;
using WindowsServiceAlohaMobile.EntityFrameWork.Models;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Implementaciones
{
    public class DeviceServicio : ServiceBase<Device>, IDevice
    {
        public DeviceServicio(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
