using AlohaLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class JOB
    {
        public int ID { get; set; }
        public string SHORTNAME { get; set; }
        public string LONGNAME { get; set; }
        public TipoLogicoALH ORDERENTRY { get; set; }


        public int OWNERID { get; set; }
        public string USERNUMBER { get; set; }
        public TipoLogicoALH DECLWAGES { get; set; }
        public TipoLogicoALH PAYTIPSHAR { get; set; }
        public TipoLogicoALH SHIFTREQD { get; set; }
        public TipoLogicoALH BARTENDER { get; set; }
        //2 ONLY TABLES / 1 TABLES AND TABS / 0 ONLY TABS
        public int TABS { get; set; }
        public TipoLogicoALH TRAINING { get; set; }
        public TipoLogicoALH PASSWORD { get; set; }
        public TipoLogicoALH ORDERTAKER { get; set; }
        public TipoLogicoALH CASHIER { get; set; }
        public int GROUP { get; set; }
        public int ICON { get; set; }
        public TipoLogicoALH NOSELFXFER { get; set; }
        public TipoLogicoALH NOCLOSE { get; set; }
        public TipoLogicoALH GETCHECK { get; set; }
        public TipoLogicoALH CASHDRAWER { get; set; }
        public TipoLogicoALH DECLCASH { get; set; }
        public TipoLogicoALH SENDLOCAL { get; set; }
        public TipoLogicoALH SENDBAR { get; set; }
        public TipoLogicoALH BLIND { get; set; }
        public int ONLYMODE { get; set; }
        public TipoLogicoALH EXPORT { get; set; }
        public TipoLogicoALH SELFVOID { get; set; }
        public TipoLogicoALH NOPRINT { get; set; }
        public TipoLogicoALH NOSCHED { get; set; }
        public TipoLogicoALH NOCASH { get; set; }
        public int DEFSCRN { get; set; }
        public TipoLogicoALH INDRTIP { get; set; }
        public TipoLogicoALH PAIDBRK { get; set; }
        public TipoLogicoALH UNPAIDBK { get; set; }
        public TipoLogicoALH MGRCKOUT { get; set; }
        public TipoLogicoALH DRIVER { get; set; }
        public TipoLogicoALH SELFBANK { get; set; }
        public TipoLogicoALH SELFDWR { get; set; }
        public TipoLogicoALH AUTORETURN { get; set; }
        public TipoLogicoALH NOTIPS { get; set; }
        public int ORDERQUEUE { get; set; }
        public TipoLogicoALH TIPOUT { get; set; }
        public TipoLogicoALH PIVOTSEAT { get; set; }
        public TipoLogicoALH NOFLASH { get; set; }
        public TipoLogicoALH REPRNTCHK { get; set; }
        public TipoLogicoALH BRKOPEN { get; set; }
        public TipoLogicoALH ITEMLOOK { get; set; }
        public TipoLogicoALH GOTOCLOSE { get; set; }
        public TipoLogicoALH NODEFSCR { get; set; }
        public TipoLogicoALH DRVSLFASGN { get; set; }
        public TipoLogicoALH DRVTRCKMLG { get; set; }
        public TipoLogicoALH DISPATCHER { get; set; }
        public TipoLogicoALH DRDRTAKER { get; set; }
        public TipoLogicoALH REQTABNAME { get; set; }
        public int REVCENTER { get; set; }
        public TipoLogicoALH MUSTDECL { get; set; }
        public TipoLogicoALH RECON { get; set; }
        public int ATTEMPT { get; set; }
        public TipoLogicoALH PWDEXP { get; set; }
        public TipoLogicoALH PWDEXPDAYS { get; set; }
        public TipoLogicoALH STARTPMS { get; set; }
        public TipoLogicoALH FORCEPMS { get; set; }
        public TipoLogicoALH TEAMCHECK { get; set; }
        public int TEAMLABOR { get; set; }
        public TipoLogicoALH ENTERBANK { get; set; }
        public TipoLogicoALH USECOUNT { get; set; }
        public TipoLogicoALH MUTLIPEDWR { get; set; }
        public TipoLogicoALH RAPACCESS { get; set; }
        public TipoLogicoALH NOAUTOORDR { get; set; }
        public TipoLogicoALH ENABLEFPI { get; set; }
        public TipoLogicoALH GETCHKFPI { get; set; }
        public int DEFSCRNTS { get; set; }
        public TipoLogicoALH THRESHMSG { get; set; }
        public TipoLogicoALH MGRCLKOUT { get; set; }
        public TipoLogicoALH EQUALPAY { get; set; }
        public TipoLogicoALH MEALBREAKS { get; set; }
        public TipoLogicoALH RESTBREAKS { get; set; }
        public TipoLogicoALH INCLABCST { get; set; }
        public TipoLogicoALH INCLABHRS { get; set; }
        public double AVGCOST { get; set; }
        public int AVGCOSTTYP { get; set; }
        public int MODESCRQS { get; set; }
        public int MODESCRTS { get; set; }
        public TipoLogicoALH USEVIZOR { get; set; }
        public TipoLogicoALH DWRTOCLOSE { get; set; }
        public TipoLogicoALH ALLOWADITM { get; set; }
        public int VOIDREASON { get; set; }
        public TipoLogicoALH ENFORCEBRK { get; set; }
        public int BRKMINUTES { get; set; }
        public TipoLogicoALH AUDITCPN { get; set; }
        public int AUDITATT { get; set; }
        public TipoLogicoALH ASKOMODE { get; set; }
        public double TIPTHRESHD { get; set; }
        public int TIPTHRSMSG { get; set; }
        public TipoLogicoALH LIMPAIDBRK { get; set; }
        public int LIMPAIDMIN { get; set; }
        public TipoLogicoALH ENFPAIDBRK { get; set; }
        public int ENFPDBMINS { get; set; }
        public int SCRTIMEOUT { get; set; }
        public int WAIVEBRKAT { get; set; }
        public int WAIVEBKMSG { get; set; }
        public int THRESHTYPE { get; set; }
        public int OVERRDFUNC { get; set; }
        public TipoLogicoALH NOCHKOUT { get; set; }
        public TipoLogicoALH ASSUMEOWN { get; set; }
        public TipoLogicoALH FASTXFER { get; set; }
        public TipoLogicoALH AUTOACCEPT { get; set; }
        public TipoLogicoALH NOCLKINOUT { get; set; }
        public TipoLogicoALH JITNOCLKIN { get; set; }
        public TipoLogicoALH RECTIPSHAR { get; set; }
        public int BREAKTYPE { get; set; }
        public double MXTIPTHRSH { get; set; }
        public TipoLogicoALH GLOBALUSER { get; set; }
        public TipoLogicoALH OCCASION { get; set; }
        public TipoLogicoALH NOADJCLCHK { get; set; }
        public TipoLogicoALH AUTODECL { get; set; }
        public TipoLogicoALH REPORTAS { get; set; }
        public TipoLogicoALH JOBLIQCERT { get; set; }
        public int JOBLIQDAYS { get; set; }
        public TipoLogicoALH EXITCLOSE { get; set; }
        public TipoLogicoALH INTERFACE { get; set; }
        public TipoLogicoALH CONTIGUOUS { get; set; }
        public TipoLogicoALH ALLTDRSREC { get; set; }
        public TipoLogicoALH TRNSFCLOSE { get; set; }
        public TipoLogicoALH PNCHADJACK { get; set; }
        public TipoLogicoALH MGRDECLACK { get; set; }
        public TipoLogicoALH SUPPRESSCH { get; set; }
        public TipoLogicoALH INTERFCEMP { get; set; }
    }
}
