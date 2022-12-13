using AlohaWebServiceMobile.EntityFrameWork.Infraestructura;
using AlohaWebServiceMobile.EntityFrameWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.EntityFrameWork.Interface
{
    public interface IPagos_pendientes : IServiceBase<Pagos_pendientes>
    {
        bool TieneRegistros(int PagoPendienteId);
    }
}
