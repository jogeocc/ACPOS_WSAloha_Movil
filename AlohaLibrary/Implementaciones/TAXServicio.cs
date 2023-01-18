using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;

namespace AlohaLibrary.Implementaciones
{
    public class TAXServicio : ServicioBaseALH<TAX>, ITAX
    {
        public TAXServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<TAX> GetAll()
        {
            List<TAX> List = new List<TAX>();
            string query = $"SELECT * FROM QTYPRICE";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "QTYPRICE");
            DataTable tabla = ds.Tables["QTYPRICE"];
            foreach (DataRow item in tabla.Rows)
            {
                TAX priceItem = new TAX();

                var Props = priceItem.GetType().GetProperties().ToList();
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        //VALORES DOUBLE
                        double.TryParse(item[prop.Name].ToString(), out double result);
                        prop.SetValue(priceItem, result);
                    }
                    else
                    {
                        if (int.TryParse(item[prop.Name].ToString(), out int value))
                        {
                            //ENTEROS
                            prop.SetValue(priceItem, value);
                        }
                        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                        {
                            //BOOLEANO TIPO ALOHA
                            prop.SetValue(priceItem, item[prop.Name].ToString().ToUpper() == "Y");
                        }
                        else
                        {
                            //CADENAS
                            prop.SetValue(priceItem, item[prop.Name].ToString());
                        }
                    }
                }
                List.Add(priceItem);
            }
            return List;
        }
    }
}
