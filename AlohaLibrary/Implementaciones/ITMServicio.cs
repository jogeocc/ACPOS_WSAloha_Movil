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
    public class ITMServicio : ServicioBaseALH<ITM>, IITM
    {
        public ITMServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<ITM> GetAll()
        {
            List<ITM> itms = new List<ITM>();

            string query = $"SELECT ID,OWNERID,USERNUMBER,SHORTNAME,CHITNAME,LONGNAME,LONGNAME2,BOHNAME,ABBREV,TAXID,TAXID2,VTAXID,PRIORITY,ROUTING,PRINTONCHK,COMBINE,HIGHLIGHT,SURCHARGE,SURCHRGMOD,COST,MOD1,MOD2,MOD3,MOD4,MOD5,MOD6,MOD7,MOD8,MOD9,MOD10,ASKDESC,ASKPRICE,ISREFILL,VROUTING,IS_KVI,TRACKFOH,PRICE_ID,PRICE FROM ITM";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "ITM");
            DataTable tabla = ds.Tables["ITM"];

            foreach (DataRow item in tabla.Rows)
            {
                itms.Add(new ITM()
                {
                    ID = int.Parse(item["ID"].ToString()),
                    OWNERID = int.Parse(item["OWNERID"].ToString()),
                    USERNUMBER = int.Parse(item["USERNUMBER"].ToString()),
                    SHORTNAME = (item["SHORTNAME"].ToString()),
                    CHITNAME = (item["CHITNAME"].ToString()),
                    LONGNAME = (item["LONGNAME"].ToString()),
                    LONGNAME2 =(item["LONGNAME2"].ToString()),
                    BOHNAME = (item["BOHNAME"].ToString()),
                    ABBREV = (item["ABBREV"].ToString()),
                    TAXID = int.Parse(item["TAXID"].ToString()),
                    TAXID2 = int.Parse(item["TAXID2"].ToString()),
                    VTAXID = int.Parse(item["VTAXID"].ToString()),
                   
                    PRICE_ID = int.Parse(item["PRICE_ID"].ToString()),
                    PRICE = decimal.Parse(item["PRICE"].ToString()),
                    MOD1 = int.Parse(item["MOD1"].ToString()),
                    MOD2 = int.Parse(item["MOD2"].ToString()),
                    MOD3 = int.Parse(item["MOD3"].ToString()),
                    MOD4 = int.Parse(item["MOD4"].ToString()),
                    MOD5 = int.Parse(item["MOD5"].ToString()),
                    MOD6 = int.Parse(item["MOD6"].ToString()),
                    MOD7 = int.Parse(item["MOD7"].ToString()),
                    MOD8 = int.Parse(item["MOD8"].ToString()),
                    MOD9 = int.Parse(item["MOD9"].ToString()),
                    MOD10 = int.Parse(item["MOD10"].ToString()),
                });
            }

            return itms;
        }
    }
}
