using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
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
                    SelectedSucursal = _ListaSucursal.Where(q => q.Codigo == Settings.Sucursal).FirstOrDefault();
                    SucursalSelectedIndex = _ListaSucursal.IndexOf(SelectedSucursal == null ? new CatalogoModel() : SelectedSucursal);
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
                _SelectedSucursal = _SucursalSelectedIndex < 0 ? new CatalogoModel() : ListaSucursal[_SucursalSelectedIndex];
            }
        }

        public ObservableCollection<CatalogoModel> ListaViaje
        {
            get { return this._ListaViaje; }
            set { SetValue(ref this._ListaViaje, value);
                if (SetSettings)
                {
                    SelectedViaje = _ListaViaje.Where(q => q.Codigo == Settings.Viaje).FirstOrDefault();
                    ViajeSelectedIndex = _ListaViaje.IndexOf(SelectedViaje == null ? new CatalogoModel() : SelectedViaje);
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
            set { SetValue(ref this._ListaBodega, value);
                if (SetSettings)
                {
                    SelectedBodega = _ListaBodega.Where(q => q.Codigo == Settings.Bodega).FirstOrDefault();
                    BodegaSelectedIndex = _ListaBodega.IndexOf(SelectedBodega == null ? new CatalogoModel() : SelectedBodega);
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
            get { return this._ViajeSelectedIndex; }
            set
            {
                SetValue(ref this._ViajeSelectedIndex, value);
                SelectedBodega = _ViajeSelectedIndex < 0 ? new CatalogoModel() : ListaViaje[_ViajeSelectedIndex];
            }
        }
        #endregion

        #region Constructor
        public FiltroJefeSupervisorViewModel()
        {
            apiService = new ApiService();
            IsEnabled = true;
            SelectedSucursal = new CatalogoModel();
            SelectedViaje = new CatalogoModel();
            SelectedBodega = new CatalogoModel();
            FechaInicio = DateTime.Now.Date.AddMonths(-2);
            FechaFin = DateTime.Now.Date;
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

                var response_cs = await apiService.GetObject<CatalogoModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Catalogos", "");
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

                List<CatalogoModel> ListaCatalogos = (List<CatalogoModel>)response_cs.Result;

                ListaSucursal = new ObservableCollection<CatalogoModel>(ListaCatalogos.Where(q => q.Tipo == "Sucursal").OrderBy(q => q.Descripcion).ToList());
                ListaSucursal.Add(new CatalogoModel
                {
                    Tipo = "Sucursal",
                    Codigo = "",
                    Descripcion = "TODO",
                });
                ListaBodega = new ObservableCollection<CatalogoModel>(ListaCatalogos.Where(q => q.Tipo == "Bodega").OrderBy(q => q.Descripcion).ToList());
                ListaBodega.Add(new CatalogoModel
                {
                    Tipo = "Bodega",
                    Codigo = "",
                    Descripcion = "TODO",
                });
                ListaViaje = new ObservableCollection<CatalogoModel>(ListaCatalogos.Where(q => q.Tipo == "Viaje").OrderBy(q => q.Descripcion).ToList());
                ListaBodega.Add(new CatalogoModel
                {
                    Tipo = "Viaje",
                    Codigo = "",
                    Descripcion = "TODO",
                });

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
                Settings.FechaInicio = FechaInicio.ToShortDateString();
                Settings.FechaFin = FechaFin.ToShortDateString();
                Settings.Sucursal = SelectedSucursal == null ? "" : SelectedSucursal.Codigo;
                Settings.Bodega = SelectedBodega == null ? "" : SelectedBodega.Codigo;
                Settings.Viaje = SelectedViaje == null ? "" : SelectedViaje.Codigo;
                Settings.NumeroOrden = NumeroOrden;


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
