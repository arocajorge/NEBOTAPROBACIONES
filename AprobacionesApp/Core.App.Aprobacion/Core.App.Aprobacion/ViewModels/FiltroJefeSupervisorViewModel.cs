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
            get { return this._BodegaSelectedIndex; }
            set
            {
                SetValue(ref this._BodegaSelectedIndex, value);
                SelectedBodega = _BodegaSelectedIndex < 0 ? new CatalogoModel() : ListaViaje[_BodegaSelectedIndex];
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
                    SelectedEstadoJefe = _ListaEstadoJefe.Where(q => q.Codigo == Settings.EstadoJefe).FirstOrDefault();
                    EstadoJefeSelectedIndex = _ListaEstadoJefe.IndexOf(SelectedEstadoJefe == null ? new CatalogoModel() : SelectedEstadoJefe);
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
                SelectedEstadoSupervisor = _EstadoJefeSelectedIndex < 0 ? new CatalogoModel() : ListaViaje[_EstadoJefeSelectedIndex];
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
                    EstadoSupervisorSelectedIndex = _ListaEstadoSupervisor.IndexOf(SelectedEstadoSupervisor == null ? new CatalogoModel() : SelectedEstadoSupervisor);
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
                SelectedEstadoSupervisor = _EstadoSupervisorSelectedIndex < 0 ? new CatalogoModel() : ListaViaje[_EstadoSupervisorSelectedIndex];
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
            FechaInicio = DateTime.Now.Date.AddMonths(-2);
            FechaFin = DateTime.Now.Date;
            switch (ROL_APRO)
            {
                case "J":
                    Settings.EstadoJefe = "A";
                    Settings.EstadoSupervisor = "";
                    break;
                case "S":
                    Settings.EstadoJefe = "";
                    Settings.EstadoSupervisor = "A";
                    break;
            }
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
                ListaViaje.Add(new CatalogoModel
                {
                    Tipo = "Viaje",
                    Codigo = "",
                    Descripcion = "TODO",
                });

                ListaEstadoJefe.Add(new CatalogoModel
                {
                    Tipo = "EstadoJefe",
                    Codigo = "",
                    Descripcion = "Todo",
                });
                ListaEstadoJefe.Add(new CatalogoModel
                {
                    Tipo = "EstadoJefe",
                    Codigo = "A",
                    Descripcion = "Pendiente",
                });
                ListaEstadoJefe.Add(new CatalogoModel
                {
                    Tipo = "EstadoJefe",
                    Codigo = "x",
                    Descripcion = "Anulado",
                });
                ListaEstadoJefe.Add(new CatalogoModel
                {
                    Tipo = "EstadoJefe",
                    Codigo = "P",
                    Descripcion = "Aprobado",
                });


                ListaEstadoSupervisor.Add(new CatalogoModel
                {
                    Tipo = "EstadoSupervisor",
                    Codigo = "",
                    Descripcion = "Todo",
                });
                ListaEstadoSupervisor.Add(new CatalogoModel
                {
                    Tipo = "EstadoSupervisor",
                    Codigo = "A",
                    Descripcion = "Pendiente",
                });
                ListaEstadoSupervisor.Add(new CatalogoModel
                {
                    Tipo = "EstadoSupervisor",
                    Codigo = "x",
                    Descripcion = "Anulado",
                });
                ListaEstadoSupervisor.Add(new CatalogoModel
                {
                    Tipo = "EstadoSupervisor",
                    Codigo = "P",
                    Descripcion = "Aprobado",
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
                IsRunning = true;
                IsEnabled = false;

                Settings.FechaInicio = FechaInicio.ToShortDateString();
                Settings.FechaFin = FechaFin.ToShortDateString();
                Settings.Sucursal = SelectedSucursal == null ? "" : SelectedSucursal.Codigo;
                Settings.Bodega = SelectedBodega == null ? "" : SelectedBodega.Codigo;
                Settings.Viaje = SelectedViaje == null ? "" : SelectedViaje.Codigo;
                Settings.NumeroOrden = NumeroOrden;

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
