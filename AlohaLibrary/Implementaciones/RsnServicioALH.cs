using AlohaLibrary.Contexto;
using AlohaLibrary.Enums;
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
    public class RsnServicioALH : ServicioBaseALH<RSN>, IRsn
    {
        public RsnServicioALH(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<RSN> GetAll()
        {
            List<RSN> lista = new List<RSN>();

            string query = $"SELECT * FROM RSN";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "RSN");
            DataTable tabla = ds.Tables["RSN"];

            foreach (DataRow item in tabla.Rows)
            {
                RSN Rsn = new RSN();
                var Props = Rsn.GetType().GetProperties().ToList();
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        //VALORES DOUBLE
                        double.TryParse(item[prop.Name].ToString(), out double result);
                        prop.SetValue(Rsn, result);
                    }
                    else
                    {
                        if (int.TryParse(item[prop.Name].ToString(), out int value))
                        {
                            //ENTEROS
                            prop.SetValue(Rsn, value);
                        }
                        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                        {
                            //BOOLEANO TIPO ALOHA
                            prop.SetValue(Rsn, item[prop.Name].ToString().ToUpper() == "Y");
                        }
                        else
                        {
                            //CADENAS
                            prop.SetValue(Rsn, item[prop.Name].ToString());
                        }
                    }
                }
                lista.Add(Rsn);
            }

            return lista;
        }
    }
}
