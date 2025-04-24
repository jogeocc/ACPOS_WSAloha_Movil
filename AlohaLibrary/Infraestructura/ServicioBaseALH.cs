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


        public  string Reconvertir(string cadena)
        {
            if (string.IsNullOrEmpty(cadena))
                return string.Empty;

            Encoding extAscii = Encoding.GetEncoding(850);   // OEM Latin-1
            Encoding win1252 = Encoding.GetEncoding(1252);   // Windows-1252

            byte[] bytes1252 = extAscii.GetBytes(cadena);

            byte[] output = Encoding.Convert(win1252, extAscii, bytes1252);

            return extAscii.GetString(output);
        }


    }
}
