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
    public class Mapeo_pagoServicio : ServiceBase<Mapeo_pagos>, IMapeo_pagos
    {
        public Mapeo_pagoServicio(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }

        public bool TieneRegistros(int mapeoId)
        {
            //return dataContext.Transacciones.Any(t => t.DispositivoId.HasValue && t.DispositivoId.Value == mapeoId);
            return false;
        }
    }
}
