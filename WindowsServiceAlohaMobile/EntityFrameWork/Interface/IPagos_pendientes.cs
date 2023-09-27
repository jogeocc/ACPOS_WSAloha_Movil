using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.EntityFrameWork.Infraestructura;
using WindowsServiceAlohaMobile.EntityFrameWork.Models;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Interface
{
    public interface IPagos_pendientes : IServiceBase<Pagos_pendientes>
    {
        bool TieneRegistros(int PagoPendienteId);
    }
}
