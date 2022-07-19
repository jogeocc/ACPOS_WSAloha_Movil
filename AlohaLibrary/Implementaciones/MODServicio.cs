using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using AlohaLibrary.Contexto;
using System.Data;

namespace AlohaLibrary.Implementaciones
{
    public class MODServicio : ServicioBaseALH<MOD>, IMOD
    {
        public MODServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<MOD> GetAll()
        {
            List<MOD> mods = new List<MOD>();
            List<string> items = new List<string>();
            List<string> precios = new List<string>();
            List<string> methods = new List<string>();
            string auxITEM = "ITEM";
            string auxPrecio = "PRICE";
            string auxMethods = "PRMETHOD";
            for (int i = 0; i < 54; i++)
            {
                items.Add(auxITEM + (i + 1).ToString().PadLeft(2, '0'));
                precios.Add(auxPrecio + (i + 1).ToString().PadLeft(2, '0'));
                methods.Add(auxMethods + (i + 1).ToString().PadLeft(2, '0'));

                //if (i + 1 <= 29)
                //{
                //}
            }

            auxPrecio = string.Join(",", precios);
            auxITEM = string.Join(",", items);
            auxMethods = string.Join(",", methods);

            string query =
                $"SELECT ID,SHORTNAME,LONGNAME,MINIMUM, MAXIMUM,FREE, {auxITEM},{auxPrecio},{auxMethods} FROM MOD";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "MOD");

            DataTable tabla = ds.Tables["MOD"];

            auxITEM = "ITEM";
            auxPrecio = "PRICE";
            auxMethods = "PRMETHOD";

            foreach (DataRow item in tabla.Rows)
            {
                MOD mod = new MOD();
                mod.ID = int.Parse(item["ID"].ToString());
                mod.SHORTNAME = item["SHORTNAME"].ToString();
                mod.LONGNAME = item["LONGNAME"].ToString();
                mod.MINIMUM = int.Parse(item["MINIMUM"].ToString());
                mod.MAXIMUM = int.Parse(item["MAXIMUM"].ToString());
                mod.FREE = int.Parse(item["FREE"].ToString());
                for (int i = 0; i < 54; i++)
                {
                    string itemAux = auxITEM + (i + 1).ToString().PadLeft(2, '0');
                    string precioAux = auxPrecio + (i + 1).ToString().PadLeft(2, '0');
                    string methodAux = auxMethods + (i + 1).ToString().PadLeft(2, '0');
                    mod.methods.Add(int.Parse(item[methodAux].ToString()));
                    mod.items.Add(int.Parse(item[itemAux].ToString()));
                    mod.precios.Add(decimal.Parse(item[precioAux].ToString()));
                }

                mods.Add(mod);
            }

            return mods;
        }
    }
}
