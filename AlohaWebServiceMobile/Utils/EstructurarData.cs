using AlohaLibrary.Contexto;
using AlohaLibrary.Enums;
using AlohaLibrary.Implementaciones;
using AlohaLibrary.Modelos;
using AlohaWebServiceMobile.Enums;
using AlohaWebServiceMobile.Models.Catalogos;
using Design_Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.Utils
{
    public class EstructurarData
    {

        string pathALoha = @"D:\PROYECTOS\Aloha_mobile\SS_DATA\DATA";

        private List<MNU> MenusDbfs = new List<MNU>();
        private List<SUB> SubMenusDBFS = new List<SUB>();
        private List<ITM> ItemsDbfs = new List<ITM>();
        private List<MOD> ModsDbfs = new List<MOD>();
        private List<MODEXT> ModsDbfs15 = new List<MODEXT>();
        private List<BTN> BtnsDbfs = new List<BTN>();
        private List<PNL> PNLSDbfs = new List<PNL>();

        private List<QTYPRICE> Qtyprices = new List<QTYPRICE>();

        private int BotonPlu = 999999;
        public EstructurarData()
        {
            //pathALoha = AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder();
            //pathALoha = @"D:\PROYECTOS\Aloha_mobile\Data";
        }

        #region OBTENER MENU
        public List<MNUmobile> ObtenerMenuMovil()
        {
            List<MNUmobile> Menus = new List<MNUmobile>();


            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                MenusDbfs = new MNUServicio(contextoAlh).GetAll();
                SubMenusDBFS = new SUBServicio(contextoAlh).GetAll();
                ItemsDbfs = new ITMServicio(contextoAlh).GetAll();
                ModsDbfs = new MODServicio(contextoAlh).GetAll();
                BtnsDbfs = new BTNServicio(contextoAlh).GetAll();
                PNLSDbfs = new PNLServicio(contextoAlh).GetAll();


                if (File.Exists(pathALoha + @"\MODEXT.dbf"))
                {
                    ModsDbfs15 = new MODEXTServicio(contextoAlh).GetAll();
                }

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
                        sub.panel_id = SubMenu.PANEL_ID;
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



            // A PARTIR DE ESTE PUNTO REALIZAR LA RECURSIVIDAD


            //recolectar paso 3 items
            foreach (var menu in Menus)
            {
                foreach (var sub in menu.subMenus)
                {
                    if (!sub.UsePanels)
                    {
                        for (int i = 0; i < sub.items.Count; i++)
                        {
                            Item item = sub.items[i];
                            item = RecursividadItems(item);
                        }
                    }
                    else
                    {
                        List<BTN> Btns = BtnsDbfs.FindAll(B => B.PANELID == sub.panel_id && (B.FUNC == (int)AlohaPanelCodes.BOTON_PANEL || B.FUNC == (int)AlohaPanelCodes.BOTON_ITEM));
                        foreach (var btn in Btns)
                        {
                            if (btn.FUNC == (int)AlohaPanelCodes.BOTON_PANEL)
                            {

                                Item Boton_panel = new Item();
                                int.TryParse(btn.PARAMS, out int IdPanel);
                                Boton_panel.id_panel = IdPanel;
                                Boton_panel.descripcion_larga = PNLSDbfs.First(P => P.ID == IdPanel).NAME;
                                Boton_panel.descripcion_corta = PNLSDbfs.First(P => P.ID == IdPanel).NAME;
                                IdsPaneles.Clear();
                                Boton_panel = RecursividadPaneles(Boton_panel);
                                if (Boton_panel != null)
                                {
                                    sub.Btns.Add(Boton_panel);
                                }
                            }
                            else
                            {
                                Item Boton_item = new Item();

                                List<string> Params = btn.PARAMS.Split(',').ToList();
                                int IdProducto = int.Parse(Params[0]);
                                double PrecioBoton = double.Parse(Params[1]);
                                int Desconocido = int.Parse(Params[2]);
                                int Metodo = int.Parse(Params[3]);


                                Boton_item.id = IdProducto;
                                producto = IdProducto;
                                profundidad = 0;
                                Boton_item = RecursividadItems(Boton_item);
                                if (Boton_item != null)
                                {
                                    sub.Btns.Add(Boton_item);
                                }

                            }
                        }
                    }
                }
            }

            return Menus;
        }

        public int profundidad;
        public int producto;
        public List<int> IdsPaneles = new List<int>();

        public Item RecursividadItems(Item item)
        {
            profundidad++;
            try
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

                    foreach (var mod in item.mods.ToList())
                    {
                        if (ModsDbfs.Any(M => M.ID == mod.id_modificador))
                        {
                            var modificador = ModsDbfs.First(M => M.ID == mod.id_modificador);
                            mod.descripcion_corta = DecodeToASCII(modificador.SHORTNAME);
                            mod.descripcion_larga = DecodeToASCII(modificador.LONGNAME);
                            mod.num_gratis = modificador.FREE;
                            mod.num_max = modificador.MAXIMUM;
                            mod.num_min = modificador.MINIMUM;

                            if (ModsDbfs15.Count > 0)
                            {
                                var ListaMods = GetMods(mod.id_modificador);
                                for (int i = 0; i < ListaMods.Count; i++)
                                {
                                    Item ItemMOD = new Item();
                                    ItemMOD.id = ListaMods[i].ITEMID;

                                    var itemDBF = ItemsDbfs.Find(I => I.ID == ItemMOD.id);
                                    ItemMOD.descripcion_corta = DecodeToASCII(itemDBF.SHORTNAME);
                                    ItemMOD.descripcion_larga = DecodeToASCII(itemDBF.LONGNAME);
                                    int condicion = ListaMods[i].PRMETHOD;
                                    if (condicion == 0)
                                    {
                                        ItemMOD.item_precio = decimal.Parse(ListaMods[i].PRICE.ToString());
                                    }
                                    else
                                    {
                                        ItemMOD.item_precio = ItemsDbfs.First(I => I.ID == ItemMOD.id).PRICE;
                                    }
                                    List<Mod> AuxList = new List<Mod>();

                                    if (itemDBF.MOD1 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD1, });
                                    if (itemDBF.MOD2 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD2, });
                                    if (itemDBF.MOD3 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD3, });
                                    if (itemDBF.MOD4 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD4, });
                                    if (itemDBF.MOD5 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD5, });
                                    if (itemDBF.MOD6 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD6, });
                                    if (itemDBF.MOD7 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD7, });
                                    if (itemDBF.MOD8 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD8, });
                                    if (itemDBF.MOD9 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD9, });
                                    if (itemDBF.MOD10 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD10 });

                                    if (AuxList.Count > 0)
                                    {
                                        ItemMOD = RecursividadItems(ItemMOD);
                                    }

                                    mod.item_mod.Add(ItemMOD);
                                }
                            }
                            else
                            {
                                for (int j = 0; j < modificador.items.Count; j++)
                                {
                                    if (modificador.items[j] != 0)
                                    {
                                        Item ItemMOD = new Item();
                                        ItemMOD.id = modificador.items[j];

                                        var itemDBF = ItemsDbfs.Find(I => I.ID == ItemMOD.id);
                                        ItemMOD.descripcion_corta = DecodeToASCII(itemDBF.SHORTNAME);
                                        ItemMOD.descripcion_larga = DecodeToASCII(itemDBF.LONGNAME);
                                        int condicion = int.Parse(modificador.methods[j].ToString());
                                        if (condicion == 0)
                                        {
                                            ItemMOD.item_precio = modificador.precios[j];
                                        }
                                        else
                                        {
                                            ItemMOD.item_precio = ItemsDbfs.First(I => I.ID == ItemMOD.id).PRICE;
                                        }
                                        List<Mod> AuxList = new List<Mod>();

                                        if (itemDBF.MOD1 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD1, });
                                        if (itemDBF.MOD2 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD2, });
                                        if (itemDBF.MOD3 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD3, });
                                        if (itemDBF.MOD4 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD4, });
                                        if (itemDBF.MOD5 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD5, });
                                        if (itemDBF.MOD6 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD6, });
                                        if (itemDBF.MOD7 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD7, });
                                        if (itemDBF.MOD8 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD8, });
                                        if (itemDBF.MOD9 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD9, });
                                        if (itemDBF.MOD10 != 0) AuxList.Add(new Mod { id_modificador = itemDBF.MOD10 });

                                        if (AuxList.Count > 0)
                                        {
                                            ItemMOD = RecursividadItems(ItemMOD);
                                        }

                                        mod.item_mod.Add(ItemMOD);
                                    }
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return item;
        }

        public Item RecursividadPaneles(Item Panel)
        {
            Item item = new Item();
            item.id_panel = Panel.id_panel;
            item.descripcion_larga = Panel.descripcion_larga;
            item.descripcion_corta = Panel.descripcion_corta;

            item.PanelTransicion.descripcion_larga = Panel.descripcion_larga;
            item.PanelTransicion.descripcion_corta = Panel.descripcion_corta;

            if (IdsPaneles.Contains(Panel.id_panel))
            {
                return null;
            }

            IdsPaneles.Add(Panel.id_panel);

            List<BTN> Btns = BtnsDbfs.FindAll(B => B.PANELID == Panel.id_panel && (B.FUNC == (int)AlohaPanelCodes.BOTON_PANEL || B.FUNC == (int)AlohaPanelCodes.BOTON_ITEM));

            foreach (var btn in Btns)
            {
                if (btn.FUNC == (int)AlohaPanelCodes.BOTON_PANEL)
                {



                    Item Boton_panel = new Item();
                    int.TryParse(btn.PARAMS, out int IdPanel);
                    Boton_panel.id_panel = IdPanel;
                    Boton_panel.descripcion_larga = PNLSDbfs.First(P => P.ID == IdPanel).NAME;
                    Boton_panel.descripcion_corta = PNLSDbfs.First(P => P.ID == IdPanel).NAME;
                    Boton_panel = RecursividadPaneles(Boton_panel);
                    if (Boton_panel != null)
                    {
                        item.PanelTransicion.Btns.Add(Boton_panel);
                    }
                }
                else
                {
                    Item Boton_item = new Item();
                    List<string> Params = btn.PARAMS.Split(',').ToList();
                    int IdProducto = int.Parse(Params[0]);
                    double PrecioBoton = double.Parse(Params[1]);
                    int Desconocido = int.Parse(Params[2]);
                    int Metodo = int.Parse(Params[3]);


                    Boton_item.id = IdProducto;
                    producto = IdProducto;
                    profundidad = 0;
                    Boton_item = RecursividadItems(Boton_item);
                    if (Boton_item != null)
                    {
                        item.PanelTransicion.Btns.Add(Boton_item);
                    }
                }
            }
            return item;
        }

        private List<MODEXT> GetMods(int IdGrupo)
        {
            List<MODEXT> mods = new List<MODEXT>();
            try
            {
                mods = ModsDbfs15.FindAll(MOD => MOD.MODGRPID == IdGrupo);
                mods = mods.OrderBy(MOD => MOD.SEQUENCE).ToList();
            }
            catch (Exception ex)
            {
                App.logger.Error("Error al obtener MODS", ex);
            }
            return mods;
        }
        #endregion

        public List<ODRmobile> ObtenerModosDePedido()
        {
            List<ODRmobile> OrderModMobile = new List<ODRmobile>();

            using (AplicacionBdContextoALH contextoALH = new AplicacionBdContextoALH(pathALoha))
            {
                List<OrdenALH> ordenALHs = new OrdenServicioALH(contextoALH).GetAll();
                foreach (var odr in ordenALHs)
                {
                    OrderModMobile.Add(new ODRmobile
                    {
                        ID = odr.ID,
                        NAME = odr.NAME,
                        INDICATOR = odr.INDICATOR,
                        ACTIVE = odr.ACTIVE == TipoLogicoALH.Y,
                        ALLITEMS = odr.ALLITEMS == TipoLogicoALH.Y,
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
        public List<PNLMobile> ObtenerPaneles()
        {
            List<PNLMobile> ListaPaneles = new List<PNLMobile>();
            List<PNL> ListDBFS = new List<PNL>();

            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                ListDBFS = new PNLServicio(contextoAlh).GetAll();
            }
            foreach (var Panel in ListDBFS)
            {
                ListaPaneles.Add(new PNLMobile()
                {
                    NAME = Panel.NAME,
                    ID = Panel.ID,
                    QSTSMODE = Panel.QSTSMODE,
                    TITLE = Panel.TITLE,
                    ALOHAMOBLE = Panel.ALOHAMOBLE,
                });
            }

            return ListaPaneles;
        }

        public List<BTNMobile> ObtenerBotones()
        {

            List<BTNMobile> ListaPaneles = new List<BTNMobile>();
            List<BTN> ListDBFS = new List<BTN>();

            using (AplicacionBdContextoALH contextoAlh = new AplicacionBdContextoALH(pathALoha))
            {
                ListDBFS = new BTNServicio(contextoAlh).GetAll();
            }
            foreach (var Boton in ListDBFS)
            {
                ListaPaneles.Add(new BTNMobile()
                {
                    ALOHAMOBLE = Boton.ALOHAMOBLE,
                    TEXT = Boton.TEXT,
                    BKBLUE = Boton.BKBLUE,
                    BKGREEN = Boton.BKGREEN,
                    BKRED = Boton.BKRED,
                    RED = Boton.RED,
                    GREEN = Boton.GREEN,
                    BLUE = Boton.BLUE,
                    FUNC = Boton.FUNC,
                    PARAMS = Boton.PARAMS,
                    PANELID = Boton.PANELID,
                    ID = Boton.ID
                });
            }

            return ListaPaneles;

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


        public string ObtenerDesign()
        {
            string DesignJson = "";

            try
            {
                Desing_Library Desing = new Desing_Library();
                DesignJson = Desing.ObtenerDisenio();
            }
            catch (Exception ex)
            {
                App.logger.Error("Error al obtener diseños", ex);
            }

            return DesignJson;
        }


    }
}

