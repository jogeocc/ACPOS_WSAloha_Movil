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
    public class OrdenServicioALH : ServicioBaseALH<OrdenALH>, IOrdenServicioALH
    {
        public OrdenServicioALH(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<OrdenALH> GetAll()
        {
            List<OrdenALH> ordenes = new List<OrdenALH>();

            string query = $"SELECT ID, NAME, ACTIVE FROM ODR";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "ODR");
            DataTable tabla = ds.Tables["ODR"];

            foreach (DataRow item in tabla.Rows)
            {
                ordenes.Add(new OrdenALH
                {
                    ID = int.Parse(item["ID"].ToString()),
                    NAME = item["NAME"].ToString(),
                    ACTIVE = item["ACTIVE"].ToString().ToUpper().Equals("Y")
                            ? TipoLogicoALH.Y
                            : TipoLogicoALH.N
            });
            }

            return ordenes;
        }
    }
}
