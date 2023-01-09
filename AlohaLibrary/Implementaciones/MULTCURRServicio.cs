using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class MULTCURRServicio : ServicioBaseALH<MULTCURR>, IMULTCURR
    {
        public MULTCURRServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<MULTCURR> GetAll()
        {
            List<MULTCURR> List = new List<MULTCURR>();
            string query = $"SELECT * FROM QTYPRICE";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "QTYPRICE");
            DataTable tabla = ds.Tables["QTYPRICE"];
            foreach (DataRow item in tabla.Rows)
            {
                MULTCURR Currency = new MULTCURR();

                var Props = Currency.GetType().GetProperties().ToList();
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        //VALORES DOUBLE
                        double.TryParse(item[prop.Name].ToString(), out double result);
                        prop.SetValue(Currency, result);
                    }
                    else
                    {
                        if (int.TryParse(item[prop.Name].ToString(), out int value))
                        {
                            //ENTEROS
                            prop.SetValue(Currency, value);
                        }
                        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                        {
                            //BOOLEANO TIPO ALOHA
                            prop.SetValue(Currency, item[prop.Name].ToString().ToUpper() == "Y");
                        }
                        else
                        {
                            //CADENAS
                            prop.SetValue(Currency, item[prop.Name].ToString());
                        }
                    }
                }
                List.Add(Currency);
            }
            return List;
        }
    }
}
