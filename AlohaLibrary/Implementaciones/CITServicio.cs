using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Infraestrutura;
using System.Data;
using AlohaLibrary.Contexto;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using AlohaLibrary.Utils;

namespace AlohaLibrary.Implementaciones
{
    public class CITServicio : ServicioBaseALH<CIT>, ICIT
    {
        public CITServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<CIT> GetAll()
        {
            List<CIT> CITS = new List<CIT>();
            string query = $"SELECT * FROM CIT ORDER BY CATEGORY";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "CIT");
            DataTable tabla = ds.Tables["CIT"];

            foreach (DataRow Item in tabla.Rows)
            {
                CIT CIT = new CIT();
                new GeneralFunctions().ReadDbf(Item, ref CIT);
                CITS.Add(CIT);
            }
            return CITS;

        }
    }
}
