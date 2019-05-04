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
    public class JefeSupervisorFiltroViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsEnabled;
        private bool _IsRunning;
        private ApiService apiService;
        private DateTime _FechaInicio;
        private bool SetSettings;

        private ObservableCollection<CatalogoModel> _ListaSucursal;
        private CatalogoModel _SelectedSucursal;
        private int _SucursalSelectedIndex;

        private ObservableCollection<CatalogoModel> _ListaViaje;
        private CatalogoModel _SelectedViaje;
        private int _ViajeSelectedIndex;
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
        #endregion

        #region Constructor
        public JefeSupervisorFiltroViewModel(string ROL_APRO)
        {
            apiService = new ApiService();
            IsEnabled = true;
            SelectedSucursal = new CatalogoModel();
            SelectedViaje = new CatalogoModel();
            FechaInicio = DateTime.Now.Date.AddMonths(-2);
            Settings.RolApro = ROL_APRO;
            SetSettings = true;
            CargarCombos();
            SetSettings = false;
        }
        #endregion

        #region Metodos
        private void CargarCombos()
        {
            ListaSucursal = new ObservableCollection<CatalogoModel>(MainViewModel.GetInstance().ListaCatalogos.Where(q => q.Tipo == "Sucursal").OrderBy(q=>q.Descripcion).ToList());
            ListaViaje = new ObservableCollection<CatalogoModel>(MainViewModel.GetInstance().ListaCatalogos.Where(q => q.Tipo == "Viaje").OrderBy(q => q.Descripcion).ToList());
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
                Settings.Sucursal = SelectedSucursal == null ? "" : SelectedSucursal.Codigo;
                Settings.Viaje = SelectedViaje == null ? "" : SelectedViaje.Codigo;
                Settings.NombreSucursal = SelectedSucursal == null ? "" : SelectedSucursal.Descripcion;
                Settings.NombreViaje = SelectedViaje == null ? "" : SelectedViaje.Descripcion;
                IsRunning = false;
                IsEnabled = true;

                FiltroModel filtro = new FiltroModel
                {
                    Usuario = Settings.IdUsuario,
                    Sucursal = Settings.Sucursal,
                    Viaje = Settings.Viaje, 
                    Bodega = Settings.Bodega, 
                    Solicitante = Settings.Solicitante
                };
                var response_sinc = await apiService.Post<FiltroModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "Filtro",
                filtro);

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