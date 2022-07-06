using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Modelos
{
    public class MOD
    {
        public int ID { get; set; }
        public string SHORTNAME { get; set; }
        public string LONGNAME { get; set; }
        public int MINIMUM { get; set; }
        public int MAXIMUM { get; set; }
        public int FREE { get; set; }

        public List<int> items { get; set; } = new List<int>();
        public List<double> precios { get; set; } = new List<double>();
        public List<int> methods { get; set; } = new List<int>();


        //// IDs items de los asociados
        //public int ITEM01 { get; set; }
        //public int ITEM02 { get; set; }
        //public int ITEM03 { get; set; }
        //public int ITEM04 { get; set; }
        //public int ITEM05 { get; set; }
        //public int ITEM06 { get; set; }
        //public int ITEM07 { get; set; }
        //public int ITEM08 { get; set; }
        //public int ITEM09 { get; set; }
        //public int ITEM10 { get; set; }
        //public int ITEM11 { get; set; }
        //public int ITEM12 { get; set; }
        //public int ITEM13 { get; set; }
        //public int ITEM14 { get; set; }
        //public int ITEM15 { get; set; }
        //public int ITEM16 { get; set; }
        //public int ITEM17 { get; set; }
        //public int ITEM18 { get; set; }
        //public int ITEM19 { get; set; }
        //public int ITEM20 { get; set; }
        //public int ITEM21 { get; set; }
        //public int ITEM22 { get; set; }
        //public int ITEM23 { get; set; }
        //public int ITEM24 { get; set; }
        //public int ITEM25 { get; set; }
        //public int ITEM26 { get; set; }
        //public int ITEM27 { get; set; }
        //public int ITEM28 { get; set; }
        //public int ITEM29 { get; set; }
        //public int ITEM30 { get; set; }
        //public int ITEM31 { get; set; }
        //public int ITEM32 { get; set; }
        //public int ITEM33 { get; set; }
        //public int ITEM34 { get; set; }
        //public int ITEM35 { get; set; }
        //public int ITEM36 { get; set; }
        //public int ITEM37 { get; set; }
        //public int ITEM38 { get; set; }
        //public int ITEM39 { get; set; }
        //public int ITEM40 { get; set; }
        //public int ITEM41 { get; set; }
        //public int ITEM42 { get; set; }
        //public int ITEM43 { get; set; }
        //public int ITEM44 { get; set; }
        //public int ITEM45 { get; set; }
        //public int ITEM46 { get; set; }
        //public int ITEM47 { get; set; }
        //public int ITEM48 { get; set; }
        //public int ITEM49 { get; set; }
        //public int ITEM50 { get; set; }
        //public int ITEM51 { get; set; }
        //public int ITEM52 { get; set; }
        //public int ITEM53 { get; set; }
        //public int ITEM54 { get; set; }

        ////precios de referecia
        //public double PRICE01 { get; set; }
        //public double PRICE02 { get; set; }
        //public double PRICE03 { get; set; }
        //public double PRICE04 { get; set; }
        //public double PRICE05 { get; set; }
        //public double PRICE06 { get; set; }
        //public double PRICE07 { get; set; }
        //public double PRICE08 { get; set; }
        //public double PRICE09 { get; set; }
        //public double PRICE10 { get; set; }
        //public double PRICE11 { get; set; }
        //public double PRICE12 { get; set; }
        //public double PRICE13 { get; set; }
        //public double PRICE14 { get; set; }
        //public double PRICE15 { get; set; }
        //public double PRICE16 { get; set; }
        //public double PRICE17 { get; set; }
        //public double PRICE18 { get; set; }
        //public double PRICE19 { get; set; }
        //public double PRICE20 { get; set; }
        //public double PRICE21 { get; set; }
        //public double PRICE22 { get; set; }
        //public double PRICE23 { get; set; }
        //public double PRICE24 { get; set; }
        //public double PRICE25 { get; set; }
        //public double PRICE26 { get; set; }
        //public double PRICE27 { get; set; }
        //public double PRICE28 { get; set; }
        //public double PRICE29 { get; set; }
        //public double PRICE30 { get; set; }
        //public double PRICE31 { get; set; }
        //public double PRICE32 { get; set; }
        //public double PRICE33 { get; set; }
        //public double PRICE34 { get; set; }
        //public double PRICE35 { get; set; }
        //public double PRICE36 { get; set; }
        //public double PRICE37 { get; set; }
        //public double PRICE38 { get; set; }
        //public double PRICE39 { get; set; }
        //public double PRICE40 { get; set; }
        //public double PRICE41 { get; set; }
        //public double PRICE42 { get; set; }
        //public double PRICE43 { get; set; }
        //public double PRICE44 { get; set; }
        //public double PRICE45 { get; set; }
        //public double PRICE46 { get; set; }
        //public double PRICE47 { get; set; }
        //public double PRICE48 { get; set; }
        //public double PRICE49 { get; set; }
        //public double PRICE50 { get; set; }
        //public double PRICE51 { get; set; }
        //public double PRICE52 { get; set; }
        //public double PRICE53 { get; set; }

        //public double PRICE54 { get; set; }

        ////condicion para saber el precio
        //public int PRMETHOD01 { get; set; }
        //public int PRMETHOD02 { get; set; }
        //public int PRMETHOD03 { get; set; }
        //public int PRMETHOD04 { get; set; }
        //public int PRMETHOD05 { get; set; }
        //public int PRMETHOD06 { get; set; }
        //public int PRMETHOD07 { get; set; }
        //public int PRMETHOD08 { get; set; }
        //public int PRMETHOD09 { get; set; }
        //public int PRMETHOD10 { get; set; }
        //public int PRMETHOD11 { get; set; }
        //public int PRMETHOD12 { get; set; }
        //public int PRMETHOD13 { get; set; }
        //public int PRMETHOD14 { get; set; }
        //public int PRMETHOD15 { get; set; }
        //public int PRMETHOD16 { get; set; }
        //public int PRMETHOD17 { get; set; }
        //public int PRMETHOD18 { get; set; }
        //public int PRMETHOD19 { get; set; }
        //public int PRMETHOD20 { get; set; }
        //public int PRMETHOD21 { get; set; }
        //public int PRMETHOD22 { get; set; }
        //public int PRMETHOD23 { get; set; }
        //public int PRMETHOD24 { get; set; }
        //public int PRMETHOD25 { get; set; }
        //public int PRMETHOD26 { get; set; }
        //public int PRMETHOD27 { get; set; }
        //public int PRMETHOD28 { get; set; }
        //public int PRMETHOD29 { get; set; }
    }
}
