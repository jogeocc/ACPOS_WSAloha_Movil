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
    public class MODEXTServicio : ServicioBaseALH<MODEXT>, IMODEXT
    {
        public MODEXTServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<MODEXT> GetAll()
        {
            List<MODEXT> List = new List<MODEXT>();

            string query = $"SELECT * FROM MODEXT";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "MODEXT");
            DataTable tabla = ds.Tables["MODEXT"];

            foreach (DataRow item in tabla.Rows)
            {
                MODEXT ModeCode = new MODEXT();

                var Props = ModeCode.GetType().GetProperties().ToList();
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        //VALORES DOUBLE
                        double.TryParse(item[prop.Name].ToString(), out double result);
                        prop.SetValue(ModeCode, result);
                    }
                    else
                    {
                        if (int.TryParse(item[prop.Name].ToString(), out int value))
                        {
                            //ENTEROS
                            prop.SetValue(ModeCode, value);
                        }
                        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                        {
                            //BOOLEANO TIPO ALOHA
                            prop.SetValue(ModeCode, item[prop.Name].ToString().ToUpper() == "Y");
                        }
                        else
                        {
                            //CADENAS
                            prop.SetValue(ModeCode, item[prop.Name].ToString());
                        }
                    }
                }
                List.Add(ModeCode);
            }


            return List;
        }
    }
}
