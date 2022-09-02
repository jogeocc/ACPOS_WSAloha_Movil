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
    public class JOBServicio : ServicioBaseALH<JOB>, IJOB
    {
        public JOBServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<JOB> GetAll()
        {
            List<JOB> JobList = new List<JOB>();

            string query = "SELECT * FROM JOB";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "JOB");

            DataTable tabla = ds.Tables["JOB"];

            foreach (DataRow item in tabla.Rows)
            {
                JOB job = new JOB();


                var Props = job.GetType().GetProperties().ToList();
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        //VALORES DOUBLE
                        double.TryParse(item[prop.Name].ToString(), out double result);
                        prop.SetValue(job, result);
                    }
                    else
                    {
                        if (int.TryParse(item[prop.Name].ToString(), out int value))
                        {
                            //ENTEROS
                            prop.SetValue(job, value);
                        }
                        else if ((item[prop.Name].ToString().ToUpper() == "Y" || item[prop.Name].ToString().ToUpper() == "N"))
                        {
                            //BOOLEANO TIPO ALOHA
                            prop.SetValue(job, item[prop.Name].ToString().ToUpper() == "Y");
                        }
                        else
                        {
                            //CADENAS
                            prop.SetValue(job, item[prop.Name].ToString());
                        }
                    }
                }
                JobList.Add(job);
            }
            return JobList;
        }
    }
}
