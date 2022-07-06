using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Enums;

namespace AlohaLibrary.Modelos
{
    public class PRO
    {
        /// DATOS GENERALES DE CADA PROMOCION
        public int ID { get; set; }

        public int KIND { get; set; }
        public TipoLogicoALH ACTIVE { get; set; }
        public DateTime? STARTDATE { get; set; } = null;
        public DateTime? ENDDATE { get; set; } = null;
        public string NAME { get; set; }
        public double MAXIMUM { get; set; }

        //PROMOCION BOGO

        /// <summary>
        /// Categoria de items requeridos
        /// </summary>
        public int BOGOITMS { get; set; }
        /// <summary>
        /// Cantidad de articulos minimos requeridos
        /// </summary>
        public int BOGONUMITM { get; set; }
        /// <summary>
        /// CATEGORIA DE ITEMS A DESCONTAR
        /// </summary>
        public int BOGODISCAT { get; set; }
        public int BOGOMETHOD { get; set; }
        public int BOGODISRST { get; set; }
        public double BOGOFIXPRI { get; set; }
        public double BOGOPERCNT { get; set; }
        public int BOGOMODS { get; set; }
        public TipoLogicoALH BOGOQAA { get; set; }

        ///Promocion COMBO ¡Este no tiene AutoApply¡

        public string COMBONAME { get; set; }
        public double COMBOPRICE { get; set; }
        public int COMBOMODS { get; set; }
        public int COMBOMIN01 { get; set; }
        public int COMBOMIN02 { get; set; }
        public int COMBOMIN03 { get; set; }
        public int COMBOMIN04 { get; set; }
        public int COMBOMIN05 { get; set; }
        public int COMBOMIN06 { get; set; }
        public int COMBOMIN07 { get; set; }
        public int COMBOMIN08 { get; set; }
        public int COMBOMIN09 { get; set; }
        public int COMBOMIN10 { get; set; }
        public int COMBOMAX01 { get; set; }
        public int COMBOMAX02 { get; set; }
        public int COMBOMAX03 { get; set; }
        public int COMBOMAX04 { get; set; }
        public int COMBOMAX05 { get; set; }
        public int COMBOMAX06 { get; set; }
        public int COMBOMAX07 { get; set; }
        public int COMBOMAX08 { get; set; }
        public int COMBOMAX09 { get; set; }
        public int COMBOMAX10 { get; set; }
        public int COMBOCAT01 { get; set; }
        public int COMBOCAT02 { get; set; }
        public int COMBOCAT03 { get; set; }
        public int COMBOCAT04 { get; set; }
        public int COMBOCAT05 { get; set; }
        public int COMBOCAT06 { get; set; }
        public int COMBOCAT07 { get; set; }
        public int COMBOCAT08 { get; set; }
        public int COMBOCAT09 { get; set; }
        public int COMBOCAT10 { get; set; }
        public string CMPTNAME01 { get; set; }
        public string CMPTNAME02 { get; set; }
        public string CMPTNAME03 { get; set; }
        public string CMPTNAME04 { get; set; }
        public string CMPTNAME05 { get; set; }
        public string CMPTNAME06 { get; set; }
        public string CMPTNAME07 { get; set; }
        public string CMPTNAME08 { get; set; }
        public string CMPTNAME09 { get; set; }
        public string CMPTNAME10 { get; set; }

        ///PROMOCION CUPON

        public int CPITMS { get; set; }

        public int CPMODS { get; set; }
        public int CPMUSTITMS { get; set; }
        public int CPMUSTCNT { get; set; }
        public TipoLogicoALH CPPERCENT { get; set; }
        public double CPAMOUNT { get; set; }
        public int CPLIMIT { get; set; }
        public TipoLogicoALH CPAA { get; set; }

        //Promocion tipo de nuevo precio
        public int NPMUSTITMS { get; set; }
        public int NPMUSTCNT { get; set; }
        public int NPFREEMODS { get; set; }
        public int NPLIMIT { get; set; }
        public int NPITEM01 { get; set; }
        public int NPITEM02 { get; set; }
        public int NPITEM03 { get; set; }
        public int NPITEM04 { get; set; }
        public int NPITEM05 { get; set; }
        public int NPITEM06 { get; set; }
        public int NPITEM07 { get; set; }
        public int NPITEM08 { get; set; }
        public int NPITEM09 { get; set; }
        public int NPITEM10 { get; set; }
        public double NPPRICE01 { get; set; }
        public double NPPRICE02 { get; set; }
        public double NPPRICE03 { get; set; }
        public double NPPRICE04 { get; set; }
        public double NPPRICE05 { get; set; }
        public double NPPRICE06 { get; set; }
        public double NPPRICE07 { get; set; }
        public double NPPRICE08 { get; set; }
        public double NPPRICE09 { get; set; }
        public double NPPRICE10 { get; set; }
        public TipoLogicoALH NPAUTOAPPY { get; set; }
        //PROMOCION TIPO REDUCCION DE CUENTA

        public int RXITEMS { get; set; }
        public TipoLogicoALH RXQUALIFY { get; set; }
        public int RXQUALITMS { get; set; }
        public double RXMINTOTAL { get; set; }
        public TipoLogicoALH RXPERCENT { get; set; }
        public double RXAMOUNT { get; set; }
        public TipoLogicoALH RXAUTOAPPY { get; set; }
    }
}
