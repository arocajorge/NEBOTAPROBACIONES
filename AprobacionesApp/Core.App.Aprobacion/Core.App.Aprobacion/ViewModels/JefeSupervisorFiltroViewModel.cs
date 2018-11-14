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
        public JefeSupervisorFiltroViewModel(string ROL_APRO)
        {
            apiService = new ApiService();
            IsEnabled = true;
            SelectedSucursal = new CatalogoModel();
            SelectedViaje = new CatalogoModel();
            SelectedBodega = new CatalogoModel();
            SelectedEstadoJefe = new CatalogoModel();
            SelectedEstadoSupervisor = new CatalogoModel();
            FechaInicio = string.IsNullOrEmpty(Settings.FechaInicio) ? DateTime.Now.Date.AddMonths(-2) : Convert.ToDateTime(Settings.FechaInicio);
            Settings.RolApro = ROL_APRO;

            if (!string.IsNullOrEmpty(Settings.FechaInicio))
            {
                Settings.Sucursal = "";
                Settings.Bodega = "";
                Settings.Viaje = "";
                if(ROL_APRO == "J")
                {
                    Settings.EstadoJefe = "A";
                    Settings.EstadoSupervisor = "";
                }else
                {
                    Settings.EstadoJefe = "";
                    Settings.EstadoSupervisor = "A";
                }
            }
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
            ListaBodega = new ObservableCollection<CatalogoModel>(MainViewModel.GetInstance().ListaCatalogos.Where(q => q.Tipo == "Bodega").OrderBy(q => q.Descripcion).ToList());
            ListaEstadoJefe = new ObservableCollection<CatalogoModel>(MainViewModel.GetInstance().ListaCatalogos.Where(q => q.Tipo == "Estado").OrderBy(q => q.Descripcion).ToList());
            ListaEstadoSupervisor = new ObservableCollection<CatalogoModel>(MainViewModel.GetInstance().ListaCatalogos.Where(q => q.Tipo == "Estado").OrderBy(q => q.Descripcion).ToList());
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
                Settings.Bodega = SelectedBodega == null ? "" : SelectedBodega.Codigo;
                Settings.Viaje = SelectedViaje == null ? "" : SelectedViaje.Codigo;
                Settings.EstadoJefe = SelectedEstadoJefe == null ? "" : SelectedEstadoJefe.Codigo;
                Settings.EstadoSupervisor = SelectedEstadoSupervisor == null ? "" : SelectedEstadoSupervisor.Codigo;

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
