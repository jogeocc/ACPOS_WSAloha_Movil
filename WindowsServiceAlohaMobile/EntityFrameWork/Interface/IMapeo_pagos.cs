using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.EntityFrameWork.Infraestructura;
using WindowsServiceAlohaMobile.EntityFrameWork.Models;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Interface
{
    public interface IMapeo_pagos : IServiceBase<Mapeo_pagos>
    {
        bool TieneRegistros(int mapeoId);
    }
}
