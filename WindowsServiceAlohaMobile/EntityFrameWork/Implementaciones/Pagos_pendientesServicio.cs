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
    public class Pagos_pendientesServicio : ServiceBase<Pagos_pendientes>, IPagos_pendientes
    {
        public Pagos_pendientesServicio(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public bool TieneRegistros(int PagoPendienteId)
        {
            return false;
        }
    }
}
