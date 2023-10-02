using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.Models.Aloha.Catalogos
{
    [JsonObject]
    public class JOBmobile
    {
        public int id { get; set; }
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }



        public bool ORDERENTRY { get; set; }
        public int OWNERID { get; set; }
        public int USERNUMBER { get; set; }
        public bool DECLWAGES { get; set; }
        public bool PAYTIPSHAR { get; set; }
        public bool SHIFTREQD { get; set; }
        public bool BARTENDER { get; set; }
        //2 ONLY TABLES / 1 TABLES AND TABS / 0 ONLY TABS
        public int TABS { get; set; }
        public bool TRAINING { get; set; }
        public bool PASSWORD { get; set; }
        public bool ORDERTAKER { get; set; }
        public bool CASHIER { get; set; }
        public int GROUP { get; set; }
        public int ICON { get; set; }
        public bool NOSELFXFER { get; set; }
        public bool NOCLOSE { get; set; }
        public bool GETCHECK { get; set; }
        public bool CASHDRAWER { get; set; }
        public bool DECLCASH { get; set; }
        public bool SENDLOCAL { get; set; }
        public bool SENDBAR { get; set; }
        public bool BLIND { get; set; }
        public int ONLYMODE { get; set; }
        public string EXPORT { get; set; }
        public bool SELFVOID { get; set; }
        public bool NOPRINT { get; set; }
        public bool NOSCHED { get; set; }
        public bool NOCASH { get; set; }
        public int DEFSCRN { get; set; }
        public bool INDRTIP { get; set; }
        public bool PAIDBRK { get; set; }
        public bool UNPAIDBK { get; set; }
        public bool MGRCKOUT { get; set; }
        public bool DRIVER { get; set; }
        public bool SELFBANK { get; set; }
        public bool SELFDWR { get; set; }
        public bool AUTORETURN { get; set; }
        public bool NOTIPS { get; set; }
        public int ORDERQUEUE { get; set; }
        public bool TIPOUT { get; set; }
        public bool PIVOTSEAT { get; set; }
        public bool NOFLASH { get; set; }
        public bool REPRNTCHK { get; set; }
        public bool BRKOPEN { get; set; }
        public bool ITEMLOOK { get; set; }
        public bool GOTOCLOSE { get; set; }
        public bool NODEFSCR { get; set; }
        public bool DRVSLFASGN { get; set; }
        public bool DRVTRCKMLG { get; set; }
        public bool DISPATCHER { get; set; }
        public bool DRDRTAKER { get; set; }
        public bool REQTABNAME { get; set; }
        public int REVCENTER { get; set; }
        public bool MUSTDECL { get; set; }
        public bool RECON { get; set; }
        public int ATTEMPT { get; set; }
        public bool PWDEXP { get; set; }
        public int PWDEXPDAYS { get; set; }
        public bool STARTPMS { get; set; }
        public bool FORCEPMS { get; set; }
        public bool TEAMCHECK { get; set; }
        public int TEAMLABOR { get; set; }
        public bool ENTERBANK { get; set; }
        public bool USECOUNT { get; set; }
        public bool MUTLIPEDWR { get; set; }
        public bool RAPACCESS { get; set; }
        public bool NOAUTOORDR { get; set; }
        public bool ENABLEFPI { get; set; }
        public bool GETCHKFPI { get; set; }
        public int DEFSCRNTS { get; set; }
        public bool THRESHMSG { get; set; }
        public bool MGRCLKOUT { get; set; }
        public bool EQUALPAY { get; set; }
        public bool MEALBREAKS { get; set; }
        public bool RESTBREAKS { get; set; }
        public bool INCLABCST { get; set; }
        public bool INCLABHRS { get; set; }
        public double AVGCOST { get; set; }
        public int AVGCOSTTYP { get; set; }
        public int MODESCRQS { get; set; }
        public int MODESCRTS { get; set; }
        public bool USEVIZOR { get; set; }
        public bool DWRTOCLOSE { get; set; }
        public bool ALLOWADITM { get; set; }
        public int VOIDREASON { get; set; }
        public bool ENFORCEBRK { get; set; }
        public int BRKMINUTES { get; set; }
        public bool AUDITCPN { get; set; }
        public int AUDITATT { get; set; }
        public bool ASKOMODE { get; set; }
        public double TIPTHRESHD { get; set; }
        public int TIPTHRSMSG { get; set; }
        public bool LIMPAIDBRK { get; set; }
        public int LIMPAIDMIN { get; set; }
        public bool ENFPAIDBRK { get; set; }
        public int ENFPDBMINS { get; set; }
        public int SCRTIMEOUT { get; set; }
        public int WAIVEBRKAT { get; set; }
        public int WAIVEBKMSG { get; set; }
        public int THRESHTYPE { get; set; }
        public int OVERRDFUNC { get; set; }
        public bool NOCHKOUT { get; set; }
        public bool ASSUMEOWN { get; set; }
        public bool FASTXFER { get; set; }
        public bool AUTOACCEPT { get; set; }
        public bool NOCLKINOUT { get; set; }
        public bool JITNOCLKIN { get; set; }
        public bool RECTIPSHAR { get; set; }
        public int BREAKTYPE { get; set; }
        public double MXTIPTHRSH { get; set; }
        public bool GLOBALUSER { get; set; }
        public bool OCCASION { get; set; }
        public bool NOADJCLCHK { get; set; }
        public bool AUTODECL { get; set; }
        public bool REPORTAS { get; set; }
        public bool JOBLIQCERT { get; set; }
        public int JOBLIQDAYS { get; set; }
        public bool EXITCLOSE { get; set; }
        public bool INTERFACE { get; set; }
        public bool CONTIGUOUS { get; set; }
        public bool ALLTDRSREC { get; set; }
        public bool TRNSFCLOSE { get; set; }
        public bool PNCHADJACK { get; set; }
        public bool MGRDECLACK { get; set; }
        public bool SUPPRESSCH { get; set; }
        public bool INTERFCEMP { get; set; }
    }
}
