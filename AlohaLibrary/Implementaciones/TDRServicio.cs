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
using System.Reflection;
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
            List<TDR> Lista = new List<TDR>();
            string query = "SELECT * FROM TDR";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "TDR");
            DataTable tabla = ds.Tables["TDR"];
            foreach (DataRow item in tabla.Rows)
            {
                TDR Tdr = new TDR();
                new GeneralFunctions().ReadDbf(item, ref Tdr);
                Lista.Add(Tdr);
            }
            return Lista;
        }
    }
}
