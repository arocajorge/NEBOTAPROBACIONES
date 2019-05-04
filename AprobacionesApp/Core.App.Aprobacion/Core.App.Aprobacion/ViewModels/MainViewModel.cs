using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Variables
        private ApiService apiService;
        #endregion

        #region ViewModels
        public LoginViewModel Login { get; set; }
        public ConfiguracionViewModel Configuracion { get; set; }
        public AprobacionGerenteViewModel AprobacionGerente { get; set; }
        public NoHayOrdenesPendientesViewModel NoHayOrdenesPendientes { get; set; }
        public PopUpBuscarOrdenViewModel PopUpBuscarOrden { get; set; }
        public JefeSupervisorFiltroViewModel FiltroJefeSupervisor { get; set; }
        public NoHayConexionViewModel NoHayConexion { get; set; }
        public JefeSupervisorOrdenesViewModel JefeSupervisorOrdenes { get; set; }
        public ObservableCollection<JefeSupervisorMenuItemViewModel> Menus { get; set; }
        public JefeSupervisorBitacorasViewModel JefeSupervisorBitacoras { get; set; }
        public JefeSupervisorOrdenViewModel JefeSupervisorOrden { get; set; }
        public JefeSupervisorBitacoraViewModel JefeSupervisorBitacora { get; set; }
        public ReferidosFiltroViewModel ReferidosFiltro { get; set; }
        public ReferidosOrdenNominaViewModel ReferidosOrdenNomina { get; set; }
        public ReferidosOrdenesNominaViewModel ReferidosOrdenesNomina { get; set; }
        public MisOrdenesTrabajoViewModel MisOrdenesTrabajo { get; set; }
        public MisOrdenesTrabajoOrdenViewModel MisOrdenesTrabajoOrden { get; set; }
        public List<CatalogoModel> ListaCatalogos { get; set; }
        public List<ProveedorModel> ListaProveedores { get; set; }
        public CumplimientoFiltroViewModel CumplimientoFiltro { get; set; }
        public CumplimientoBitacorasViewModel CumplimientoBitacoras { get; set; }
        public CumplimientoLineasViewModel CumplimientoLineas { get; set; }
        public ProveedorViewModel Proveedor { get; set; }
        public MisPedidosViewModel MisPedidos { get; set; }
        public MisPedidosPedidoViewModel MisPedidosPedido { get; set; }
        public AprobacionPedidoViewModel AprobacionPedido { get; set; }
        public MiBonoViewModel MiBono { get; set; }
        public CreacionBitacorasViewModel CreacionBitacoras { get; set; }
        public CreacionBitacoraViewModel CreacionBitacora { get; set; }
        public CreacionLineasViewModel CreacionLineas { get; set; }
        public CreacionLineaViewModel CreacionLinea { get; set; }

        #endregion

        #region Combos
        public ComboCatalogosViewModel ComboCatalogos { get; set; }
        public ComboProveedoresViewModel ComboProveedores { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            apiService = new ApiService();
            ListaCatalogos = new List<CatalogoModel>();
            instance = this;
            this.Login = new LoginViewModel();
        }
        #endregion

        #region SingleTon
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            else
                return instance;
        }
        #endregion

        #region Metodos
        public async Task loadMenu(List<UsuarioMenuModel> Lista)
        {
            this.Menus = new ObservableCollection<JefeSupervisorMenuItemViewModel>();

            foreach (var item in Lista)
            {
                this.Menus.Add(new JefeSupervisorMenuItemViewModel
                {
                    Icon = item.Menu == "JefeSupervisorOrdenesPage" ? "ic_filter_1" 
                        : (item.Menu == "JefeSupervisorBitacorasPage" ? "ic_filter_2"
                        : (item.Menu == "JefeSupervisorFiltroPage" ? "ic_location_on"
                        : (item.Menu == "ReferidosOrdenesNominaPage" ? "ic_filter_1"
                        : (item.Menu == "ReferidosFiltroPage" ? "ic_location_on"
                        : (item.Menu == "MisOrdenesTrabajoPage" ? "ic_filter_3" 
                        : (item.Menu == "AprobacionGerentePage" ? "ic_check_box_white"
                        : (item.Menu == "CumplimientoFiltroPage" ? "ic_filter_1"
                        : (item.Menu == "MisPedidosPage" ? "ic_filter_4"
                        : (item.Menu == "ComboProveedoresPage" ? "ic_supervised_user_circle"
                        : (item.Menu == "AprobacionPedidoPage" ? "ic_check_box_white"
                        : (item.Menu == "MiBonoPage" ? "ic_attach_money"
                        : (item.Menu == "CreacionBitacorasPage" ? "ic_filter_5"
                        : "")))))))))))),
                    PageName = item.Menu,
                    Title = item.Menu == "JefeSupervisorOrdenesPage" ? "Ordenes"
                        : (item.Menu == "JefeSupervisorBitacorasPage" ? "Bitácoras"
                        : (item.Menu == "JefeSupervisorFiltroPage" ? "Filtros"
                        : (item.Menu == "ReferidosOrdenesNominaPage" ? "Referidos"
                        : (item.Menu == "ReferidosFiltroPage" ? "Filtros"
                        : (item.Menu == "MisOrdenesTrabajoPage" ? "Mis órdenes de trabajo"
                        : (item.Menu == "AprobacionGerentePage" ? "Aprobación órdenes"
                        : (item.Menu == "CumplimientoFiltroPage" ? "Bitácoras"
                        : (item.Menu == "MisPedidosPage" ? "Mis pedidos"
                        : (item.Menu == "ComboProveedoresPage" ? "Proveedores"
                        : (item.Menu == "AprobacionPedidoPage" ? "Aprobación pedidos"
                        : (item.Menu == "MiBonoPage" ? "Mi bono"
                        : (item.Menu == "CreacionBitacorasPage" ? "Mis bitácoras"
                        : ""))))))))))))
                });
            }
            this.Menus.Add(new JefeSupervisorMenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = "Cerrar sesión"
            });
        }

        public async Task LoadCombos()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.UrlConexionActual))
                {
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
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }

                ListaCatalogos = (List<CatalogoModel>)response_cs.Result;
                ListaCatalogos.Add(new CatalogoModel { Codigo = "", Descripcion = "Todo", Tipo = "Sucursal" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "", Descripcion = "Todo", Tipo = "Bodega" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "", Descripcion = "Todo", Tipo = "Viaje" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "", Descripcion = "Todo", Tipo = "Estado" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "A", Descripcion = "Pendiente", Tipo = "Estado" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "X", Descripcion = "Anulado", Tipo = "Estado" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "P", Descripcion = "Aprobado", Tipo = "Estado" });

                ListaCatalogos.Add(new CatalogoModel { Codigo = "P", Descripcion = "Aprobado", Tipo = "EstadoNomina" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "T", Descripcion = "Aprobado para un viaje", Tipo = "EstadoNomina" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "A", Descripcion = "Pendiente", Tipo = "EstadoNomina" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "X", Descripcion = "Anulado", Tipo = "EstadoNomina" });
                ListaCatalogos.Add(new CatalogoModel { Codigo = "", Descripcion = "Todo", Tipo = "EstadoNomina" });

                ListaCatalogos.ForEach(q => q.Combo = q.Tipo == "Sucursal" ? Enumeradores.eCombo.BARCO : (q.Tipo == "Bodega" ? Enumeradores.eCombo.BODEGA : (q.Tipo == "Viaje" ? Enumeradores.eCombo.VIAJE : (q.Tipo == "Empleado" ? Enumeradores.eCombo.SOLICITANTE: Enumeradores.eCombo.PROVEEDOR))));

                var response_pro = await apiService.GetList<ProveedorModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Proveedor", "");
                if (!response_pro.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_pro.Message,
                        "Aceptar");
                    return;
                }
                ListaProveedores = (List<ProveedorModel>)response_pro.Result;                
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");

                return;
            }
        }

        public void ModificarProveedor(string Codigo, decimal Duracion)
        {
            foreach (var item in ListaProveedores)
            {
                if (item.Codigo == Codigo)
                    item.Duracion = Duracion;
            }
        }

        #endregion

        #region Comandos
        public ICommand BuscarOrdenCommand
        {
            get
            {
                return new RelayCommand(BuscarOrden);
            }
        }

        private async void BuscarOrden()
        {
            try
            {
                PopUpBuscarOrden = new PopUpBuscarOrdenViewModel();
                await PopupNavigation.PushAsync(new PopUpBuscarOrdenPage());
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                       "Alerta",
                       ex.Message,
                       "Aceptar");
                return;
            }
            
        }

        public ICommand NuevaOrdenCommand
        {
            get
            {
                return new RelayCommand(NuevaOrden);
            }
        }

        private async void NuevaOrden()
        {
            try
            {
                OrdenModel Orden = new OrdenModel { Fecha = DateTime.Now.Date, Titulo = "Nueva Orden", IdSucursal = Settings.Sucursal, IdViaje = Settings.Viaje, NomCentroCosto = Settings.NombreSucursal, NomViaje = Settings.NombreViaje,
                    IdSolicitante = Settings.Solicitante,
                    IdBodega = Settings.Bodega,
                        NombreBodega = Settings.NombreBodega,
                    NombreSolicitante = Settings.NombreSolicitante
                };
                GetInstance().MisOrdenesTrabajoOrden = new MisOrdenesTrabajoOrdenViewModel(Orden);
                await App.Navigator.PushAsync(new MisOrdenesTrabajoOrdenPage());
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                       "Alerta",
                       ex.Message,
                       "Aceptar");
                return;
            }

        }

        public ICommand NuevaPedidoCommand
        {
            get
            {
                return new RelayCommand(NuevaPedido);
            }
        }

        private async void NuevaPedido()
        {
            try
            {
                var Orden = new PedidoModel { Fecha = DateTime.Now.Date, Titulo = "Nuevo pedido", IdSucursal = Settings.Sucursal, IdViaje = Settings.Viaje, NombreSucursal = Settings.NombreSucursal, NombreViaje = Settings.NombreViaje,
                    IdSolicitante = Settings.Solicitante,
                    IdBodega = Settings.Bodega,
                    NombreBodega = Settings.NombreBodega, NombreEmpleado = Settings.NombreSolicitante };
                GetInstance().MisPedidosPedido = new MisPedidosPedidoViewModel(Orden);
                await App.Navigator.PushAsync(new MisPedidosPedidoPage());
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                       "Alerta",
                       ex.Message,
                       "Aceptar");
                return;
            }

        }


        public ICommand NuevaBitacoraCommand
        {
            get
            {
                return new RelayCommand(NuevaBitacora);
            }
        }

        private async void NuevaBitacora()
        {
            try
            {
                var Orden = new BitacoraModel
                {
                    Barco = Settings.Sucursal,
                    NomBarco = Settings.NombreSucursal,
                    Viaje = Settings.Viaje,
                    NomViaje = Settings.NombreViaje,
                    FechaArribo = DateTime.Now.Date,
                    FechaZarpe = DateTime.Now.Date,
                    Titulo = "Nueva bitácora"
                };

                GetInstance().CreacionBitacora = new CreacionBitacoraViewModel(Orden);
                await App.Navigator.PushAsync(new CreacionBitacoraPage());
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                       "Alerta",
                       ex.Message,
                       "Aceptar");
                return;
            }

        }

        public ICommand NuevaLineaCommand
        {
            get
            {
                return new RelayCommand(NuevaLinea);
            }
        }

        private async void NuevaLinea()
        {
            try
            {
                var Orden = new BitacoraModel
                {
                    Barco = Settings.Sucursal,
                    NomBarco = Settings.NombreSucursal,
                    Viaje = Settings.Viaje,
                    NomViaje = Settings.NombreViaje,
                    FechaArribo = DateTime.Now.Date,
                    FechaZarpe = DateTime.Now.Date,
                    Titulo = "Nueva obra"
                };

                GetInstance().CreacionLinea = new CreacionLineaViewModel(Orden);
                await App.Navigator.PushAsync(new CreacionLineaPage());
            }
            catch (System.Exception ex)
            {
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
