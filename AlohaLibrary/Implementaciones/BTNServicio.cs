using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using AlohaLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class BTNServicio : ServicioBaseALH<BTN>, IBTN
    {
        public BTNServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<BTN> GetAll()
        {
            List<BTN> JobList = new List<BTN>();

            string query = "SELECT * FROM BTN";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "BTN");

            DataTable tabla = ds.Tables["BTN"];

            foreach (DataRow item in tabla.Rows)
            {
                BTN BTNITEM = new BTN();

                new GeneralFunctions().ReadDbf(item, ref BTNITEM);
                JobList.Add(BTNITEM);
            }
            return JobList;
        }
    }
}
