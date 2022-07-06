using AlohaLibrary.Contexto;
using AlohaLibrary.Implementaciones;
using AlohaLibrary.Modelos;
using AlohaWebServiceMobile.Models.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Utils
{
    public class EstructurarData
    {
        string pathALoha = @"D:\PROYECTOS\VAPIANO\reforma\Data";

        public List<MNUmobile> ObtenerMenus()
        {
            List<MNUmobile> Menus = new List<MNUmobile>();

            //try
            //{
            //    pathALoha = AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder();
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception);
            //}

            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                var MenuAloha = new MNUServicio(contextoAlh).GetAll();
                foreach (MNU menu in MenuAloha)
                {
                    Menus.Add(new MNUmobile
                    {
                        id_menu = menu.ID,
                        descripcion_corta = menu.SHORTNAME,
                        descripcion_larga = menu.LONGNAME,
                    });
                }
            }


            return Menus;
        }
        public object ObtenerSubMenus()
        {
            return 1;

        }
        public object ObtenerItems()
        {
            return 1;
        }

        public object ObtenerModificadores()
        {
            return 1;
        }

        public object ObtenerModosDePedido()
        {
            return 1;
        }
        public object FormasDePago()
        {
            return 1;
        }
    }
}
