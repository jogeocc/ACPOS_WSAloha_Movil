using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary;

namespace AlohaWebServiceMobile.Models.Catalogos
{
    public class JOBmobile
    {
        public int id { get; set; }
        public string descripcion_corta { get; set; }
        public string descripcion_larga { get; set; }



        public int OWNERID { get; set; }
        public string USERNUMBER { get; set; }
        public bool ORDERENTRY { get; set; }
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
        public bool EXPORT { get; set; }
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
        public bool PWDEXPDAYS { get; set; }
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
        public string AVGCOST { get; set; }
        public string AVGCOSTTYP { get; set; }
        public string MODESCRQS { get; set; }
        public string MODESCRTS { get; set; }
        public string USEVIZOR { get; set; }
        public string DWRTOCLOSE { get; set; }
        public string ALLOWADITM { get; set; }
        public string VOIDREASON { get; set; }
        public string ENFORCEBRK { get; set; }
        public string BRKMINUTES { get; set; }
        public string AUDITCPN { get; set; }
        public string AUDITATT { get; set; }
        public string ASKOMODE { get; set; }
        public string TIPTHRESHD { get; set; }
        public string TIPTHRSMSG { get; set; }
        public string LIMPAIDBRK { get; set; }
        public string LIMPAIDMIN { get; set; }
        public string ENFPAIDBRK { get; set; }
        public string ENFPDBMINS { get; set; }
        public string SCRTIMEOUT { get; set; }
        public string WAIVEBRKAT { get; set; }
        public string WAIVEBKMSG { get; set; }
        public string THRESHTYPE { get; set; }
        public string OVERRDFUNC { get; set; }
        public string NOCHKOUT { get; set; }
        public string ASSUMEOWN { get; set; }
        public string FASTXFER { get; set; }
        public string AUTOACCEPT { get; set; }
        public string NOCLKINOUT { get; set; }
        public string JITNOCLKIN { get; set; }
        public string RECTIPSHAR { get; set; }
        public string BREAKTYPE { get; set; }
        public string MXTIPTHRSH { get; set; }
        public string GLOBALUSER { get; set; }
        public string OCCASION { get; set; }
        public string NOADJCLCHK { get; set; }
        public string AUTODECL { get; set; }
        public string REPORTAS { get; set; }
        public string JOBLIQCERT { get; set; }
        public string JOBLIQDAYS { get; set; }
        public string EXITCLOSE { get; set; }
        public string INTERFACE { get; set; }
        public string CONTIGUOUS { get; set; }
        public string ALLTDRSREC { get; set; }
        public string TRNSFCLOSE { get; set; }
        public string PNCHADJACK { get; set; }
        public string MGRDECLACK { get; set; }
        public string SUPPRESSCH { get; set; }
        public string INTERFCEMP { get; set; }



    }
}
