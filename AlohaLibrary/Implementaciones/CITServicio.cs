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

namespace AlohaLibrary.Implementaciones
{
    public class CITServicio: ServicioBaseALH<CIT>, ICIT
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

            foreach (DataRow cat_item in tabla.Rows)
            {
                CITS.Add(new CIT()
                {
                    CATEGORY = int.Parse(cat_item["CATEGORY"].ToString()),
                    ITEMID = int.Parse(cat_item["ITEMID"].ToString()),
                });
            }
            return CITS;

        }
    }
}
