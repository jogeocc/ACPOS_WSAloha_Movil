using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Contexto;
using AlohaLibrary.Enums;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;

namespace AlohaLibrary.Implementaciones
{
    public class EmpServicio : ServicioBaseALH<EMP>, IEMP
    {
        public EmpServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<EMP> GetAll()
        {
            List<EMP> formasPago = new List<EMP>();

            string query = $"SELECT ID,OWNERID,USERNUMBER,SEC_NUM,SSN,SSNTEXT,FIRSTNAME,MIDDLENAME,LASTNAME,NICKNAME,JOBCODE1,TERMINATED FROM EMP";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "EMP");
            DataTable tabla = ds.Tables["EMP"];

            foreach (DataRow item in tabla.Rows)
            {
                formasPago.Add(new EMP()
                {
                    ID = int.Parse(item["ID"].ToString()),
                    OWNERID = int.Parse(item["OWNERID"].ToString()),
                    USERNUMBER = int.Parse(item["USERNUMBER"].ToString()),
                    SEC_NUM = int.Parse(item["SEC_NUM"].ToString()),
                    SSN = int.Parse(item["SSN"].ToString()),
                    SSNTEXT = (item["SSNTEXT"].ToString()),
                    FIRSTNAME = Reconvertir(item["FIRSTNAME"].ToString()),
                    MIDDLENAME = Reconvertir(item["MIDDLENAME"].ToString()),
                    LASTNAME = Reconvertir(item["LASTNAME"].ToString()),
                    NICKNAME = Reconvertir(item["NICKNAME"].ToString()),
                    JOBCODE1 = int.Parse((item["JOBCODE1"].ToString())),
                    TERMINATED = item["TERMINATED"].ToString().ToUpper().Equals("Y") ? TipoLogicoALH.Y : TipoLogicoALH.N
                });
            }

            return formasPago;
        }
    }
}
