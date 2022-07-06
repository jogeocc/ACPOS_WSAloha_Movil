using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class GCHKINFOServicio : ServicioBaseALH<GCHKINFO>
    {
        public GCHKINFOServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<GCHKINFO> GetAll()
        {
            List<GCHKINFO> lista = new List<GCHKINFO>();

            string query = $"SELECT * FROM GCHKINFO";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "GCHKINFO");
            DataTable tabla = ds.Tables["GCHKINFO"];

            foreach (DataRow item in tabla.Rows)
            {

                lista.Add(new GCHKINFO
                {
                    UNIT = int.Parse(item["UNIT"].ToString()),
                    DOB = DateTime.Parse(item["DOB"].ToString()),
                    EMPLOYEE = int.Parse(item["EMPLOYEE"].ToString()),
                    CHECKID = int.Parse(item["CHECKID"].ToString()),
                    QUEUEID = int.Parse(item["QUEUEID"].ToString()),
                    TABLEID = int.Parse(item["TABLEID"].ToString()),
                });
            }

            return lista;
        }
    }
}
