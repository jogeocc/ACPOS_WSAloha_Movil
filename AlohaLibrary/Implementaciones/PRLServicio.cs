using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using System.Data;
using AlohaLibrary.Modelos;
using AlohaLibrary.Contexto;

namespace AlohaLibrary.Implementaciones
{
    public class PRLServicio : ServicioBaseALH<PRL>, IPRL
    {
        public PRLServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<PRL> GetAll()
        {
            List<PRL> prls = new List<PRL>();

            string query =
                $"SELECT * FROM PRL ORDER BY ID";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "PRL");
            DataTable tabla = ds.Tables["PRL"];

            foreach (DataRow prl in tabla.Rows)
            {
                prls.Add(new PRL()
                {
                    ID = int.Parse(prl["ID"].ToString()),
                    DESC = prl["DESC"].ToString(),
                    PRICE = decimal.Parse(prl["PRICE"].ToString())
                });
            }

            return prls;
        }
    }
}
