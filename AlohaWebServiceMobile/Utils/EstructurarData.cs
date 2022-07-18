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

        public EstructurarData()
        {
            pathALoha = AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder();
        }

        public List<MNUmobile> ObtenerMenus()
        {
            List<MNUmobile> Menus = new List<MNUmobile>();

            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                var MenuAloha = new MNUServicio(contextoAlh).GetAll();
                foreach (MNU menu in MenuAloha)
                {

                    MNUmobile mNUmobile = new MNUmobile();
                    mNUmobile.id_menu = menu.ID;
                    mNUmobile.descripcion_larga = menu.LONGNAME;
                    mNUmobile.descripcion_corta = menu.SHORTNAME;
                    #region iterar sobre submenus del menu
                    var Props = menu.GetType().GetProperties().ToList();
                    string MENU = "MENU";
                    int couter = 0;
                    int couterOrder = 1;
                    foreach (var propItem in Props.FindAll(p => p.Name.ToString().Contains("MENU")))
                    {
                        if ((int)propItem.GetValue(menu) > 0)
                        {
                            var name = propItem.Name;
                            SubMenu SubMenus = new SubMenu()
                            {
                                id = (int)propItem.GetValue(menu),
                            };

                            couterOrder++;
                            mNUmobile.subMenus.Add(SubMenus);
                        }
                    }
                    #endregion
                    Menus.Add(mNUmobile);
                }

            }


            return Menus;
        }
        public List<SUBmobile> ObtenerSubMenus()
        {
            List<SUBmobile> SubMenus = new List<SUBmobile>();
            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                var SubAloha = new SUBServicio(contextoAlh).GetAll();
                foreach (var sub in SubAloha)
                {
                    SubMenus.Add(new SUBmobile
                    {
                        id_submenu = sub.ID,
                        descripcion_corta = sub.SHORTNAME,
                        descripcion_larga = sub.LONGNAME,
                    });
                }

            }
            return SubMenus;
        }
        public List<ITMmobile> ObtenerItems()
        {

            List<ITMmobile> Items = new List<ITMmobile>();
            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                var ItemsAloha = new ITMServicio(contextoAlh).GetAll();
                foreach (var itm in ItemsAloha)
                {
                    Items.Add(new ITMmobile
                    {
                        id_item = itm.ID,
                        descripcion_corta = itm.SHORTNAME,
                        descripcion_larga = itm.LONGNAME
                    });
                }

            }
            return Items;
        }

        public List<MODmobile> ObtenerModificadores()
        {
            List<MODmobile> Mods = new List<MODmobile>();

            using (AplicacionBdContextoALH contextoALH = new AplicacionBdContextoALH(pathALoha))
            {
                var ModsAloha = new MODServicio(contextoALH).GetAll();
                foreach (var mod in ModsAloha)
                {
                    Mods.Add(new MODmobile
                    {
                        id_modificador = mod.ID,
                        descripcion_corta = mod.SHORTNAME,
                        descripcion_larga = mod.LONGNAME
                    });
                }
            }

            return Mods;
        }

        public List<ODRmobile> ObtenerModosDePedido()
        {
            List<ODRmobile> OrderModMobile = new List<ODRmobile>();

            using (AplicacionBdContextoALH contextoALH = new AplicacionBdContextoALH(pathALoha))
            {
                var ordenALHs = new OrdenServicioALH(contextoALH).GetAll();
                foreach (var odr in ordenALHs)
                {
                    OrderModMobile.Add(new ODRmobile
                    {
                        ID = odr.ID,
                        NAME = odr.NAME
                    });
                }
            }
            return OrderModMobile;
        }
        public List<TDRmobile> FormasDePago()
        {
            List<TDRmobile> TendersMobile = new List<TDRmobile>();

            using (AplicacionBdContextoALH contextoALH = new AplicacionBdContextoALH(pathALoha))
            {
                var TendersAloha = new TDRServicio(contextoALH).GetAll();
                foreach (var tdr in TendersAloha)
                {
                    TendersMobile.Add(new TDRmobile
                    {
                        id_forma_de_pago = tdr.ID,
                        descripcion = tdr.NAME,
                        status = (byte)(tdr.ACTIVE == AlohaLibrary.Enums.TipoLogicoALH.Y ? 1 : 0),
                    });
                }
            }


            return TendersMobile.OrderBy(t => t.id_forma_de_pago).ToList();
        }
    }
}
