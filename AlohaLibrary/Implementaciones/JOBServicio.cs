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
    public class JOBServicio : ServicioBaseALH<JOB>, IJOB
    {
        public JOBServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<JOB> GetAll()
        {
            List<JOB> JobList = new List<JOB>();

            string query = "SELECT * FROM JOB";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "JOB");

            DataTable tabla = ds.Tables["JOB"];
            foreach (DataRow item in tabla.Rows)
            {
                JOB job = new JOB();
                job.ID = int.Parse(item["ID"].ToString());
                job.LONGNAME = item["LONGNAME"].ToString();
                job.SHORTNAME = item["SHORTNAME"].ToString();
                job.ORDERENTRY = item["ORDERENTRY"].ToString().ToUpper().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N;
                JobList.Add(job);
            }
            return JobList;
        }
    }
}
