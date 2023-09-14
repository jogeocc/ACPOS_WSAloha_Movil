using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Utils
{
    public class GeneralFunctions
    {
        public void ReadDbf<T>(DataRow ItemDbf, ref T ItemModelo)
        {

            var Props = ItemModelo.GetType().GetProperties().ToList();
            foreach (PropertyInfo prop in Props)
            {
                if (prop.PropertyType == typeof(double))
                {
                    //VALORES DOUBLE
                    double.TryParse(ItemDbf[prop.Name].ToString(), out double result);
                    prop.SetValue(ItemModelo, result);
                }
                else
                {
                    if (prop.PropertyType == typeof(int))
                    {
                        //ENTEROS
                        int.TryParse(ItemDbf[prop.Name].ToString(), out int value);

                        prop.SetValue(ItemModelo, value);
                    }
                    else if ((ItemDbf[prop.Name].ToString().ToUpper() == "Y" || ItemDbf[prop.Name].ToString().ToUpper() == "N"))
                    {
                        //BOOLEANO TIPO ALOHA
                        prop.SetValue(ItemModelo, ItemDbf[prop.Name].ToString().ToUpper() == "Y");
                    }
                    else
                    {
                        //CADENAS
                        prop.SetValue(ItemModelo, ItemDbf[prop.Name].ToString());
                    }
                }
            }
        }
    }
}
