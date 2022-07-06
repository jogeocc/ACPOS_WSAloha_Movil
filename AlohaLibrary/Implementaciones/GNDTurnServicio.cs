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
    public class GNDTurnServicio : ServicioBaseALH<GNDTurn>, IGNDTurnServicio
    {
        public GNDTurnServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<GNDTurn> GetAll()
        {
            List<GNDTurn> lista = new List<GNDTurn>();

            //string query = $"SELECT EMPLOYEE,DRIVER,JOBCODE,TABLE,TABLEID,NAME,PERIOD,REVID,MINUTES,SALES,CHECKS,GUESTS,WAITHOUR,WAITMIN,WAITSEC,SEATHOUR,SEATMIN,SEATSEC,OPENHOUR,OPENMIN,OPENSEC,CLOSEHOUR,CLOSEMIN,CLOSESEC,FIRSTORDHR,FIRSTORDMN,FIRSTORDSC,LASTORDHR,LASTORDMN,LASTORDSC,FIRSTPAYHR,FIRSTPAYMN,FIRSTPAYSC,LASTPAYHR,LASTPAYMN,LASTPAYSC,BUSHOUR,BUSMIN,BUSSEC,FIRSTBMPHR,FIRSTBMPMN,FIRSTBMPSC,LASTBMPHR,LASTBMPMN,LASTBMPSC,READYHOUR,READYMIN,READYSEC,ASSIGNHOUR,ASSIGNMIN,ASSIGNSEC,DRVOUTHOUR,DRVOUTMIN,DRVOUTSEC,DRVINHOUR,DRVINMIN,DRVINSEC,UNIT,DOB,PROMISED,CATEGORYID,MODEID,FSTVIDID,LSTVIDID,STOREID,OCCASION,TERMID,QUEUEID,FIRSTADDHR,FIRSTADDMN,FIRSTADDSC,LASTLOCKHR,LASTLOCKMN,LASTLOCKSC,CLOSEDRWHR,CLOSEDRWMN,CLOSEDRWSC,GUESTSRVHR,GUESTSRVMN,GUESTSRVSC,FRSTDISPHR,FRSTDISPMN,FRSTDISPSC,AUTHBGNHR,AUTHBGNMN,AUTHBGNSC,AUTHENDHR,AUTHENDMN,AUTHENDSC," +
            //    $"CHECK  FROM GNDTurn";

            string query = "SELECT EMPLOYEE,DRIVER,JOBCODE,TABLE,TABLEID,NAME,PERIOD,REVID,MINUTES,SALES,CHECKS,GUESTS,WAITHOUR,WAITMIN,WAITSEC,SEATHOUR,SEATMIN,SEATSEC,OPENHOUR,OPENMIN,OPENSEC,CLOSEHOUR,CLOSEMIN,CLOSESEC,FIRSTORDHR,FIRSTORDMN,FIRSTORDSC,LASTORDHR,LASTORDMN,LASTORDSC,FIRSTPAYHR,FIRSTPAYMN,FIRSTPAYSC,LASTPAYHR,LASTPAYMN,LASTPAYSC,BUSHOUR,BUSMIN,BUSSEC,FIRSTBMPHR,FIRSTBMPMN,FIRSTBMPSC,LASTBMPHR,LASTBMPMN,LASTBMPSC,READYHOUR,READYMIN,READYSEC,ASSIGNHOUR,ASSIGNMIN,ASSIGNSEC,DRVOUTHOUR,DRVOUTMIN,DRVOUTSEC,DRVINHOUR,DRVINMIN,DRVINSEC,UNIT,DOB,PROMISED,CATEGORYID,MODEID,FSTVIDID,LSTVIDID,STOREID,OCCASION,TERMID,QUEUEID,FIRSTADDHR,FIRSTADDMN,FIRSTADDSC,LASTLOCKHR,LASTLOCKMN,LASTLOCKSC,CLOSEDRWHR,CLOSEDRWMN,CLOSEDRWSC,GUESTSRVHR,GUESTSRVMN,GUESTSRVSC,FRSTDISPHR,FRSTDISPMN,FRSTDISPSC,AUTHBGNHR,AUTHBGNMN,AUTHBGNSC,AUTHENDHR,AUTHENDMN,AUTHENDSC " +
                "FROM GNDTurn";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "GNDTurn");
            DataTable tabla = ds.Tables["GNDTurn"];

            foreach (DataRow item in tabla.Rows)
            {
             
                lista.Add(new GNDTurn
                {
                    EMPLOYEE = int.Parse(item["EMPLOYEE"].ToString()),
                    DRIVER = int.Parse(item["DRIVER"].ToString()),
                    JOBCODE = int.Parse(item["JOBCODE"].ToString()),
                    TABLE = int.Parse(item["TABLE"].ToString()),
                    TABLEID = int.Parse(item["TABLEID"].ToString()),
                    NAME = (item["NAME"].ToString()),
                    PERIOD = int.Parse(item["PERIOD"].ToString()),
                    REVID = int.Parse(item["REVID"].ToString()),
                    MINUTES = int.Parse((item["MINUTES"].ToString() != "")?item["MINUTES"].ToString():"0"),
                    SALES = Decimal.Parse(item["SALES"].ToString()),
                    CHECKS = int.Parse(item["CHECKS"].ToString()),
                    GUESTS = int.Parse(item["GUESTS"].ToString()),
                    //WAITHOUR = int.Parse(item["WAITHOUR"].ToString()),
                    //WAITMIN = int.Parse(item["WAITMIN"].ToString()),
                    //WAITSEC = int.Parse(item["WAITSEC"].ToString()),
                    //SEATHOUR = int.Parse(item["SEATHOUR"].ToString()),
                    //SEATMIN = int.Parse(item["SEATMIN"].ToString()),
                    //SEATSEC = int.Parse(item["SEATSEC"].ToString()),
                    OPENHOUR = int.Parse(item["OPENHOUR"].ToString()),
                    OPENMIN = int.Parse(item["OPENMIN"].ToString()),
                    OPENSEC = int.Parse(item["OPENSEC"].ToString()),
                    CLOSEHOUR = int.Parse(item["CLOSEHOUR"].ToString()),
                    CLOSEMIN = int.Parse(item["CLOSEMIN"].ToString()),
                    CLOSESEC = int.Parse(item["CLOSESEC"].ToString()),
                    //FIRSTORDHR = int.Parse(item["FIRSTORDHR"].ToString()),
                    //FIRSTORDMN = int.Parse(item["FIRSTORDMN"].ToString()),
                    //FIRSTORDSC = int.Parse(item["FIRSTORDSC"].ToString()),
                    //LASTORDHR = int.Parse(item["LASTORDHR"].ToString()),
                    //LASTORDMN = int.Parse(item["LASTORDMN"].ToString()),
                    //LASTORDSC = int.Parse(item["LASTORDSC"].ToString()),
                    //FIRSTPAYHR = int.Parse(item["FIRSTPAYHR"].ToString()),
                    //FIRSTPAYMN = int.Parse(item["FIRSTPAYMN"].ToString()),
                    //FIRSTPAYSC = int.Parse(item["FIRSTPAYSC"].ToString()),
                    //LASTPAYHR = int.Parse(item["LASTPAYHR"].ToString()),
                    //LASTPAYMN = int.Parse(item["LASTPAYMN"].ToString()),
                    //LASTPAYSC = int.Parse(item["LASTPAYSC"].ToString()),
                    //BUSHOUR = int.Parse(item["BUSHOUR"].ToString()),
                    //BUSMIN = int.Parse(item["BUSMIN"].ToString()),
                    //BUSSEC = int.Parse(item["BUSSEC"].ToString()),
                    //FIRSTBMPHR = int.Parse(item["FIRSTBMPHR"].ToString()),
                    //FIRSTBMPMN = int.Parse(item["FIRSTBMPMN"].ToString()),
                    //FIRSTBMPSC = int.Parse(item["FIRSTBMPSC"].ToString()),
                    //LASTBMPHR = int.Parse(item["LASTBMPHR"].ToString()),
                    //LASTBMPMN = int.Parse(item["LASTBMPMN"].ToString()),
                    //LASTBMPSC = int.Parse(item["LASTBMPSC"].ToString()),
                    //READYHOUR = int.Parse(item["READYHOUR"].ToString()),
                    //READYMIN = int.Parse(item["READYMIN"].ToString()),
                    //READYSEC = int.Parse(item["READYSEC"].ToString()),
                    //ASSIGNHOUR = int.Parse(item["ASSIGNHOUR"].ToString()),
                    //ASSIGNMIN = int.Parse(item["ASSIGNMIN"].ToString()),
                    //ASSIGNSEC = int.Parse(item["ASSIGNSEC"].ToString()),
                    //DRVOUTHOUR = int.Parse(item["DRVOUTHOUR"].ToString()),
                    //DRVOUTMIN = int.Parse(item["DRVOUTMIN"].ToString()),
                    //DRVOUTSEC = int.Parse(item["DRVOUTSEC"].ToString()),
                    //DRVINHOUR = int.Parse(item["DRVINHOUR"].ToString()),
                    //DRVINMIN = int.Parse(item["DRVINMIN"].ToString()),
                    //DRVINSEC = int.Parse(item["DRVINSEC"].ToString()),
                    //UNIT = int.Parse(item["UNIT"].ToString()),
                    DOB = DateTime.Parse(item["DOB"].ToString()),
                    //PROMISED = (item["PROMISED"].ToString()),
                    //CATEGORYID = int.Parse(item["CATEGORYID"].ToString()),
                    //MODEID = int.Parse(item["MODEID"].ToString()),
                    //FSTVIDID = int.Parse(item["FSTVIDID"].ToString()),
                    //LSTVIDID = int.Parse(item["LSTVIDID"].ToString()),
                    //STOREID = int.Parse(item["STOREID"].ToString()),
                    //OCCASION = int.Parse(item["OCCASION"].ToString()),
                    TERMID = int.Parse(item["TERMID"].ToString()),
                    QUEUEID = int.Parse(item["QUEUEID"].ToString()),
                   
                    CHECK = int.Parse(item["CHECK"].ToString())

                });
            }

            return lista;
        }
    }
}
