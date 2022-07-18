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

        public List<MNUmobile> ObtenerMenuMovil()
        {
            List<MNUmobile> Menus = new List<MNUmobile>();

            List<MNU> Menus2 = new List<MNU>();
            List<SUB> SubMenus = new List<SUB>();
            List<ITM> Items = new List<ITM>();
            List<MOD> Mods = new List<MOD>();
            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                Menus2 = new MNUServicio(contextoAlh).GetAll();
                SubMenus = new SUBServicio(contextoAlh).GetAll();
                Items = new ITMServicio(contextoAlh).GetAll();
                Mods = new MODServicio(contextoAlh).GetAll();
            }

            //RELACIONAR TODO LA DATA DEL MENU -> SUBMENUS -> ITEMS -> MODS -> ITEMS

            foreach (MNU menu in Menus2)
            {
                MNUmobile mNUmobile = new MNUmobile();
                mNUmobile.id_menu = menu.ID;
                mNUmobile.descripcion_larga = menu.LONGNAME;
                mNUmobile.descripcion_corta = menu.SHORTNAME;

                #region iterar sobre submenus del menu
                var Props = menu.GetType().GetProperties().ToList();
                string MENU = "MENU";
                foreach (var propItem in Props.FindAll(p => p.Name.ToString().Contains("MENU")))
                {
                    if ((int)propItem.GetValue(menu) > 0)
                    {
                        var name = propItem.Name;
                        SubMenu submenu = new SubMenu()
                        {
                            id = (int)propItem.GetValue(menu),
                        };
                        mNUmobile.subMenus.Add(submenu);
                    }
                }
                #endregion
                Menus.Add(mNUmobile);
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
