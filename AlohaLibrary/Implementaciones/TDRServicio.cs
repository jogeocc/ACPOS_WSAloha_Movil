using AlohaLibrary.Contexto;
using AlohaLibrary.Enums;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using AlohaLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class TDRServicio : ServicioBaseALH<TDR>, ITDRServicio
    {
        public TDRServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<TDR> GetAll()
        {
            List<TDR> Lista = new List<TDR>();
            string query = "SELECT * FROM TDR";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "TDR");
            DataTable tabla = ds.Tables["TDR"];
            foreach (DataRow item in tabla.Rows)
            {
                TDR Tdr = new TDR();

                new GeneralFunctions().ReadDbf(item, ref Tdr);

                //var Props = Tdr.GetType().GetProperties().ToList();
                //foreach (PropertyInfo prop in Props)
                //{
                //    Console.WriteLine(prop.Name);

                //    if (prop.PropertyType == typeof(double))
                //    {
                //        //VALORES DOUBLE
                //        double.TryParse(item[prop.Name].ToString(), out double result);
                //        prop.SetValue(Tdr, result);
                //    }
                //    else
                //    {
                //        //if (int.TryParse(item[prop.Name].ToString(), out int value) && prop.PropertyType == typeof(string))
                //        if  (prop.PropertyType == typeof(int))
                //        {
                //            //ENTEROS
                //            int.TryParse(item[prop.Name].ToString(), out int value);
                //            prop.SetValue(Tdr, value);
                //        }
                //        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                //        {
                //            //BOOLEANO TIPO ALOHA
                //            prop.SetValue(Tdr, item[prop.Name].ToString().ToUpper() == "Y");
                //        }
                //        else
                //        {
                //            //CADENAS
                //            prop.SetValue(Tdr, item[prop.Name].ToString());
                //        }
                //    }
                //}
                Lista.Add(Tdr);
            }
            return Lista;
        }
    }
}
