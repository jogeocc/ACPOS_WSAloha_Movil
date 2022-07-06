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
    public class CATServicio : ServicioBaseALH<CAT>, ICATServicio
    {
        public CATServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<CAT> GetAll()
        {
            List<CAT> categorias = new List<CAT>();

            string query = $"SELECT ID, NAME, SALES FROM CAT";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "CAT");
            DataTable tabla = ds.Tables["CAT"];

            foreach (DataRow item in tabla.Rows)
            {
                categorias.Add(new CAT
                {
                    ID = int.Parse(item["ID"].ToString()),
                    NAME = item["NAME"].ToString(),
                    SALES = item["SALES"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N
                });
            }

            return categorias;
        }
    }
}
