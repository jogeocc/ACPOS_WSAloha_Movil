using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using AlohaLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class BTNServicio : ServicioBaseALH<BTN>, IBTN
    {
        public BTNServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<BTN> GetAll()
        {
            List<BTN> JobList = new List<BTN>();

            string query = "SELECT * FROM BTN";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "BTN");

            DataTable tabla = ds.Tables["BTN"];

            foreach (DataRow item in tabla.Rows)
            {
                BTN BTNITEM = new BTN();

                new GeneralFunctions().ReadDbf(item, ref BTNITEM);

                //var Props = BTNITEM.GetType().GetProperties().ToList();
                //foreach (PropertyInfo prop in Props)
                //{
                //    if (prop.PropertyType == typeof(double))
                //    {
                //        //VALORES DOUBLE
                //        double.TryParse(item[prop.Name].ToString(), out double result);
                //        prop.SetValue(BTNITEM, result);
                //    }
                //    else
                //    {
                //        if (int.TryParse(item[prop.Name].ToString(), out int value) && prop.PropertyType != typeof(string))
                //        {
                //            //ENTEROS
                //            prop.SetValue(BTNITEM, value);
                //        }
                //        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                //        {
                //            //BOOLEANO TIPO ALOHA
                //            prop.SetValue(BTNITEM, item[prop.Name].ToString().ToUpper() == "Y");
                //        }
                //        else
                //        {
                //            //CADENAS
                //            prop.SetValue(BTNITEM, item[prop.Name].ToString());
                //        }
                //    }
                //}
                JobList.Add(BTNITEM);
            }
            return JobList;
        }
    }
}
