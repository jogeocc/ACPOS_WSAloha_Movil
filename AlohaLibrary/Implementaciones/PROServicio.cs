using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Enums;
using AlohaLibrary.Modelos;
using AlohaLibrary.Interfaces;
using System.Data;
using AlohaLibrary.Contexto;

namespace AlohaLibrary.Implementaciones
{
    public class PROServicio :ServicioBaseALH<PRO>, IPROServicio
    {
        public PROServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<PRO> GetAll()
        {
            List<PRO> pros = new List<PRO>();
            string query = "SELECT * FROM PRO order by ID";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "PRO");

            DataTable tabla = ds.Tables["PRO"];

            foreach (DataRow item in tabla.Rows)
            {
                PRO promocion = new PRO();
                promocion.ID = int.Parse(item["ID"].ToString());
                promocion.KIND = int.Parse(item["KIND"].ToString());

                //TODO TEMPORAL HASTA TENER TODAS LAS PROMOS

                promocion.ACTIVE = item["ACTIVE"].ToString().ToUpper().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N;
                bool success = DateTime.TryParse(item["STARTDATE"].ToString(), out DateTime fecha);
                if (success) promocion.STARTDATE = fecha;
                success = DateTime.TryParse(item["ENDDATE"].ToString(), out fecha);
                if (success) promocion.ENDDATE = fecha;
                promocion.NAME = item["NAME"].ToString();
                promocion.MAXIMUM = double.Parse(item["MAXIMUM"].ToString());

                switch (promocion.KIND)
                {
                    //BOGO
                    case 1:
                        promocion.BOGOITMS = int.Parse(item["BOGOITMS"].ToString());
                        promocion.BOGONUMITM = int.Parse(item["BOGONUMITM"].ToString());
                        promocion.BOGODISCAT = int.Parse(item["BOGODISCAT"].ToString());
                        promocion.BOGOMETHOD = int.Parse(item["BOGOMETHOD"].ToString());
                        promocion.BOGODISRST = int.Parse(item["BOGODISRST"].ToString());
                        promocion.BOGOFIXPRI = double.Parse(item["BOGOFIXPRI"].ToString());
                        promocion.BOGOPERCNT = double.Parse(item["BOGOPERCNT"].ToString());
                        promocion.BOGOMODS = int.Parse(item["BOGOMODS"].ToString());
                        promocion.BOGOQAA = item["BOGOQAA"].ToString().ToUpper().Equals("Y")
                            ? TipoLogicoALH.Y
                            : TipoLogicoALH.N;
                        break;
                    case 2:
                        #region combos
                        promocion.COMBONAME = item["COMBONAME"].ToString();
                        promocion.COMBOPRICE = double.Parse(item["COMBOPRICE"].ToString());
                        promocion.COMBOMODS = int.Parse(item["COMBOMODS"].ToString());
                        promocion.COMBOMIN01 = int.Parse(item["COMBOMIN01"].ToString());
                        promocion.COMBOMIN02 = int.Parse(item["COMBOMIN02"].ToString());
                        promocion.COMBOMIN03 = int.Parse(item["COMBOMIN03"].ToString());
                        promocion.COMBOMIN04 = int.Parse(item["COMBOMIN04"].ToString());
                        promocion.COMBOMIN05 = int.Parse(item["COMBOMIN05"].ToString());
                        promocion.COMBOMIN06 = int.Parse(item["COMBOMIN06"].ToString());
                        promocion.COMBOMIN07 = int.Parse(item["COMBOMIN07"].ToString());
                        promocion.COMBOMIN08 = int.Parse(item["COMBOMIN08"].ToString());
                        promocion.COMBOMIN09 = int.Parse(item["COMBOMIN09"].ToString());
                        promocion.COMBOMIN10 = int.Parse(item["COMBOMIN10"].ToString());
                        promocion.COMBOMAX01 = int.Parse(item["COMBOMAX01"].ToString());
                        promocion.COMBOMAX02 = int.Parse(item["COMBOMAX02"].ToString());
                        promocion.COMBOMAX03 = int.Parse(item["COMBOMAX03"].ToString());
                        promocion.COMBOMAX04 = int.Parse(item["COMBOMAX04"].ToString());
                        promocion.COMBOMAX05 = int.Parse(item["COMBOMAX05"].ToString());
                        promocion.COMBOMAX06 = int.Parse(item["COMBOMAX06"].ToString());
                        promocion.COMBOMAX07 = int.Parse(item["COMBOMAX07"].ToString());
                        promocion.COMBOMAX08 = int.Parse(item["COMBOMAX08"].ToString());
                        promocion.COMBOMAX09 = int.Parse(item["COMBOMAX09"].ToString());
                        promocion.COMBOMAX10 = int.Parse(item["COMBOMAX10"].ToString());
                        promocion.COMBOCAT01 = int.Parse(item["COMBOCAT01"].ToString());
                        promocion.COMBOCAT02 = int.Parse(item["COMBOCAT02"].ToString());
                        promocion.COMBOCAT03 = int.Parse(item["COMBOCAT03"].ToString());
                        promocion.COMBOCAT04 = int.Parse(item["COMBOCAT04"].ToString());
                        promocion.COMBOCAT05 = int.Parse(item["COMBOCAT05"].ToString());
                        promocion.COMBOCAT06 = int.Parse(item["COMBOCAT06"].ToString());
                        promocion.COMBOCAT07 = int.Parse(item["COMBOCAT07"].ToString());
                        promocion.COMBOCAT08 = int.Parse(item["COMBOCAT08"].ToString());
                        promocion.COMBOCAT09 = int.Parse(item["COMBOCAT09"].ToString());
                        promocion.COMBOCAT10 = int.Parse(item["COMBOCAT10"].ToString());
                        promocion.CMPTNAME01 = item["CMPTNAME01"].ToString();
                        promocion.CMPTNAME02 = item["CMPTNAME02"].ToString();
                        promocion.CMPTNAME03 = item["CMPTNAME03"].ToString();
                        promocion.CMPTNAME04 = item["CMPTNAME04"].ToString();
                        promocion.CMPTNAME05 = item["CMPTNAME05"].ToString();
                        promocion.CMPTNAME06 = item["CMPTNAME06"].ToString();
                        promocion.CMPTNAME07 = item["CMPTNAME07"].ToString();
                        promocion.CMPTNAME08 = item["CMPTNAME08"].ToString();
                        promocion.CMPTNAME09 = item["CMPTNAME09"].ToString();
                        promocion.CMPTNAME10 = item["CMPTNAME10"].ToString();
                        break;
                    #endregion

                    case 3:
                        promocion.CPITMS = int.Parse(item["CPITMS"].ToString());
                        promocion.CPMODS = int.Parse(item["CPMODS"].ToString());
                        promocion.CPMUSTITMS = int.Parse(item["CPMUSTITMS"].ToString());
                        promocion.CPMUSTCNT = int.Parse(item["CPMUSTCNT"].ToString());
                        promocion.CPPERCENT = item["CPPERCENT"].ToString().ToUpper().Equals("Y")
                            ? TipoLogicoALH.Y
                            : TipoLogicoALH.N;
                        promocion.CPAMOUNT = double.Parse(item["CPAMOUNT"].ToString());
                        promocion.CPLIMIT = int.Parse(item["CPLIMIT"].ToString());
                        promocion.CPAA = item["CPAA"].ToString().ToUpper().Equals("Y")
                            ? TipoLogicoALH.Y
                            : TipoLogicoALH.N;
                        break;
                    case 4:
                        promocion.NPMUSTITMS = int.Parse(item["NPMUSTITMS"].ToString());
                        promocion.NPMUSTCNT = int.Parse(item["NPMUSTCNT"].ToString());
                        promocion.NPFREEMODS = int.Parse(item["NPFREEMODS"].ToString());
                        promocion.NPLIMIT = int.Parse(item["NPLIMIT"].ToString());
                        promocion.NPITEM01 = int.Parse(item["NPITEM01"].ToString());
                        promocion.NPITEM02 = int.Parse(item["NPITEM02"].ToString());
                        promocion.NPITEM03 = int.Parse(item["NPITEM03"].ToString());
                        promocion.NPITEM04 = int.Parse(item["NPITEM04"].ToString());
                        promocion.NPITEM05 = int.Parse(item["NPITEM05"].ToString());
                        promocion.NPITEM06 = int.Parse(item["NPITEM06"].ToString());
                        promocion.NPITEM07 = int.Parse(item["NPITEM07"].ToString());
                        promocion.NPITEM08 = int.Parse(item["NPITEM08"].ToString());
                        promocion.NPITEM09 = int.Parse(item["NPITEM09"].ToString());
                        promocion.NPITEM10 = int.Parse(item["NPITEM10"].ToString());
                        promocion.NPPRICE01 = double.Parse(item["NPPRICE01"].ToString());
                        promocion.NPPRICE02 = double.Parse(item["NPPRICE02"].ToString());
                        promocion.NPPRICE03 = double.Parse(item["NPPRICE03"].ToString());
                        promocion.NPPRICE04 = double.Parse(item["NPPRICE04"].ToString());
                        promocion.NPPRICE05 = double.Parse(item["NPPRICE05"].ToString());
                        promocion.NPPRICE06 = double.Parse(item["NPPRICE06"].ToString());
                        promocion.NPPRICE07 = double.Parse(item["NPPRICE07"].ToString());
                        promocion.NPPRICE08 = double.Parse(item["NPPRICE08"].ToString());
                        promocion.NPPRICE09 = double.Parse(item["NPPRICE09"].ToString());
                        promocion.NPPRICE10 = double.Parse(item["NPPRICE10"].ToString());
                        promocion.NPAUTOAPPY = item["NPAUTOAPPY"].ToString().ToUpper().Equals("Y")
                            ? TipoLogicoALH.Y
                            : TipoLogicoALH.N;
                        break;
                    case 5:
                        promocion.RXITEMS =
                            int.Parse(item["RXITEMS"].ToString());
                        promocion.RXQUALIFY =
                            item["CPPERCENT"].ToString().ToUpper().Equals("Y")
                                ? TipoLogicoALH.Y
                                : TipoLogicoALH.N;
                        promocion.RXQUALITMS =
                            int.Parse(item["RXQUALITMS"].ToString());
                        promocion.RXMINTOTAL =
                            double.Parse(item["RXMINTOTAL"].ToString());
                        promocion.RXPERCENT =
                            item["RXPERCENT"].ToString().ToUpper().Equals("Y")
                                ? TipoLogicoALH.Y
                                : TipoLogicoALH.N;
                        ;
                        promocion.RXAMOUNT =
                            double.Parse(item["RXAMOUNT"].ToString());
                        ;
                        promocion.RXAUTOAPPY =
                            item["RXAUTOAPPY"].ToString().ToUpper().Equals("Y")
                                ? TipoLogicoALH.Y
                                : TipoLogicoALH.N;
                        ;
                        break;
                }

                pros.Add(promocion);
            }

            return pros;
        }
    }
}
