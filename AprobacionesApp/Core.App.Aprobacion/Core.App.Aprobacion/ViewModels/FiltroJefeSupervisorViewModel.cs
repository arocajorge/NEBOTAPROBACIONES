using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class FiltroJefeSupervisorViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsEnabled;
        private bool _IsRunning;
        private ApiService apiService;
        private DateTime _FechaInicio;
        private DateTime _FechaFin;
        private string _NumeroOrden;
        private bool SetSettings;

        private ObservableCollection<CatalogoModel> _ListaSucursal;
        private CatalogoModel _SelectedSucursal;
        private int _SucursalSelectedIndex;

        private ObservableCollection<CatalogoModel> _ListaViaje;
        private CatalogoModel _SelectedViaje;
        private int _ViajeSelectedIndex;

        private ObservableCollection<CatalogoModel> _ListaBodega;
        private CatalogoModel _SelectedBodega;
        private int _BodegaSelectedIndex;

        private ObservableCollection<CatalogoModel> _ListaEstadoJefe;
        private CatalogoModel _SelectedEstadoJefe;
        private int _EstadoJefeSelectedIndex;

        private ObservableCollection<CatalogoModel> _ListaEstadoSupervisor;
        private CatalogoModel _SelectedEstadoSupervisor;
        private int _EstadoSupervisorSelectedIndex;
        #endregion

        #region Propiedades
        public bool IsRunning
        {
            get { return this._IsRunning; }
            set { SetValue(ref this._IsRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set { SetValue(ref this._IsEnabled, value); }
        }
        public DateTime FechaInicio
        {
            get { return this._FechaInicio; }
            set { SetValue(ref this._FechaInicio, value); }
        }
        public DateTime FechaFin
        {
            get { return this._FechaFin; }
            set { SetValue(ref this._FechaFin, value); }
        }
        public string NumeroOrden
        {
            get { return this._NumeroOrden; }
            set { SetValue(ref this._NumeroOrden, value); }
        }

        public ObservableCollection<CatalogoModel> ListaSucursal
        {
            get { return this._ListaSucursal; }
            set {
                SetValue(ref this._ListaSucursal, value);
                if (SetSettings)
                {
                    SelectedSucursal = ListaSucursal.Where(q => q.Codigo == Settings.Sucursal).FirstOrDefault();
                    if (SelectedSucursal != null)
                        SucursalSelectedIndex = _ListaSucursal.IndexOf(SelectedSucursal);
                }
            }
        }
        public CatalogoModel SelectedSucursal
        {
            get { return this._SelectedSucursal; }
            set { SetValue(ref this._SelectedSucursal, value); }
        }
        public int SucursalSelectedIndex
        {
            get { return this._SucursalSelectedIndex; }
            set
            {
                SetValue(ref this._SucursalSelectedIndex, value);
                SelectedSucursal = _SucursalSelectedIndex < 0 ? new CatalogoModel() : ListaSucursal[_SucursalSelectedIndex];
            }
        }

        public ObservableCollection<CatalogoModel> ListaViaje
        {
            get { return this._ListaViaje; }
            set {
                SetValue(ref this._ListaViaje, value);
                if(SetSettings)
                {
                    SelectedViaje = ListaViaje.Where(q => q.Codigo == Settings.Viaje).FirstOrDefault();
                    if (SelectedViaje != null)
                        ViajeSelectedIndex = _ListaViaje.IndexOf(SelectedViaje);
                }
            }
        }
        public CatalogoModel SelectedViaje
        {
            get { return this._SelectedViaje; }
            set { SetValue(ref this._SelectedViaje, value); }
        }
        public int ViajeSelectedIndex
        {
            get { return this._ViajeSelectedIndex; }
            set
            {
                SetValue(ref this._ViajeSelectedIndex, value);
                SelectedViaje = _ViajeSelectedIndex < 0 ? new CatalogoModel() : ListaViaje[_ViajeSelectedIndex];
            }
        }

        public ObservableCollection<CatalogoModel> ListaBodega
        {
            get { return this._ListaBodega; }
            set {
                SetValue(ref this._ListaBodega, value);
                if (SetSettings)
                {
                    SelectedBodega = ListaBodega.Where(q => q.Codigo == Settings.Bodega).FirstOrDefault();
                    if (SelectedBodega != null)
                        BodegaSelectedIndex = ListaBodega.IndexOf(SelectedBodega);
                }
            }
        }
        public CatalogoModel SelectedBodega
        {
            get { return this._SelectedBodega; }
            set { SetValue(ref this._SelectedBodega, value); }
        }
        public int BodegaSelectedIndex
        {
            get { return this._BodegaSelectedIndex; }
            set
            {
                SetValue(ref this._BodegaSelectedIndex, value);
                SelectedBodega = _BodegaSelectedIndex < 0 ? new CatalogoModel() : ListaBodega[_BodegaSelectedIndex];
            }
        }

        public ObservableCollection<CatalogoModel> ListaEstadoJefe
        {
            get { return this._ListaEstadoJefe; }
            set
            {
                SetValue(ref this._ListaEstadoJefe, value);
                if (SetSettings)
                {
                    SelectedEstadoJefe = ListaEstadoJefe.Where(q => q.Codigo == Settings.EstadoJefe).FirstOrDefault();
                    if (SelectedEstadoJefe != null)
                        EstadoJefeSelectedIndex = ListaEstadoJefe.IndexOf(SelectedEstadoJefe);
                }
            }
        }
        public CatalogoModel SelectedEstadoJefe
        {
            get { return this._SelectedEstadoJefe; }
            set { SetValue(ref this._SelectedEstadoJefe, value); }
        }
        public int EstadoJefeSelectedIndex
        {
            get { return this._EstadoJefeSelectedIndex; }
            set
            {
                SetValue(ref this._EstadoJefeSelectedIndex, value);
                SelectedEstadoJefe = _EstadoJefeSelectedIndex < 0 ? new CatalogoModel() : ListaEstadoJefe[_EstadoJefeSelectedIndex];
            }
        }

        public ObservableCollection<CatalogoModel> ListaEstadoSupervisor
        {
            get { return this._ListaEstadoSupervisor; }
            set
            {
                SetValue(ref this._ListaEstadoSupervisor, value);
                if (SetSettings)
                {
                    SelectedEstadoSupervisor = _ListaEstadoSupervisor.Where(q => q.Codigo == Settings.EstadoSupervisor).FirstOrDefault();
                    if (SelectedEstadoSupervisor != null)
                        EstadoSupervisorSelectedIndex = ListaEstadoSupervisor.IndexOf(SelectedEstadoSupervisor);
                }
            }
        }
        public CatalogoModel SelectedEstadoSupervisor
        {
            get { return this._SelectedEstadoSupervisor; }
            set { SetValue(ref this._SelectedEstadoSupervisor, value); }
        }
        public int EstadoSupervisorSelectedIndex
        {
            get { return this._EstadoSupervisorSelectedIndex; }
            set
            {
                SetValue(ref this._EstadoSupervisorSelectedIndex, value);
                SelectedEstadoSupervisor = _EstadoSupervisorSelectedIndex < 0 ? new CatalogoModel() : ListaEstadoSupervisor[_EstadoSupervisorSelectedIndex];
            }
        }
        #endregion

        #region Constructor
        public FiltroJefeSupervisorViewModel(string ROL_APRO)
        {
            apiService = new ApiService();
            IsEnabled = true;
            SelectedSucursal = new CatalogoModel();
            SelectedViaje = new CatalogoModel();
            SelectedBodega = new CatalogoModel();
            SelectedEstadoJefe = new CatalogoModel();
            SelectedEstadoSupervisor = new CatalogoModel();
            FechaInicio = string.IsNullOrEmpty(Settings.FechaInicio) ? DateTime.Now.Date.AddMonths(-2) : Convert.ToDateTime(Settings.FechaInicio);
            FechaFin = DateTime.Now.Date;
            Settings.RolApro = ROL_APRO;

          if(!string.IsNullOrEmpty(Settings.FechaInicio))
                SetSettings = true;
            CargarCombos();
            SetSettings = false;
        }
        #endregion

        #region Metodos
        public async void CargarCombos()
        {
            try
            {
                IsRunning = true;
                IsEnabled = false;
                if (string.IsNullOrEmpty(Settings.UrlConexionActual))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Dispositivo no configurado",
                        "Aceptar");
                    return;
                }

                Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
                if (!con.IsSuccess)
                {
                    string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                    con = await apiService.CheckConnection(UrlDistinto);
                    if (!con.IsSuccess)
                    {
                        this.IsEnabled = true;
                        this.IsRunning = false;
                        await Application.Current.MainPage.DisplayAlert(
                            "Alerta",
                            con.Message,
                            "Aceptar");
                        return;
                    }
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }

                var response_cs = await apiService.GetList<CatalogoModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Catalogos", "");
                if (!response_cs.IsSuccess)
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }

                var ListaCatalogos = (List<CatalogoModel>)response_cs.Result;
                ListaCatalogos.Add(new CatalogoModel { Codigo = "", Descripcion = "Todo", Tipo = "Sucursal" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "", Descripcion = "Todo", Tipo = "Bodega" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "", Descripcion = "Todo", Tipo = "Viaje" });
                List<CatalogoModel> _lstJefe = new List<CatalogoModel>();
                _lstJefe.Add(new CatalogoModel
                {
                    Codigo = "A",
                    Descripcion = "Pendiente",
                });
                _lstJefe.Add(new CatalogoModel
                {
                    Codigo = "X",
                    Descripcion = "Anulado",
                });
                _lstJefe.Add(new CatalogoModel
                {
                    Codigo = "P",
                    Descripcion = "Aprobado",
                });
                _lstJefe.Add(new CatalogoModel
                {
                    Codigo = "",
                    Descripcion = "Todo"
                });

                ListaSucursal = new ObservableCollection<CatalogoModel>(ListaCatalogos.Where(q => q.Tipo == "Sucursal").Select(q=>new CatalogoModel { Codigo = q.Codigo, Descripcion = q.Descripcion}).OrderBy(q => q.Descripcion).ToList());                

                ListaBodega = new ObservableCollection<CatalogoModel>(ListaCatalogos.Where(q => q.Tipo == "Bodega").Select(q => new CatalogoModel { Codigo = q.Codigo, Descripcion = q.Descripcion }).OrderBy(q => q.Descripcion).ToList());                

                ListaViaje = new ObservableCollection<CatalogoModel>(ListaCatalogos.Where(q => q.Tipo == "Viaje").Select(q => new CatalogoModel { Codigo = q.Codigo, Descripcion = q.Descripcion }).OrderBy(q => q.Descripcion).ToList());                

                ListaEstadoJefe = new ObservableCollection<CatalogoModel>(_lstJefe);                
                
                ListaEstadoSupervisor = new ObservableCollection<CatalogoModel>(_lstJefe);

                if (string.IsNullOrEmpty(Settings.FechaInicio))
                {
                    if (Settings.RolApro == "J")
                    {
                        EstadoJefeSelectedIndex = 0;
                        EstadoSupervisorSelectedIndex = 3;
                    }
                    else
                    {
                        EstadoJefeSelectedIndex = 3;
                        EstadoSupervisorSelectedIndex = 0;
                    }
                }

                this.IsEnabled = true;
                this.IsRunning = false;
            }
            catch (System.Exception ex)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");

                return;
            }
        }
        #endregion

        #region Comandos
        public ICommand FiltrarCommand
        {
            get
            {
                return new RelayCommand(Filtrar);
            }
        }

        private async void Filtrar()
        {
            try
            {
                IsRunning = true;
                IsEnabled = false;

                Settings.FechaInicio = FechaInicio.ToShortDateString();
                Settings.FechaFin = FechaFin.ToShortDateString();
                Settings.Sucursal = SelectedSucursal == null ? "" : SelectedSucursal.Codigo;
                Settings.Bodega = SelectedBodega == null ? "" : SelectedBodega.Codigo;
                Settings.Viaje = SelectedViaje == null ? "" : SelectedViaje.Codigo;
                Settings.EstadoJefe = SelectedEstadoJefe == null ? "" : SelectedEstadoJefe.Codigo;
                Settings.EstadoSupervisor = SelectedEstadoSupervisor == null ? "" : SelectedEstadoSupervisor.Codigo;
                Settings.NumeroOrden = NumeroOrden;
                /*
                string mensaje = "FechaInicio="+Settings.FechaInicio;
                mensaje += "&FechaFin=" + Settings.FechaFin;
                mensaje += "&Sucursal=" + Settings.Sucursal;
                mensaje += "&Bodega=" + Settings.Bodega;
                mensaje += "&Viaje=" + Settings.Viaje;
                mensaje += "&EstadoJefe=" + Settings.EstadoJefe;
                mensaje += "&EstadoSupervisor=" + Settings.EstadoSupervisor;
                mensaje += "&NumeroOrden=" + Settings.NumeroOrden;
                
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    mensaje,
                    "Aceptar");
                    */
                IsRunning = false;
                IsEnabled = true;

                MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
                Application.Current.MainPage = new JefeSupervisorMasterPage();
            }
            catch (System.Exception ex)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");

                return;
            }
        }
        #endregion
    }
}
