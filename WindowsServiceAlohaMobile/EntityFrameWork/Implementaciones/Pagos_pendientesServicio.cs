using AlohaWebServiceMobile.EntityFrameWork.Infraestructura;
using AlohaWebServiceMobile.EntityFrameWork.Interface;
using AlohaWebServiceMobile.EntityFrameWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
