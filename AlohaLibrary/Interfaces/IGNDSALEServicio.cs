using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Modelos;
using AlohaLibrary.ModelosPersonalizados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Interfaces
{
    public interface IGNDSALEServicio : IServicioBaseALH<GNDSale>
    {
        List<Sale> GetSales();
    }
}
