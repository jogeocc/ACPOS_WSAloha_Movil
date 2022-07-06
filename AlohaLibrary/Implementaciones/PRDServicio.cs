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
    public class PRDServicio : ServicioBaseALH<PRD>, IPRDServicio
    {
        public PRDServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<PRD> GetAll()
        {
            List<PRD> lista = new List<PRD>();

            string query = "SELECT * FROM PRD";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "PRD");
            DataTable tabla = ds.Tables["PRD"];

            foreach (DataRow item in tabla.Rows)
            {
                var prd = new PRD()
                {
                    ID = int.Parse(item["ID"].ToString()),
                    OWNERID = int.Parse(item["OWNERID"].ToString()),
                    USERNUMBER = int.Parse(item["USERNUMBER"].ToString()),
                    NAME = item["NAME"].ToString(),
                    STARTHOUR = int.Parse(item["STARTHOUR"].ToString()),
                    STARTMIN = int.Parse(item["STARTMIN"].ToString()),
                };
                prd.HoraInicio = new TimeSpan(prd.STARTHOUR, prd.STARTMIN, 0); 
                lista.Add(prd);
            }

            return lista;
        }
    }
}
