using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class EmpleadoServicioALH : ServicioBaseALH<EmpleadoALH>, IEmpleadoServicioALH
    {
        public EmpleadoServicioALH(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<EmpleadoALH> GetAll()
        {
            List<EmpleadoALH> lista = new List<EmpleadoALH>();

            string query = $"SELECT ID, USERNUMBER, FIRSTNAME, MIDDLENAME, LASTNAME, NICKNAME FROM EMP";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "EMP");
            DataTable tabla = ds.Tables["EMP"];

            foreach (DataRow item in tabla.Rows)
            {
                lista.Add(new EmpleadoALH
                {
                    ID = int.Parse(item["ID"].ToString()),
                    USERNUMBER = int.Parse(item["USERNUMBER"].ToString()),
                    FIRSTNAME = item["FIRSTNAME"].ToString().Trim(),
                    MIDDLENAME = item["MIDDLENAME"].ToString().Trim(),
                    LASTNAME = item["LASTNAME"].ToString().Trim(),
                    NICKNAME = item["NICKNAME"].ToString().Trim(),
                });
            }

            return lista;
        }
    }
}
