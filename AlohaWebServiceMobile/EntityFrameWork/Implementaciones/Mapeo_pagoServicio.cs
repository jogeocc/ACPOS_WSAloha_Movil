using AlohaWebServiceMobile.EntityFrameWork.Infraestructura;
using AlohaWebServiceMobile.EntityFrameWork.Interface;
using AlohaWebServiceMobile.EntityFrameWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.EntityFrameWork.Implementaciones
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
