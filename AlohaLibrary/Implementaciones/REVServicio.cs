using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Contexto;
using AlohaLibrary.Enums;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;

namespace AlohaLibrary.Implementaciones
{
    public class REVServicio : ServicioBaseALH<REV>, IREV
    {
        public REVServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<REV> GetAll()
        {
            List<REV> itms = new List<REV>();

            string query = $"SELECT ID,OWNERID,USERNUMBER,NAME,AUTOGRAT,NUMTABS,MINGRAT,PRINTCHK,WAITFORAUT,TIPLINE,ROOMLINE,NOTIPS,GRATTEXT,GRATPERCNT,GRATDOLLAR,PIVOTSEAT,PIVOTGST,PIVOTCAT,ENTREE,ENTREECAT,BARGUEST,MEMBINCNUM,DISPFLD1,DISPFLD2,DISPFLD3,GRATTAXID,MINGSTGRTX,VERPMSINFO,ITMGRATTAX FROM REV";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "REV");
            DataTable tabla = ds.Tables["REV"];

            foreach (DataRow item in tabla.Rows)
            {
                itms.Add(new REV()
                {
                    ID = int.Parse(item["ID"].ToString()),
                    OWNERID = int.Parse(item["OWNERID"].ToString()),
                    USERNUMBER = int.Parse(item["USERNUMBER"].ToString()),
                    NAME = (item["NAME"].ToString()),
                    AUTOGRAT = item["AUTOGRAT"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    BARGUEST = item["BARGUEST"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    DISPFLD1 = int.Parse(item["DISPFLD1"].ToString()),
                    DISPFLD2 = int.Parse(item["DISPFLD2"].ToString()),
                    DISPFLD3 = int.Parse(item["DISPFLD3"].ToString()),
                    ENTREE = item["ENTREE"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    ENTREECAT = int.Parse(item["ENTREECAT"].ToString()),
                    GRATDOLLAR = decimal.Parse(item["GRATDOLLAR"].ToString()),
                    GRATPERCNT = decimal.Parse(item["GRATPERCNT"].ToString()),
                    GRATTAXID = int.Parse(item["GRATTAXID"].ToString()),
                    GRATTEXT = item["GRATTEXT"].ToString(),
                    ITMGRATTAX = item["ITMGRATTAX"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    MEMBINCNUM = item["MEMBINCNUM"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    MINGRAT = decimal.Parse(item["MINGRAT"].ToString()),
                    MINGSTGRTX = int.Parse(item["MINGSTGRTX"].ToString()),
                    NOTIPS = item["NOTIPS"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    NUMTABS = item["NUMTABS"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    PIVOTCAT = int.Parse(item["PIVOTCAT"].ToString()),
                    PIVOTGST = item["PIVOTGST"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    PIVOTSEAT = item["PIVOTGST"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    PRINTCHK = int.Parse(item["PRINTCHK"].ToString()),
                    ROOMLINE = (item["ROOMLINE"].ToString()),
                    TIPLINE = (item["TIPLINE"].ToString()),
                    VERPMSINFO = item["VERPMSINFO"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                    WAITFORAUT = item["WAITFORAUT"].ToString().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N,
                });
            }

            return itms;
        }
    }
}
