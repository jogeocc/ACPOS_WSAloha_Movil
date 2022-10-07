using AlohaLibrary.Contexto;
using AlohaLibrary.Enums;
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

        private List<MNU> MenusDbfs = new List<MNU>();
        private List<SUB> SubMenusDBFS = new List<SUB>();
        private List<ITM> ItemsDbfs = new List<ITM>();
        private List<MOD> ModsDbfs = new List<MOD>();
        private List<QTYPRICE> Qtyprices = new List<QTYPRICE>();

        private int BotonPlu = 999999;
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
                Qtyprices = new QTYPRICEServicio(contextoAlh).GetAll();
            }

            //RELACIONAR TODO LA DATA DEL MENU -> SUBMENUS -> ITEMS -> MODS -> ITEMS

            //RECOLECTAR PRIMER PASO MENUS
            foreach (MNU menu in MenusDbfs.Where(M => M.ID == App.appConfig.ID_MENU_MOVIL))
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
                            item.descripcion_corta = DecodeToASCII(articulo.SHORTNAME);
                            item.descripcion_larga = DecodeToASCII(articulo.LONGNAME);
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
                            if (articulo.MOD10 != 0) item.mods.Add(new Mod { id_modificador = articulo.MOD10 });

                            if (Qtyprices.Any(I => I.ITEMID == item.id))
                            {
                                var Cantidad = Qtyprices.First(I => I.ID == item.id);
                                item.is_cantidad = true;
                                item.unidad_medida = Cantidad.UNITNAME;
                                item.unidad_decimales = Cantidad.DECIMALS;
                            }


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
                                mod.descripcion_corta = DecodeToASCII(modificador.SHORTNAME);
                                mod.descripcion_larga = DecodeToASCII(modificador.LONGNAME);
                                mod.num_gratis = modificador.FREE;
                                mod.num_max = modificador.MAXIMUM;
                                mod.num_min = modificador.MINIMUM;
                                for (int i = 0; i < modificador.items.Count; i++)
                                {
                                    if (modificador.items[i] != 0)
                                    {
                                        Item ItemMOD = new Item();
                                        ItemMOD.id = modificador.items[i];
                                        ItemMOD.descripcion_corta = DecodeToASCII(ItemsDbfs.Find(I => I.ID == ItemMOD.id).SHORTNAME);
                                        ItemMOD.descripcion_larga = DecodeToASCII(ItemsDbfs.Find(I => I.ID == ItemMOD.id).LONGNAME);
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
                        NAME = odr.NAME,
                        ACTIVE = odr.ACTIVE == TipoLogicoALH.Y
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
                        acepta_propina = tdr.TIPS,
                        status = tdr.ACTIVE,
                        pin_pad = tdr.IDENTIFY,
                        Etiqueta_min = tdr.IDMINDIGIT,
                        Etiqueta_max = tdr.IDMAXDIGIT,
                        Etiqueta_nombre = tdr.IDENTNAME,
                    }); ;
                }
            }


            return TendersMobile.OrderBy(t => t.id_forma_de_pago).ToList();
        }
        public List<JOBmobile> ObtenerPerfilesTrabajo()
        {
            List<JOBmobile> PerfilesTrabajo = new List<JOBmobile>();
            List<JOB> JobsDBFS = new List<JOB>();
            List<ACC> Accesos = new List<ACC>();

            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                JobsDBFS = new JOBServicio(contextoAlh).GetAll();
                Accesos = new ACCServicio(contextoAlh).GetAll();
            }
            foreach (var perfil in JobsDBFS)
            {
                JOBmobile job = new JOBmobile();
                job.id = perfil.ID;
                job.descripcion_corta = perfil.SHORTNAME;
                job.descripcion_larga = perfil.LONGNAME;
                PerfilesTrabajo.Add(job);
            }
            return PerfilesTrabajo;
        }
        public List<MODCODEmobile> ObtenerModCodes()
        {
            List<MODCODEmobile> List = new List<MODCODEmobile>();
            List<MODCODE> ListDBFS = new List<MODCODE>();
            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                ListDBFS = new MODCODEServicio(contextoAlh).GetAll();
            }
            foreach (var CodeMod in ListDBFS)
            {
                List.Add(new MODCODEmobile
                {
                    ID = CodeMod.USERNUMBER,
                    ACTIVE = CodeMod.ACTIVE,
                    DESC = CodeMod.DESC,
                    INDICATOR = CodeMod.INDICATOR,
                    MOD_NAME = CodeMod.MOD_NAME,
                    QUANTITY = CodeMod.QUANTITY,
                });
            }
            return List;
        }
        public List<PRTMobile> ObtenerImpresoras()
        {
            List<PRTMobile> Impresoras = new List<PRTMobile>();
            List<PRT> ListDBFS = new List<PRT>();

            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                ListDBFS = new PRTServicio(contextoAlh).GetAll();
            }
            foreach (var Printer in ListDBFS)
            {
                Impresoras.Add(new PRTMobile
                {
                    ID = Printer.ID,
                    NAME = Printer.NAME,
                    TERMINAL = Printer.TERMINAL,
                });
            }
            return Impresoras;
        }
        public List<VOIDMobile> ObtenerVoids()
        {
            List<VOIDMobile> ListaVoids = new List<VOIDMobile>();
            List<RSN> ListDBFS = new List<RSN>();

            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                ListDBFS = new RsnServicioALH(contextoAlh).GetAll();
            }
            foreach (var Anulacion in ListDBFS)
            {
                ListaVoids.Add(new VOIDMobile
                {
                    NAME = Anulacion.NAME,
                    ID = Anulacion.ID,
                });
            }
            return ListaVoids;
        }

        private string DecodeToASCII(string cadena)
        {

            Encoding extAscii = Encoding.GetEncoding(850);
            Encoding win1252 = Encoding.GetEncoding(1252);

            byte[] bytes1252 = extAscii.GetBytes(cadena);

            byte[] output = Encoding.Convert(win1252, extAscii, bytes1252);//use of the objects

            string CadenaLimpia = extAscii.GetString(output);
            return CadenaLimpia;
        }


        public object RecursividadNivelesMods()
        {
            return new object();
        }
    }
}

