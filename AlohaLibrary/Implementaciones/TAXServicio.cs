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
    public class TAXServicio : ServicioBaseALH<TAX>, ITAX
    {
        public TAXServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<TAX> GetAll()
        {
            List<TAX> itms = new List<TAX>();

            string query = $"SELECT ID ,OWNERID ,USERNUMBER ,NAME ,SUBSTITUTE ,EXCLUSIVE ,INCLUSIVE ,VENDOR ,RATE  FROM TAX";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "TAX");
            DataTable tabla = ds.Tables["TAX"];

            foreach (DataRow item in tabla.Rows)
            {
                itms.Add(new TAX()
                {
                    ID = int.Parse(item["ID"].ToString()),
                    OWNERID = int.Parse(item["OWNERID"].ToString()),
                    USERNUMBER = int.Parse(item["USERNUMBER"].ToString()),
                    NAME = (item["NAME"].ToString()),
                    SUBSTITUTE = (item["SUBSTITUTE"].ToString()),
                    EXCLUSIVE = (item["EXCLUSIVE"].ToString()),
                    INCLUSIVE = (item["INCLUSIVE"].ToString()),
                    VENDOR = (item["VENDOR"].ToString()),
                    RATE = decimal.Parse(item["RATE"].ToString()),


                });
            }

            return itms;
        }
    }
}
