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
    public class GNDSALEServicio : ServicioBaseALH<GNDSale>, IGNDSALEServicio
    {
        public GNDSALEServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<GNDSale> GetAll()
        {
            List<GNDSale> lista = new List<GNDSale>();

            string query = $"SELECT EMPLOYEE,CHECK,PERIOD,TYPE,TYPEID,AMOUNT,OPENHOUR,OPENMIN,ORDERHOUR,ORDERMIN,CLOSEHOUR,CLOSEMIN,SHIFT,COUNT,REVENUE,TIPEMP,UNIT,DOB,TYPEID2,OCCASION,REVID2,TIPEMPSHFT,DRAWERID FROM GNDSale";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "GNDSale");
            DataTable tabla = ds.Tables["GNDSale"];


            foreach (DataRow item in tabla.Rows)
            {
                var employee = int.Parse(item["EMPLOYEE"].ToString());
                var check = int.Parse(item["CHECK"].ToString());
                var period = int.Parse(item["PERIOD"].ToString());
                var type = int.Parse(item["TYPE"].ToString());
                var typeId = int.Parse(item["TYPEID"].ToString());
                var amount = decimal.Parse(item["AMOUNT"].ToString());
                var openHour = int.Parse(item["OPENHOUR"].ToString());
                var openMin = int.Parse(item["OPENMIN"].ToString());
                var orderHour = int.Parse(item["ORDERHOUR"].ToString());
                var orderMin = int.Parse(item["ORDERMIN"].ToString());
                var closeHour = (item["CLOSEHOUR"] == null || item["CLOSEHOUR"].ToString().Trim()=="") ?0:int.Parse(item["CLOSEHOUR"].ToString());
                var closeMin = int.Parse(item["CLOSEMIN"].ToString());
                var shift = int.Parse(item["SHIFT"].ToString());
                var count = int.Parse(item["COUNT"].ToString());
                var revenue = int.Parse(item["REVENUE"].ToString());
                var tipEmp = int.Parse(item["TIPEMP"].ToString());
                var unit = int.Parse(item["UNIT"].ToString());
                var dob = DateTime.Parse(item["DOB"].ToString());
                var typeId2 = int.Parse(item["TYPEID2"].ToString());
                var occasion = int.Parse(item["OCCASION"].ToString());
                var revid2 = int.Parse(item["REVID2"].ToString());
                var tipEmpShift = int.Parse(item["TIPEMPSHFT"].ToString());
                var drawerId = int.Parse(item["DRAWERID"].ToString());

                lista.Add(new GNDSale
                {
                    EMPLOYEE = employee,
                    CHECK = check,
                    PERIOD = period,
                    TYPE = type,
                    TYPEID = typeId,
                    AMOUNT = amount,
                    OPENHOUR = openHour,
                    OPENMIN = openMin,
                    ORDERHOUR = orderHour,
                    ORDERMIN = orderMin,
                    CLOSEHOUR = closeHour,
                    CLOSEMIN = closeMin,
                    SHIFT = shift,
                    COUNT = count,
                    REVENUE = revenue,
                    TIPEMP = tipEmp,
                    UNIT = unit,
                    DOB = dob,
                    TYPEID2 = typeId2,
                    OCCASION = occasion,
                    REVID2 = revid2,
                    TIPEMPSHFT = tipEmpShift,
                    DRAWERID = drawerId,

                });
            }

            return lista;
        }

        public List<Sale> GetSales()
        {
            List<Sale> lista = new List<Sale>();

            string query = "SELECT s.EMPLOYEE, s.CHECK, s.PERIOD, t.TYPE, t.TYPEID, s.AMOUNT, t.TIP " +
                "FROM (" +
                "SELECT * FROM GNDSALE" +
                "FROM GNDTNDR t " +
                "INNER JOIN (" +
                "SELECT EMPLOYEE, CHECK, PERIOD, TYPE, TYPEID, AMOUNT " +
                "FROM GNDSALE " +
                "WHERE TYPE = 44) s " +
                "ON t.CHECK = s.CHECK";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "Sale");
            DataTable tabla = ds.Tables["Sale"];

            foreach (DataRow item in tabla.Rows)
            {
                var employee = int.Parse(item["EMPLOYEE"].ToString());
                var check = int.Parse(item["CHECK"].ToString());
                var period = int.Parse(item["PERIOD"].ToString());
                var type = int.Parse(item["TYPE"].ToString());
                var typeId = int.Parse(item["TYPEID"].ToString());
                var amount = decimal.Parse(item["AMOUNT"].ToString());

                lista.Add(new Sale
                {
                    EMPLOYEE = employee,
                    CHECK = check,
                    PERIOD = period,
                    TYPE = type,
                    TYPEID = typeId,
                    AMOUNT = amount,
                    TIP = decimal.Parse(item["TIP"].ToString()),
                });
            }

            return lista;
        }

        public List<SaleDiscount> GetSalesDiscounts()
        {
            List<SaleDiscount> lista = new List<SaleDiscount>();

            string query = "SELECT s.CHECK, s.PERIOD, l.ITEMID, l.PRICE, l.AMT, i.TAXID, i.TAXID2, i.VTAXID " +
                "FROM ((GNDSALE s " +
                "INNER JOIN GNDLINE l " +
                "ON s.CHECK = l.CHECKID) " +
                "INNER JOIN ITM i " +
                "ON l.ITEMID = i.ID) " +
                "WHERE s.TYPE = 5 OR s.TYPE = 6";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "Sale");
            DataTable tabla = ds.Tables["Sale"];

            TAXServicio taxServicio = new TAXServicio(Contexto);
            var taxes = taxServicio.GetAll();

            //foreach (DataRow item in tabla.Rows)
            //{
            //    int TAXID1 = int.Parse(item["TAXID"].ToString());
            //    int TAXID2 = int.Parse(item["TAXID2"].ToString());
            //    int VTAXID = int.Parse(item["VTAXID"].ToString());

            //    decimal tax1 = TAXID1 != 0 ? taxes.Find(t => t.ID == TAXID1).RATE : 0;
            //    decimal tax2 = TAXID2 != 0 ? taxes.Find(t => t.ID == TAXID2).RATE : 0;
            //    decimal vTax = VTAXID != 0 ? taxes.Find(t => t.ID == VTAXID).RATE : 0;
            //    decimal amt = decimal.Parse(item["AMT"].ToString());

            //    lista.Add(new SaleDiscount
            //    {
            //        CHECK = int.Parse(item["CHECK"].ToString()),
            //        PERIOD = int.Parse(item["PERIOD"].ToString()),
            //        ITEMID = int.Parse(item["ITEMID"].ToString()),
            //        PRICE = decimal.Parse(item["PRICE"].ToString()),
            //        AMT = amt,
            //        Tax1 = tax1,
            //        Tax2 = tax2,
            //        VTax = vTax,
            //        TaxTotal = tax1 + tax2 + vTax,
            //        DescuentoSinIva = amt / (tax1 + tax2 + vTax + 1)
            //    });
            //}

            return lista;
        }

        public DataTable GetSalesDiscounts2()
        {
            List<SaleDiscount> lista = new List<SaleDiscount>();

            string query = "SELECT s.CHECK, s.PERIOD, l.ITEMID, l.PRICE, l.AMT " +
                "FROM ((GNDSALE s " +
                "INNER JOIN GNDLINE l " +
                "ON s.CHECK = l.CHECKID) " +
                "INNER JOIN ITM i " +
                "ON l.ITEMID = i.ID) " +
                "WHERE ";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "Sale");
            return ds.Tables["Sale"];
        }
    }
}
