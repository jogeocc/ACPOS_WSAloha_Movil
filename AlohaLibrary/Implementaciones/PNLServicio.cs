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
    public class PNLServicio : ServicioBaseALH<PNL>, IPNL
    {
        public PNLServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<PNL> GetAll()
        {
            List<PNL> List = new List<PNL>();

            string query = $"SELECT * FROM PNL";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "PNL");
            DataTable tabla = ds.Tables["PNL"];

            foreach (DataRow item in tabla.Rows)
            {
                PNL Panel = new PNL();

                var Props = Panel.GetType().GetProperties().ToList();
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        //VALORES DOUBLE
                        double.TryParse(item[prop.Name].ToString(), out double result);
                        prop.SetValue(Panel, result);
                    }
                    else
                    {
                        if (int.TryParse(item[prop.Name].ToString(), out int value))
                        {
                            //ENTEROS
                            prop.SetValue(Panel, value);
                        }
                        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                        {
                            //BOOLEANO TIPO ALOHA
                            prop.SetValue(Panel, item[prop.Name].ToString().ToUpper() == "Y");
                        }
                        else
                        {
                            //CADENAS
                            prop.SetValue(Panel, item[prop.Name].ToString());
                        }
                    }
                }
                List.Add(Panel);
            }


            return List;
        }
    }
}
