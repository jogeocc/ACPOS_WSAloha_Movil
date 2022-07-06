using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Contexto;

namespace AlohaLibrary.Infraestrutura
{
    public interface IServicioBaseALH<T> where T : class
    {
        AplicacionBdContextoALH Contexto { get; }

        OleDbDataAdapter EjecutarConsulta(string query);

        List<T> GetAll();
    }
}
