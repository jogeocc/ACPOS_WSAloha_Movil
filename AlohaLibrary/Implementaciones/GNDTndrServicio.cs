using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using AlohaLibrary.ModelosPersonalizados;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class GNDTndrServicio : ServicioBaseALH<GNDTndr>, IGNDTndrServicio
    {
        public GNDTndrServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<GNDTndr> GetAll()
        {
            List<GNDTndr> lista = new List<GNDTndr>();

            string query = $"SELECT EMPLOYEE,CHECK,DATE,SYSDATE,TYPE,TYPEID, " +
                $" IIF(Isnull(IDENT),'0',IDENT) AS IDENT, " +
                $" IIF(Isnull(AUTH),'0',AUTH) AS AUTH, " +
                $" IIF(Isnull(EXP),'0',EXP) AS EXP, " +
                $" IIF(Isnull(NAME),'0',NAME) AS NAME, " +
                $" IIF(Isnull(UNIT),'0',UNIT) AS UNIT, " +
                $" IIF(Isnull(AMOUNT),'0',AMOUNT) AS AMOUNT, " +
                $" IIF(Isnull(TIP),'0',TIP) AS TIP, " +
                $" IIF(Isnull(NR),'0',NR) AS NR, " +
                $" IIF(Isnull(TRACK),'0',TRACK) AS TRACK, " +
                $" IIF(Isnull(HOUSEID),'0',HOUSEID) AS HOUSEID, " +
                $" IIF(Isnull(TIPPABLE),'0',TIPPABLE) AS TIPPABLE, " +
                $" IIF(Isnull(MANAGER),'0',MANAGER) AS MANAGER, " +
                $" MINUTE, " +
                $" HOUR, " +
                $" IIF(Isnull(ID),'0',ID) AS ID, " +
                $" IIF(Isnull(AUTOGRAT),'0',AUTOGRAT) AS AUTOGRAT, " +
                $" IIF(Isnull(STRUNIT),'0',STRUNIT) AS STRUNIT, " +
                $" IIF(Isnull(REVENUE),'0',REVENUE) AS REVENUE, " +
                $" IIF(Isnull(OCCASION),'0',OCCASION) AS OCCASION, " +
                $" IIF(Isnull(SOURCE),'0',SOURCE) AS SOURCE, " +
                $" IIF(Isnull(PMSPOSTD),'0',PMSPOSTD) AS PMSPOSTD, " +
                $" IIF(Isnull(DRAWER),'0', DRAWER) AS DRAWER " +
                $" FROM GNDTndr";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "GNDTndr");
            DataTable tabla = ds.Tables["GNDTndr"];

            foreach (DataRow item in tabla.Rows)
            {
                var tndr = new GNDTndr
                {
                    EMPLOYEE = int.Parse(item["EMPLOYEE"].ToString()),
                    CHECK = int.Parse(item["CHECK"].ToString()),
                    TYPE = int.Parse(item["TYPE"].ToString()),
                    TYPEID = int.Parse(item["TYPEID"].ToString()),
                    AMOUNT = decimal.Parse(item["AMOUNT"].ToString()),
                    DATE = DateTime.Parse(item["DATE"].ToString()),
                    SYSDATE = DateTime.Parse(item["SYSDATE"].ToString()),
                    IDENT = (item["IDENT"].ToString()),
                    AUTH = (item["AUTH"].ToString()),
                    EXP = (item["EXP"].ToString()),
                    NAME = (item["NAME"].ToString()),
                    UNIT = (item["UNIT"].ToString()),
                    TIP = decimal.Parse(item["TIP"].ToString()),
                    NR = int.Parse(item["NR"].ToString()),
                    HOUSEID = int.Parse(item["HOUSEID"].ToString()),
                    TIPPABLE = int.Parse(item["TIPPABLE"].ToString()),
                    MANAGER = int.Parse(item["MANAGER"].ToString()),
                    HOUR = int.Parse(item["HOUR"].ToString()),
                    MINUTE = int.Parse(item["MINUTE"].ToString()),
                    ID = int.Parse(item["ID"].ToString()),
                    AUTOGRAT = int.Parse(item["AUTOGRAT"].ToString()),
                    STRUNIT = int.Parse(item["STRUNIT"].ToString()),
                    REVENUE = int.Parse(item["REVENUE"].ToString()),
                    OCCASION = int.Parse(item["OCCASION"].ToString()),
                    SOURCE = int.Parse(item["SOURCE"].ToString()),
                    PMSPOSTD = int.Parse(item["PMSPOSTD"].ToString()),
                    DRAWER = int.Parse(item["DRAWER"].ToString())
                };

                tndr.Hora = new TimeSpan(tndr.HOUR ?? 0, tndr.MINUTE ?? 0, 0);

                lista.Add(tndr);
            }

            return lista;
        }

        public List<Tender> GetTenders()
        {
            List<Tender> lista = new List<Tender>();

            string query = "SELECT td.CHECK, td.TYPE, td.TYPEID, td.AMOUNT, td.TIP, tn.TABLEID, tn.PERIOD " +
                "FROM ((GNDTNDR td " +
                "INNER JOIN GCHKINFO c " +
                "ON td.CHECK = c.CHECKID) " +
                "INNER JOIN GNDTURN tn " +
                "ON c.TABLEID = tn.TABLEID)";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "Tender");
            DataTable tabla = ds.Tables["Tender"];

            foreach (DataRow item in tabla.Rows)
            {
                lista.Add(new Tender
                {
                    CHECK = int.Parse(item["CHECK"].ToString()),
                    TYPE = int.Parse(item["TYPE"].ToString()),
                    TYPEID = int.Parse(item["TYPEID"].ToString()),
                    AMOUNT = decimal.Parse(item["AMOUNT"].ToString()),
                    TIP = decimal.Parse(item["TIP"].ToString()),
                    TABLEID = int.Parse(item["TABLEID"].ToString()),
                    PERIOD = int.Parse(item["PERIOD"].ToString()),
                });
            }

            return lista;
        }
    }
}
