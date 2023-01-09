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
    public class GCIServicio : ServicioBaseALH<GCI>, IGCI
    {
        public GCIServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<GCI> GetAll()
        {
            List<GCI> List = new List<GCI>();
            string query = $"SELECT * FROM QTYPRICE";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "QTYPRICE");
            DataTable tabla = ds.Tables["QTYPRICE"];
            foreach (DataRow item in tabla.Rows)
            {
                GCI MessageCheck = new GCI();

                var Props = MessageCheck.GetType().GetProperties().ToList();
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        //VALORES DOUBLE
                        double.TryParse(item[prop.Name].ToString(), out double result);
                        prop.SetValue(MessageCheck, result);
                    }
                    else
                    {
                        if (int.TryParse(item[prop.Name].ToString(), out int value))
                        {
                            //ENTEROS
                            prop.SetValue(MessageCheck, value);
                        }
                        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                        {
                            //BOOLEANO TIPO ALOHA
                            prop.SetValue(MessageCheck, item[prop.Name].ToString().ToUpper() == "Y");
                        }
                        else
                        {
                            //CADENAS
                            prop.SetValue(MessageCheck, item[prop.Name].ToString());
                        }
                    }
                }
                List.Add(MessageCheck);
            }
            return List;
        }
    }
}
