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
    public class PCIDServicio : ServicioBaseALH<PCID>, IPCID
    {
        public PCIDServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<PCID> GetAll()
        {

            List<PCID> List = new List<PCID>();
            string query = $"SELECT * FROM PCID";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "PCID");
            DataTable tabla = ds.Tables["PCID"];

            foreach (DataRow ACCES in tabla.Rows)
            {
                PCID Acceso = new PCID();
                new GeneralFunctions().ReadDbf(ACCES, ref Acceso);
                List.Add(Acceso);
            }
            return List;

        }
    }
}
