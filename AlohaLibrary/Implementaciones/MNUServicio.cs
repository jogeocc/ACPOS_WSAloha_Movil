using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlohaLibrary.Contexto;
using AlohaLibrary.Infraestrutura;
using AlohaLibrary.Interfaces;
using AlohaLibrary.Modelos;

namespace AlohaLibrary.Implementaciones
{
    public class MNUServicio : ServicioBaseALH<MNU>, IMNU
    {
        public MNUServicio(AplicacionBdContextoALH contexto) : base(contexto)
        {
        }
        public override List<MNU> GetAll()
        {
            List<MNU> mnus = new List<MNU>();

            string query = "SELECT * FROM MNU";

            DataSet ds = new DataSet();
            EjecutarConsulta(query).Fill(ds, "MNU");
            DataTable tabla = ds.Tables["MNU"];

            foreach (DataRow item in tabla.Rows)
            {
                mnus.Add(new MNU()
                {
                    ID = int.Parse(item["ID"].ToString()),
                    LONGNAME = (item["LONGNAME"].ToString()),
                    SHORTNAME = (item["SHORTNAME"].ToString()),
                    MENU01 = int.Parse(item["MENU01"].ToString()),
                    MENU02 = int.Parse(item["MENU02"].ToString()),
                    MENU03 = int.Parse(item["MENU03"].ToString()),
                    MENU04 = int.Parse(item["MENU04"].ToString()),
                    MENU05 = int.Parse(item["MENU05"].ToString()),
                    MENU06 = int.Parse(item["MENU06"].ToString()),
                    MENU07 = int.Parse(item["MENU07"].ToString()),
                    MENU08 = int.Parse(item["MENU08"].ToString()),
                    MENU09 = int.Parse(item["MENU09"].ToString()),
                    MENU10 = int.Parse(item["MENU10"].ToString()),
                    MENU11 = int.Parse(item["MENU11"].ToString()),
                    MENU12 = int.Parse(item["MENU12"].ToString()),
                    MENU13 = int.Parse(item["MENU13"].ToString()),
                    MENU14 = int.Parse(item["MENU14"].ToString()),
                    MENU15 = int.Parse(item["MENU15"].ToString()),
                    MENU16 = int.Parse(item["MENU16"].ToString()),
                    MENU17 = int.Parse(item["MENU17"].ToString()),
                    MENU18 = int.Parse(item["MENU18"].ToString()),
                    MENU19 = int.Parse(item["MENU19"].ToString()),
                    MENU20 = int.Parse(item["MENU20"].ToString()),
                    MENU21 = int.Parse(item["MENU21"].ToString()),
                    MENU22 = int.Parse(item["MENU22"].ToString()),
                    MENU23 = int.Parse(item["MENU23"].ToString()),
                    MENU24 = int.Parse(item["MENU24"].ToString()),
                    MENU25 = int.Parse(item["MENU25"].ToString()),
                    MENU26 = int.Parse(item["MENU26"].ToString()),
                    MENU27 = int.Parse(item["MENU27"].ToString()),
                    MENU28 = int.Parse(item["MENU28"].ToString()),
                    MENU29 = int.Parse(item["MENU29"].ToString()),
                    MENU30 = int.Parse(item["MENU30"].ToString()),
                    MENU31 = int.Parse(item["MENU31"].ToString()),
                    MENU32 = int.Parse(item["MENU32"].ToString()),
                    MENU33 = int.Parse(item["MENU33"].ToString()),
                    MENU34 = int.Parse(item["MENU34"].ToString()),
                    MENU35 = int.Parse(item["MENU35"].ToString()),
                    MENU36 = int.Parse(item["MENU36"].ToString()),
                    MENU37 = int.Parse(item["MENU37"].ToString()),
                    MENU38 = int.Parse(item["MENU38"].ToString()),
                    MENU39 = int.Parse(item["MENU39"].ToString()),
                    MENU40 = int.Parse(item["MENU40"].ToString()),
                    MENU41 = int.Parse(item["MENU41"].ToString()),
                    MENU42 = int.Parse(item["MENU42"].ToString()),
                    MENU43 = int.Parse(item["MENU43"].ToString()),
                    MENU44 = int.Parse(item["MENU44"].ToString()),
                    MENU45 = int.Parse(item["MENU45"].ToString()),
                    MENU46 = int.Parse(item["MENU46"].ToString()),
                    MENU47 = int.Parse(item["MENU47"].ToString()),
                    MENU48 = int.Parse(item["MENU48"].ToString()),
                    MENU49 = int.Parse(item["MENU49"].ToString()),
                    MENU50 = int.Parse(item["MENU50"].ToString()),
                    MENU51 = int.Parse(item["MENU51"].ToString()),
                    MENU52 = int.Parse(item["MENU52"].ToString()),
                    MENU53 = int.Parse(item["MENU53"].ToString()),
                    MENU54 = int.Parse(item["MENU54"].ToString()),
                    MENU55 = int.Parse(item["MENU55"].ToString()),
                    MENU56 = int.Parse(item["MENU56"].ToString()),
                    MENU57 = int.Parse(item["MENU57"].ToString()),
                    MENU58 = int.Parse(item["MENU58"].ToString()),
                    MENU59 = int.Parse(item["MENU59"].ToString()),
                    MENU60 = int.Parse(item["MENU60"].ToString()),
                    MENU61 = int.Parse(item["MENU61"].ToString()),
                    MENU62 = int.Parse(item["MENU62"].ToString()),
                    MENU63 = int.Parse(item["MENU63"].ToString()),
                    MENU64 = int.Parse(item["MENU64"].ToString()),
                    MENU65 = int.Parse(item["MENU65"].ToString()),
                    MENU66 = int.Parse(item["MENU66"].ToString()),
                    MENU67 = int.Parse(item["MENU67"].ToString()),
                    MENU68 = int.Parse(item["MENU68"].ToString()),
                    MENU69 = int.Parse(item["MENU69"].ToString()),
                    MENU70 = int.Parse(item["MENU70"].ToString()),
                    MENU71 = int.Parse(item["MENU71"].ToString()),
                    MENU72 = int.Parse(item["MENU72"].ToString()),
                    MENU73 = int.Parse(item["MENU73"].ToString()),
                    MENU74 = int.Parse(item["MENU74"].ToString()),
                    MENU75 = int.Parse(item["MENU75"].ToString()),
                    MENU76 = int.Parse(item["MENU76"].ToString()),
                    MENU77 = int.Parse(item["MENU77"].ToString()),
                    MENU78 = int.Parse(item["MENU78"].ToString()),
                    MENU79 = int.Parse(item["MENU79"].ToString()),
                    MENU80 = int.Parse(item["MENU80"].ToString()),
                    MENU81 = int.Parse(item["MENU81"].ToString()),
                    MENU82 = int.Parse(item["MENU82"].ToString()),
                    MENU83 = int.Parse(item["MENU83"].ToString()),
                    MENU84 = int.Parse(item["MENU84"].ToString()),
                    MENU85 = int.Parse(item["MENU85"].ToString()),
                    MENU86 = int.Parse(item["MENU86"].ToString()),
                    MENU87 = int.Parse(item["MENU87"].ToString()),
                    MENU88 = int.Parse(item["MENU88"].ToString()),
                    MENU89 = int.Parse(item["MENU89"].ToString()),
                    MENU90 = int.Parse(item["MENU90"].ToString()),
                    MENU91 = int.Parse(item["MENU91"].ToString()),
                    MENU92 = int.Parse(item["MENU92"].ToString()),
                    MENU93 = int.Parse(item["MENU93"].ToString()),
                    MENU94 = int.Parse(item["MENU94"].ToString()),
                    MENU95 = int.Parse(item["MENU95"].ToString()),
                    MENU96 = int.Parse(item["MENU96"].ToString()),
                    MENU97 = int.Parse(item["MENU97"].ToString()),
                    MENU98 = int.Parse(item["MENU98"].ToString()),

                });
            }


            return mnus;
        }
    }
}
