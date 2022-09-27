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
    public class ACCServicio : ServicioBaseALH<ACC>, IACC
    {
        public ACCServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<ACC> GetAll()
        {

            List<ACC> List = new List<ACC>();
            string query = $"SELECT * FROM ACC";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "ACC");
            DataTable tabla = ds.Tables["ACC"];
            foreach (DataRow ACCES in tabla.Rows)
            {
                ACC Acceso = new ACC();

                var Props = Acceso.GetType().GetProperties().ToList();
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        //VALORES DOUBLE
                        double.TryParse(ACCES[prop.Name].ToString(), out double result);
                        prop.SetValue(Acceso, result);
                    }
                    else
                    {
                        if (int.TryParse(ACCES[prop.Name].ToString(), out int value))
                        {
                            //ENTEROS
                            prop.SetValue(Acceso, value);
                        }
                        else if ((ACCES[prop.Name].ToString().ToUpper() == "Y" || ACCES[prop.Name].ToString().ToUpper() == "N"))
                        {
                            //BOOLEANO TIPO ALOHA
                            prop.SetValue(Acceso, ACCES[prop.Name].ToString().ToUpper() == "Y");
                        }
                        else
                        {
                            //CADENAS
                            prop.SetValue(Acceso, ACCES[prop.Name].ToString());
                        }
                    }
                }
                List.Add(Acceso);
            }
            return List;
        }
    }
}
