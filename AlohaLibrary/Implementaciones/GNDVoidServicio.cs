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
    public class GNDVoidServicio : ServicioBaseALH<GNDVoid>, IGNDVoid
    {
        public GNDVoidServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<GNDVoid> GetAll()
        {
            List<GNDVoid> lista = new List<GNDVoid>();

            string query = $"SELECT EMPLOYEE,MANAGER,CHECK,TABLENAME,ITEM,PRICE,DATE,SYSDATE,HOUR,MINUTE,REASON,INVENTORY,UNIT,ENTRYID,OCCASION,REVID FROM GNDVoid";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "GNDVoid");
            DataTable tabla = ds.Tables["GNDVoid"];

            foreach (DataRow item in tabla.Rows)
            {
                lista.Add(new GNDVoid
                {
                    EMPLOYEE = int.Parse(item["EMPLOYEE"].ToString()),
                    MANAGER = int.Parse(item["MANAGER"].ToString()),
                    CHECK = int.Parse(item["CHECK"].ToString()),
                    TABLENAME = (item["TABLENAME"].ToString()),
                    ITEM = int.Parse(item["ITEM"].ToString()),
                    PRICE = decimal.Parse(item["PRICE"].ToString()),
                    DATE = DateTime.Parse(item["DATE"].ToString()),
                    SYSDATE = DateTime.Parse(item["SYSDATE"].ToString()),
                    HOUR = int.Parse(item["HOUR"].ToString()),
                    MINUTE = int.Parse(item["MINUTE"].ToString()),
                    REASON = int.Parse(item["REASON"].ToString()),
                    UNIT = int.Parse(item["UNIT"].ToString()),
                    ENTRYID = (item["ENTRYID"].ToString()!=null && item["ENTRYID"].ToString() !="") ?  int.Parse(item["ENTRYID"].ToString()) : 0,
                    OCCASION = int.Parse(item["OCCASION"].ToString()),
                    REVID = int.Parse(item["REVID"].ToString()),

                });
            }

            return lista;
        }
    }
}
