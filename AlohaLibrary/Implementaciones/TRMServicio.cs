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
    public class TRMServicio : ServicioBaseALH<TRM>, ITRM
    {
        public TRMServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<TRM> GetAll()
        {
            List<TRM> Lista = new List<TRM>();
            string query = "SELECT * FROM TRM";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "TRM");
            DataTable tabla = ds.Tables["TRM"];
            foreach (DataRow item in tabla.Rows)
            {
                TRM Trm = new TRM();
                new GeneralFunctions().ReadDbf(item, ref Trm);
                Lista.Add(Trm);
            }
            return Lista;
        }
    }
}
