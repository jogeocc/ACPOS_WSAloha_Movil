using AlohaLibrary.Contexto;
using AlohaLibrary.Enums;
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
    public class RsnServicioALH : ServicioBaseALH<Rsn>, IRsn
    {
        public RsnServicioALH(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<Rsn> GetAll()
        {
            List<Rsn> lista = new List<Rsn>();

            string query = $"SELECT ID,OWNERID,USERNUMBER,NAME FROM Rsn";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "Rsn");
            DataTable tabla = ds.Tables["Rsn"];

            foreach (DataRow item in tabla.Rows)
            {
                lista.Add(new Rsn
                {
                    ID = int.Parse(item["ID"].ToString()),
                    OWNERID = int.Parse(item["OWNERID"].ToString()),
                    USERNUMBER = int.Parse(item["USERNUMBER"].ToString()),
                    NAME = (item["NAME"].ToString()),
                });
            }

            return lista;
        }
    }
}
