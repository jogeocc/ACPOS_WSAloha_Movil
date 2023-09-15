using AlohaLibrary.Contexto;
using AlohaLibrary.Enums;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using AlohaLibrary.Utils;
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

            string query = $"SELECT * FROM CAT";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "CAT");
            DataTable tabla = ds.Tables["CAT"];

            foreach (DataRow item in tabla.Rows)
            {
                CAT CAT = new CAT();
                new GeneralFunctions().ReadDbf(item, ref CAT);
                categorias.Add(CAT);
            }

            return categorias;
        }
    }
}
