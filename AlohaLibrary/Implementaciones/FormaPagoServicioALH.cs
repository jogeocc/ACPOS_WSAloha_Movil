using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;

namespace AlohaLibrary.Implementaciones
{
    public class FormaPagoServicioALH : ServicioBaseALH<FormaPagoALH>, IFormaPagoServicioALH
    {
        public FormaPagoServicioALH(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<FormaPagoALH> GetAll()
        {
            List<FormaPagoALH> formasPago = new List<FormaPagoALH>();

            string query = $"SELECT ID, NAME FROM TDR";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "TDR");
            DataTable tabla = ds.Tables["TDR"];

            foreach (DataRow item in tabla.Rows)
            {
                formasPago.Add(new FormaPagoALH()
                {
                    ID = int.Parse(item["ID"].ToString()),
                    NAME = item["NAME"].ToString(),
                });
            }

            return formasPago;
        }
    }
}
