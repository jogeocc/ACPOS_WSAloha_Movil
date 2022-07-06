using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaLibrary.Implementaciones
{
    public class TABServicio : ServicioBaseALH<TAB>, ITAB
    {
        public TABServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }

        public override List<TAB> GetAll()
        {
            List<TAB> ListTab = new List<TAB>();

            string query = "SELECT * FROM TAB";
            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "TAB");
            DataTable tabla = ds.Tables["TAB"];

            foreach (DataRow item in tabla.Rows)
            {
                ListTab.Add(new TAB
                {
                    DESC = item["DESC"].ToString(),
                    ID = int.Parse(item["ID"].ToString()),
                    LOOKUPNAME = int.Parse(item["LOOKUPNAME"].ToString()),
                    NUMSEATS = int.Parse(item["NUMSEATS"].ToString()),
                    OWNERID = int.Parse(item["OWNERID"].ToString()),
                    REVCENTER = int.Parse(item["REVCENTER"].ToString()),
                    STYLE = int.Parse(item["STYLE"].ToString()),
                    USERNUMBER = int.Parse(item["USERNUMBER"].ToString()),
                });
            }


            return ListTab;
        }
    }
}
