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

        List<MNU> MenusDbfs = new List<MNU>();
        List<SUB> SubMenusDBFS = new List<SUB>();
        List<ITM> ItemsDbfs = new List<ITM>();
        List<MOD> ModsDbfs = new List<MOD>();
        int BotonPlu = 999999;
        public EstructurarData()
        {
            pathALoha = AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder();
        }

        public List<MNUmobile> ObtenerMenuMovil()
        {
            List<MNUmobile> Menus = new List<MNUmobile>();


            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                MenusDbfs = new MNUServicio(contextoAlh).GetAll();
                SubMenusDBFS = new SUBServicio(contextoAlh).GetAll();
                ItemsDbfs = new ITMServicio(contextoAlh).GetAll();
                ModsDbfs = new MODServicio(contextoAlh).GetAll();
            }
            //RELACIONAR TODO LA DATA DEL MENU -> SUBMENUS -> ITEMS -> MODS -> ITEMS

            //RECOLECTAR PRIMER PASO MENUS
            foreach (MNU menu in MenusDbfs.Where(M => M.ID == 109))
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


            //RECOLECTAR PASO 2 SUBMENUS

            foreach (MNUmobile menu in Menus)
            {
                foreach (SubMenu sub in menu.subMenus)
                {
                    if (SubMenusDBFS.Any(S => S.ID == sub.id))
                    {
                        var SubMenu = SubMenusDBFS.First(S => S.ID == sub.id);
                        sub.descripcion_corta = SubMenu.SHORTNAME;
                        sub.descripcion_larga = SubMenu.LONGNAME;
                        #region iterar propiedades de submenu para obtener los items
                        var Props = SubMenu.GetType().GetProperties().ToList();
                        string AuxPrice = "PRICE";
                        string AuxPrmethod = "PRMETHOD";
                        string AuxPRICELVL = "PRICELVL";
                        int couter = 0;
                        foreach (var propItem in Props.FindAll(p => p.Name.ToString().Contains("ITEM")))
                        {
                            if ((int)propItem.GetValue(SubMenu) > 0)
                            {
                                var id = (int)propItem.GetValue(SubMenu);
                                if (id == BotonPlu) continue;
                                sub.items.Add(new Item
                                {
                                    id = (int)propItem.GetValue(SubMenu),
                                    submenu_precio_metodo = int.Parse(Props.Find(P => P.Name == AuxPrmethod + (couter + 1).ToString().PadLeft(2, '0')).GetValue(SubMenu).ToString()),
                                    submenu_precio_nivel = int.Parse(Props.Find(P => P.Name == AuxPRICELVL + (couter + 1).ToString().PadLeft(2, '0')).GetValue(SubMenu).ToString()),
                                    submenu_precio_sub = double.Parse(Props.Find(P => P.Name == AuxPrice + (couter + 1).ToString().PadLeft(2, '0')).GetValue(SubMenu).ToString()) / 100,
                                });
                            }
                        }
                        #endregion

                    }
                }
            }
            //recolectar paso 3 items
            foreach (var menu in Menus)
            {
                foreach (var sub in menu.subMenus)
                {
                    foreach (var item in sub.items)
                    {
                        if (ItemsDbfs.Any(I => I.ID == item.id))
                        {
                            var articulo = ItemsDbfs.First(I => I.ID == item.id);
                            item.descripcion_corta = articulo.SHORTNAME;
                            item.descripcion_larga = articulo.LONGNAME;
                            item.item_precio = articulo.PRICE;
                            item.item_precio_ID = articulo.PRICE_ID;

                            if (articulo.MOD1 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD1, });
                            if (articulo.MOD2 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD2, });
                            if (articulo.MOD3 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD3, });
                            if (articulo.MOD4 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD4, });
                            if (articulo.MOD5 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD5, });
                            if (articulo.MOD6 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD6, });
                            if (articulo.MOD7 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD7, });
                            if (articulo.MOD8 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD8, });
                            if (articulo.MOD9 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD9, });
                            if (articulo.MOD10 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD10, });
                        }
                    }
                }
            }

            //recolectar paso 4 Grupos de modificadores

            foreach (var menu in Menus)
            {
                foreach (var sub in menu.subMenus)
                {
                    foreach (var item in sub.items)
                    {
                        foreach (var mod in item.mods)
                        {
                            if (ModsDbfs.Any(M => M.ID == mod.id_modificador))
                            {
                                var modificador = ModsDbfs.First(M => M.ID == mod.id_modificador);
                                mod.descripcion_corta = modificador.SHORTNAME;
                                mod.descripcion_larga = modificador.LONGNAME;
                                mod.num_gratis = modificador.FREE;
                                mod.num_max = modificador.MAXIMUM;
                                mod.num_min = modificador.MINIMUM;
                                for (int i = 0; i < modificador.items.Count; i++)
                                {
                                    if (modificador.items[i] != 0)
                                    {
                                        Item ItemMOD = new Item();
                                        ItemMOD.id = modificador.items[i];
                                        ItemMOD.descripcion_corta = ItemsDbfs.Find(I => I.ID == ItemMOD.id).SHORTNAME;
                                        ItemMOD.descripcion_larga = ItemsDbfs.Find(I => I.ID == ItemMOD.id).LONGNAME;
                                        int condicion = int.Parse(modificador.methods[i].ToString());
                                        if (condicion == 0)
                                        {
                                            ItemMOD.item_precio = modificador.precios[i];
                                        }
                                        else
                                        {
                                            ItemMOD.item_precio = ItemsDbfs.First(I => I.ID == ItemMOD.id).PRICE;
                                        }

                                        mod.item_mod.Add(ItemMOD);
                                    }
                                }
                            }
                        }
                    }
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
        public List<JOBmobile> ObtenerPerfilesTrabajo()
        {
            List<JOBmobile> ObtenerPerfilesTrabajo = new List<JOBmobile>();

            return ObtenerPerfilesTrabajo;
        }

    }
}
