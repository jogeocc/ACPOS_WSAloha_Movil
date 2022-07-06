using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Contexto;

namespace AlohaLibrary.Infraestrutura
{
    public abstract class ServicioBaseALH<T> : IServicioBaseALH<T> where T : class
    {
        public AplicacionBdContextoALH Contexto { get; private set; }

        public ServicioBaseALH(AplicacionBdContextoALH contexto)
        {
            Contexto = contexto;
        }

        public OleDbDataAdapter EjecutarConsulta(string query)
        {
            Contexto.Conexion.Open();
            Contexto.Adaptador.SelectCommand.CommandText = query;
            Contexto.Conexion.Close();
            return Contexto.Adaptador;
        }

        public abstract List<T> GetAll();
    }
}
