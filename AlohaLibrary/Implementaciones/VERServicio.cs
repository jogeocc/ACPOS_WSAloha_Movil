using AlohaLibrary.Contexto;
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
    public class VERServicio : ServicioBaseALH<VER>, IVER
    {
        public VERServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<VER> GetAll()
        {
            List<VER> Lista = new List<VER>();
            string query = "SELECT * FROM VER";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "VER");
            DataTable tabla = ds.Tables["VER"];
            foreach (DataRow item in tabla.Rows)
            {
                VER ver = new VER();
                new GeneralFunctions().ReadDbf(item, ref ver);
                Lista.Add(ver);
            }
            return Lista;
        }
    }
}
