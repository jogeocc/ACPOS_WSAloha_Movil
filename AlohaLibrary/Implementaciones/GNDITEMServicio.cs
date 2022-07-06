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
    public class GNDITEMServicio : ServicioBaseALH<GNDITEM>, IGNDITEMServicio
    {
        public GNDITEMServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<GNDITEM> GetAll()
        {
            List<GNDITEM> lista = new List<GNDITEM>();

            string query = $"SELECT TYPE, EMPLOYEE, CHECK, ITEM, ENTRYID, CATEGORY, PERIOD, MODE, PRICE, QUANTITY, DISCPRIC, INCLTAX, TAXID FROM GNDITEM";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "GNDITEM");
            DataTable tabla = ds.Tables["GNDITEM"];

            foreach (DataRow item in tabla.Rows)
            {
               
                lista.Add(new GNDITEM
                {
                    TYPE = int.Parse(item["TYPE"].ToString()),
                    EMPLOYEE = int.Parse(item["EMPLOYEE"].ToString()),
                    CHECK = int.Parse(item["CHECK"].ToString()),
                    ITEM = int.Parse(item["ITEM"].ToString()),
                    CATEGORY = int.Parse(item["CATEGORY"].ToString()),
                    MODE = int.Parse(item["MODE"].ToString()),
                    PERIOD = int.Parse(item["PERIOD"].ToString()),
                    PRICE = decimal.Parse(item["PRICE"].ToString()),
                    QUANTITY = decimal.Parse(item["QUANTITY"].ToString()),
                    DISCPRIC = decimal.Parse(item["DISCPRIC"].ToString()),
                    INCLTAX = decimal.Parse(item["INCLTAX"].ToString()),
                    TAXID = int.Parse(item["TAXID"].ToString()),
                    ENTRYID = int.Parse(item["ENTRYID"].ToString()),
                });
            }

            return lista;
        }
    }
}
