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
    public class TDRServicio : ServicioBaseALH<TDR>, ITDRServicio
    {
        public TDRServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<TDR> GetAll()
        {
            List<TDR> lista = new List<TDR>();

            string query = "SELECT * FROM TDR";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "TDR");
            DataTable tabla = ds.Tables["TDR"];

            foreach (DataRow item in tabla.Rows)
            {
                lista.Add(new TDR()
                {
                    ID = int.Parse(item["ID"].ToString()),
                    OWNERID = int.Parse(item["OWNERID"].ToString()),
                    USERNUMBER = int.Parse(item["USERNUMBER"].ToString()),
                    NAME = item["NAME"].ToString(),
                    CASH = item["CASH"].ToString().ToUpper().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    ACTIVE = item["ACTIVE"].ToString().ToUpper().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    TIPS = item["TIPS"].ToString().ToUpper().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                });
            }

            return lista;
        }
    }
}
