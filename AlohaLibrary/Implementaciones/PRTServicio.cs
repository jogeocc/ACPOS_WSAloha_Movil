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
    public class PRTServicio : ServicioBaseALH<PRT>, IPRT
    {
        public PRTServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<PRT> GetAll()
        {
            List<PRT> list = new List<PRT>();

            string query = $"SELECT * FROM PRT";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "QTYPRICE");
            DataTable tabla = ds.Tables["QTYPRICE"];
            foreach (DataRow item in tabla.Rows)
            {
                PRT Printer = new PRT();

                var Props = Printer.GetType().GetProperties().ToList();
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        //VALORES DOUBLE
                        double.TryParse(item[prop.Name].ToString(), out double result);
                        prop.SetValue(Printer, result);
                    }
                    else
                    {
                        if (int.TryParse(item[prop.Name].ToString(), out int value))
                        {
                            //ENTEROS
                            prop.SetValue(Printer, value);
                        }
                        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                        {
                            //BOOLEANO TIPO ALOHA
                            prop.SetValue(Printer, item[prop.Name].ToString().ToUpper() == "Y");
                        }
                        else
                        {
                            //CADENAS
                            prop.SetValue(Printer, item[prop.Name].ToString());
                        }
                    }
                }
                list.Add(Printer);
            }

            return list;
        }
    }
}
