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
    public class PCServicio : ServicioBaseALH<PC>, IPC
    {
        public PCServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<PC> GetAll()
        {

            List<PC> List = new List<PC>();
            string query = $"SELECT * FROM PC";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "PC");
            DataTable tabla = ds.Tables["PC"];

            foreach (DataRow ACCES in tabla.Rows)
            {
                PC Acceso = new PC();
                new GeneralFunctions().ReadDbf(ACCES, ref Acceso);
                List.Add(Acceso);
            }
            return List;
        }
    }
}
