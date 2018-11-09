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
    public class ReferidosFiltroViewModel : BaseViewModel
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

        private ObservableCollection<CatalogoModel> _ListaEstadoGerente;
        private CatalogoModel _SelectedEstadoGerente;
        private int _EstadoGerenteSelectedIndex;
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
            set
            {
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

        public ObservableCollection<CatalogoModel> ListaEstadoGerente
        {
            get { return this._ListaEstadoGerente; }
            set
            {
                SetValue(ref this._ListaEstadoGerente, value);
                if (SetSettings)
                {
                    SelectedEstadoGerente = ListaEstadoGerente.Where(q => q.Codigo == Settings.EstadoGerente).FirstOrDefault();
                    if (SelectedEstadoGerente != null)
                        EstadoGerenteSelectedIndex = ListaEstadoGerente.IndexOf(SelectedEstadoGerente);
                }
            }
        }
        public CatalogoModel SelectedEstadoGerente
        {
            get { return this._SelectedEstadoGerente; }
            set { SetValue(ref this._SelectedEstadoGerente, value); }
        }
        public int EstadoGerenteSelectedIndex
        {
            get { return this._EstadoGerenteSelectedIndex; }
            set
            {
                SetValue(ref this._EstadoGerenteSelectedIndex, value);
                SelectedEstadoGerente = _EstadoGerenteSelectedIndex < 0 ? new CatalogoModel() : ListaEstadoGerente[_EstadoGerenteSelectedIndex];
            }
        }
        #endregion

        #region Constructor
        public ReferidosFiltroViewModel()
        {            
            apiService = new ApiService();
            IsEnabled = true;
            SelectedEstadoGerente = new CatalogoModel();
            SelectedSucursal = new CatalogoModel();
            SetSettings = true;
            if (string.IsNullOrEmpty(Settings.FechaInicio))
            {
                FechaInicio = DateTime.Now.Date.AddMonths(-1);
                Settings.Sucursal = string.Empty;
                Settings.EstadoGerente = string.Empty;
            }else
                FechaInicio = Convert.ToDateTime(Settings.FechaInicio);
            CargarCombos();
            SetSettings = false;
        }
        #endregion

        #region Metodos
        private void CargarCombos()
        {
            ListaSucursal = new ObservableCollection<CatalogoModel>(MainViewModel.GetInstance().ListaCatalogos.Where(q => q.Tipo == "Sucursal").OrderBy(q => q.Descripcion).ToList());
            ListaEstadoGerente = new ObservableCollection<CatalogoModel>(MainViewModel.GetInstance().ListaCatalogos.Where(q => q.Tipo == "EstadoNomina").OrderBy(q => q.Descripcion).ToList());
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
                Settings.EstadoGerente = SelectedEstadoGerente == null ? "" : SelectedEstadoGerente.Codigo;

                IsRunning = false;
                IsEnabled = true;

                MainViewModel.GetInstance().ReferidosOrdenesNomina = new ReferidosOrdenesNominaViewModel();
                Application.Current.MainPage = new ReferidosMasterPage();
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
