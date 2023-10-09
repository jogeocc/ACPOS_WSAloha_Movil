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
    public class CMPServicio : ServicioBaseALH<CMP>, ICMP
    {
        public CMPServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<CMP> GetAll()
        {
            List<CMP> List = new List<CMP>();

            string query = "SELECT * FROM CMP";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "CMP");

            DataTable tabla = ds.Tables["CMP"];

            foreach (DataRow item in tabla.Rows)
            {
                CMP Registro = new CMP();

                new GeneralFunctions().ReadDbf(item, ref Registro);
                List.Add(Registro);
            }
            return List;
        }
    }
}
