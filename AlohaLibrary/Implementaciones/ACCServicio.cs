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
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class ACCServicio : ServicioBaseALH<ACC>, IACC
    {
        public ACCServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<ACC> GetAll()
        {

            List<ACC> List = new List<ACC>();
            string query = $"SELECT * FROM ACC";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "ACC");
            DataTable tabla = ds.Tables["ACC"];



            foreach (DataRow ACCES in tabla.Rows)
            {
                ACC Acceso = new ACC();
                new GeneralFunctions().ReadDbf(ACCES, ref Acceso);
                List.Add(Acceso);
            }
            return List;
        }
    }
}
