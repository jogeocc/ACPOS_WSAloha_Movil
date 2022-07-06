using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.ModelosPersonalizados
{
    public class SaleDiscount
    {
        public int CHECK { get; set; }
        public int PERIOD { get; set; }
        public int ITEMID { get; set; }
        public decimal PRICE { get; set; }
        public decimal AMT { get; set; }
        public decimal Tax1 { get; set; }
        public decimal Tax2 { get; set; }
        public decimal VTax { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal DescuentoSinIva { get; set; }
    }
}
