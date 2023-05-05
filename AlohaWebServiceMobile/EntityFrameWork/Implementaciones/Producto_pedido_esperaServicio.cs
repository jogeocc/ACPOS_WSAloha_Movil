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
    public class Producto_pedido_esperaServicio : ServiceBase<Producto_Pedido_Espera>, IProducto_pedido_espera
    {
        public Producto_pedido_esperaServicio(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
