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
    public class GNDAuditServicio : ServicioBaseALH<GNDAudit>, IGNDAudit
    {
        public GNDAuditServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<GNDAudit> GetAll()
        {
            List<GNDAudit> lista = new List<GNDAudit>();

            string query = $"SELECT AUDITTYPE,DOB,HOUR,MINUTE,EMPLOYEE,CHECK,ITEM,QUANTITY,AMOUNT,PREVCHECK,PREVEMP,ORIGCHECK,ORIGEMP,MANAGER,REASON,DATA1,DATA2,OCCASION FROM GNDAudit";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "GNDAudit");
            DataTable tabla = ds.Tables["GNDAudit"];

            foreach (DataRow item in tabla.Rows)
            {
                
                lista.Add(new GNDAudit
                {
                    AUDITTYPE = int.Parse(item["AUDITTYPE"].ToString()),
                    DOB = DateTime.Parse(item["DOB"].ToString()),
                    HOUR = int.Parse(item["HOUR"].ToString()),
                    MINUTE = int.Parse(item["MINUTE"].ToString()),
                    EMPLOYEE = int.Parse(item["EMPLOYEE"].ToString()),
                    CHECK = int.Parse(item["CHECK"].ToString()),
                    ITEM = int.Parse(item["ITEM"].ToString()),
                    QUANTITY = decimal.Parse(item["QUANTITY"].ToString()),
                    AMOUNT = decimal.Parse(item["AMOUNT"].ToString()),
                    PREVCHECK = int.Parse(item["PREVCHECK"].ToString()),
                    PREVEMP = int.Parse(item["PREVEMP"].ToString()),
                    ORIGCHECK = int.Parse(item["ORIGCHECK"].ToString()),
                    ORIGEMP = int.Parse(item["ORIGEMP"].ToString()),
                    MANAGER = int.Parse(item["MANAGER"].ToString()),
                    REASON = int.Parse(item["REASON"].ToString()),
                    DATA1 = int.Parse(item["DATA1"].ToString()),
                    DATA2 = int.Parse(item["DATA2"].ToString()),
                    OCCASION = int.Parse(item["OCCASION"].ToString())

                });
            }

            return lista;
        }
    }
}
