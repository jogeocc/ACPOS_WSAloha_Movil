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
    public class MODCODEServicio : ServicioBaseALH<MODCODE>, IMODCODE
    {
        public MODCODEServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<MODCODE> GetAll()
        {
            List<MODCODE> List = new List<MODCODE>();

            string query = $"SELECT * FROM MODCODE";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "MODCODE");
            DataTable tabla = ds.Tables["MODCODE"];

            foreach (DataRow item in tabla.Rows)
            {
                MODCODE ModeCode = new MODCODE();

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
