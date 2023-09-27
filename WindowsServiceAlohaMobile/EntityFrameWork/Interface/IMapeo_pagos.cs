using AlohaWebServiceMobile.EntityFrameWork.Infraestructura;
using AlohaWebServiceMobile.EntityFrameWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Interface
{
    public interface IMapeo_pagos : IServiceBase<Mapeo_pagos>
    {
        bool TieneRegistros(int mapeoId);
    }
}
