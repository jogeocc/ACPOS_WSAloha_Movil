using AlohaLibrary.Contexto;
using AlohaLibrary.Implementaciones;
using AlohaLibrary.Modelos;
using AlohaWebServiceMobile.EntityFrameWork.Enums;
using AlohaWebServiceMobile.EntityFrameWork.Implementaciones;
using AlohaWebServiceMobile.EntityFrameWork.Infraestructura;
using AlohaWebServiceMobile.EntityFrameWork.Models;
using AlohaWebServiceMobile.Models.Aloha;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlohaWebServiceMobile.ViewModels
{
    public class VMMapeoPagos : VistaModelo
    {
        private DatabaseFactory Dbf { get; set; }
        private Mapeo_pagoServicio Mapeo_pagoServicio { get; set; }
        public bool Editar { get; set; }

        public VMMapeoPagos()
        {
            Dbf = new DatabaseFactory();
            Mapeo_pagoServicio = new Mapeo_pagoServicio(Dbf);

            BuscadorID = "";
            ModelosPagoDBF = new List<Mapeo_pagos>();
        }

        public TipoRespuesta Guardar()
        {
            try
            {
                //Mapeo_Pagos.id_pago_aloha = Id_pago_aloha;
                //Mapeo_Pagos.clv_mapeo_pago = Clv_mapeo_pago;
                //Mapeo_Pagos.ID = Id;
                //Mapeo_Pagos.acumula_puntos = Acumula_puntos;
                //Mapeo_Pagos.nombre_pago_aloha = Nombre_pago_aloha;
                //Mapeo_Pagos.status = Status;
                Mapeo_Pagos.id_pago_aloha = ModeloNuevoMapeo.ID;
                Mapeo_Pagos.clv_mapeo_pago = Clv_mapeo_pago;
                Mapeo_Pagos.nombre_pago_aloha = ModeloNuevoMapeo.NAME;
                Mapeo_Pagos.status = Status;

                int result;

                if (Editar)
                {
                    result = Mapeo_pagoServicio.Update(Mapeo_Pagos);
                }
                else
                {


                    result = Mapeo_pagoServicio.Create(Mapeo_Pagos);
                }

                return result > 0 ? TipoRespuesta.HECHO : TipoRespuesta.VACIO;
            }
            catch (Exception ex)
            {
                return TipoRespuesta.ERROR_SISTEMA;
            }

        }

        public TipoRespuesta Eliminar()
        {
            try
            {
                int result = Mapeo_pagoServicio.Delete(Mapeo_Pagos.ID);

                return result > 0 ? TipoRespuesta.HECHO : TipoRespuesta.VACIO;
            }
            catch (Exception ex)
            {

                return TipoRespuesta.ERROR_SISTEMA;
            }
        }


        public void ObtenerLista()
        {
            string pathAloha = AlohaLibrary.Helpers.DirectoriosAloha.GetAlohaDataFolder();
            
            if (Directory.Exists(pathAloha))
            {
                using (AplicacionBdContextoALH contextoALH = new AplicacionBdContextoALH(pathAloha))
                {

                    //var DataGndTdr = new GNDTndrServicio(contextoALH).GetAll();
                    var TDRID = new TDRServicio(contextoALH).GetAll().ToList().OrderBy(TDR => TDR.ID).ToList();

                    List<ModeloNuevo> nuevaLista = new List<ModeloNuevo>();
                    foreach (TDR tdr in TDRID)
                    {
                        nuevaLista.Add(new ModeloNuevo
                        {
                            ID = tdr.ID,
                            NAME = tdr.NAME
                        });
                    }
                    ModeloNuevosMapeo = nuevaLista;
                }
            }
            else
            {

            }

        }

        public void Listar()
        {
            ModelosPagoDBF = Mapeo_pagoServicio.GetAll().ToList();
        }

        public void InicializarFormulario(bool editar = false)
        {
            ObtenerLista();
            Editar = editar;

            if (Editar)
            {
                ModeloNuevoMapeo = ModeloNuevosMapeo.Find(m => m.ID == Mapeo_Pagos.id_pago_aloha);
                Clv_mapeo_pago = Mapeo_Pagos.clv_mapeo_pago;
                Status = Mapeo_Pagos.status;

            }
            else
            {
                Mapeo_Pagos = new Mapeo_pagos();
                Id_pago_aloha = 0;
                Clv_mapeo_pago = "";
                Nombre_pago_aloha = "";
                Status = true;

            }
        }

        public bool ValidaEliminacion()
        {
            string mensaje = "";
            string titulo = "";

            if (Mapeo_pagoServicio.TieneRegistros(Mapeo_Pagos.ID))
            {
                titulo = "Eliminar";
                mensaje = "El dispositivo no se puede eliminar";
            }

            if (!(string.IsNullOrEmpty(mensaje) && string.IsNullOrEmpty(titulo)))
            {
                MessageBox.Show(mensaje, titulo, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private Mapeo_pagos mapeo_Pagos;
        public Mapeo_pagos Mapeo_Pagos
        {
            get { return mapeo_Pagos; }
            set
            {
                mapeo_Pagos = value;
                OnPropertyChanged(nameof(Mapeo_Pagos));
            }
        }
        private string clv_mapeo_pago;
        public string Clv_mapeo_pago
        {
            get { return clv_mapeo_pago; }
            set
            {
                clv_mapeo_pago = value;
                OnPropertyChanged(nameof(Clv_mapeo_pago));
            }
        }

        private string nombre_pago_aloha;
        public string Nombre_pago_aloha
        {
            get { return nombre_pago_aloha; }
            set
            {
                nombre_pago_aloha = value;
                OnPropertyChanged(nameof(Nombre_pago_aloha));
            }
        }

        private int id_pago_aloha;
        public int Id_pago_aloha
        {
            get { return id_pago_aloha; }
            set
            {
                id_pago_aloha = value;
                OnPropertyChanged(nameof(Id_pago_aloha));
            }
        }

        private List<Mapeo_pagos> modelosPagoDBF;
        public List<Mapeo_pagos> ModelosPagoDBF
        {
            get { return modelosPagoDBF; }
            set
            {
                modelosPagoDBF = value;
                OnPropertyChanged(nameof(ModelosPagoDBF));
            }
        }

        private List<ModeloNuevo> modeloNuevosMapeo;

        public List<ModeloNuevo> ModeloNuevosMapeo
        {
            get { return modeloNuevosMapeo; }
            set
            {
                modeloNuevosMapeo = value;
                OnPropertyChanged(nameof(ModeloNuevosMapeo));
            }
        }

        private ModeloNuevo modeloNuevoMapeo;
        public ModeloNuevo ModeloNuevoMapeo
        {
            get { return modeloNuevoMapeo; }
            set
            {
                if (value != null)
                {
                    Nombre_pago_aloha = value.NAME;
                }
                modeloNuevoMapeo = value;
                OnPropertyChanged(nameof(ModeloNuevoMapeo));
            }
        }

        private bool status;
        public bool Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private string buscadorID;
        public string BuscadorID
        {
            get { return buscadorID; }
            set
            {
                buscadorID = value;
                OnPropertyChanged(nameof(BuscadorID));
            }
        }
    }
}
