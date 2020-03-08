using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class JefeSupervisorMenuItemViewModel
    {
        #region Variables
        private ApiService apiService;
        #endregion

        #region Propiedades
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        #endregion

        #region Constructor
        public JefeSupervisorMenuItemViewModel()
        {
            apiService = new ApiService();
        }
        #endregion

        #region Comandos
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        

        private async void Navigate()
        {
            try
            {

                if(App.MasterJefeSupervisor != null)
                    App.MasterJefeSupervisor.IsPresented = false;
                if(App.MasterReferidos != null)
                    App.MasterReferidos.IsPresented = false;
                if (App.MasterGerente != null)
                    App.MasterGerente.IsPresented = false;
                if (App.MasterCumplimiento != null)
                    App.MasterCumplimiento.IsPresented = false;
                if (App.NoHayOrdenes != null)
                    App.NoHayOrdenes.IsPresented = false;

                switch (this.PageName)
                {
                    case "JefeSupervisorFiltroPage":

                        Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
                        if (!con.IsSuccess)
                        {
                            string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                            con = await apiService.CheckConnection(UrlDistinto);
                            if (!con.IsSuccess)
                            {
                                MainViewModel.GetInstance().NoHayConexion = new NoHayConexionViewModel();
                                Application.Current.MainPage = new NavigationPage(new NoHayConexionPage());
                                return;
                            }
                            else
                                Settings.UrlConexionActual = UrlDistinto;
                        }

                        var response_cs = await apiService.GetObject<UsuarioModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Usuario", "USUARIO=" + Settings.IdUsuario);
                        if (!response_cs.IsSuccess)
                        {
                            MainViewModel.GetInstance().NoHayConexion = new NoHayConexionViewModel();
                            Application.Current.MainPage = new NavigationPage(new NoHayConexionPage());
                            return;
                        }
                        var usuario = (UsuarioModel)response_cs.Result;
                        if (usuario != null)
                        {
                            MainViewModel.GetInstance().FiltroJefeSupervisor = new JefeSupervisorFiltroViewModel(usuario.RolApro.ToUpper());
                            Application.Current.MainPage = new NavigationPage(new JefeSupervisorFiltroPage());
                        }
                        break;
                    case "LoginPage":
                        MainViewModel.GetInstance().Login = new LoginViewModel();
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                        break;
                    case "JefeSupervisorOrdenesPage":
                        MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
                        Application.Current.MainPage = new JefeSupervisorMasterPage();
                        break;
                    case "AprobacionOrdenPage":
                        MainViewModel.GetInstance().AprobacionOrden = new AprobacionOrdenViewModel();
                        await App.Navigator.PushAsync(new AprobacionOrdenPage());
                        break;
                    case "AprobacionPedidoPage":
                        MainViewModel.GetInstance().AprobacionPedido = new AprobacionPedidoViewModel();
                        await App.Navigator.PushAsync(new AprobacionPedidoPage());
                        break;
                    case "JefeSupervisorBitacorasPage":
                        if (string.IsNullOrEmpty(Settings.Sucursal) || string.IsNullOrEmpty(Settings.Viaje))
                        {
                            await Application.Current.MainPage.DisplayAlert(
                              "Alerta",
                              "Para ingresar a bitácoras debe escoger barco y viaje",
                              "Aceptar");

                            MainViewModel.GetInstance().FiltroJefeSupervisor = new JefeSupervisorFiltroViewModel(Settings.RolApro.ToUpper());
                            Application.Current.MainPage = new NavigationPage(new JefeSupervisorFiltroPage());
                        }
                        else
                        {
                            MainViewModel.GetInstance().JefeSupervisorBitacoras = new JefeSupervisorBitacorasViewModel();
                            await App.Navigator.PushAsync(new JefeSupervisorBitacorasPage());
                        }
                        break;

                    case "ReferidosFiltroPage":
                        MainViewModel.GetInstance().ReferidosFiltro = new ReferidosFiltroViewModel();                        
                        Application.Current.MainPage = new NavigationPage(new ReferidosFiltroPage());
                        break;
                    case "ReferidosOrdenesNominaPage":
                        MainViewModel.GetInstance().ReferidosOrdenesNomina = new ReferidosOrdenesNominaViewModel();
                        await App.Navigator.PushAsync(new ReferidosOrdenesNominaPage());
                        break;
                    case "MisOrdenesTrabajoPage":
                        MainViewModel.GetInstance().MisOrdenesTrabajo = new MisOrdenesTrabajoViewModel();
                        await App.Navigator.PushAsync(new MisOrdenesTrabajoPage());
                        break;
                    case "MisPedidosPage":
                        MainViewModel.GetInstance().MisPedidos = new MisPedidosViewModel();
                        await App.Navigator.PushAsync(new MisPedidosPage());
                        break;
                    case "CumplimientoFiltroPage":
                        MainViewModel.GetInstance().CumplimientoFiltro = new CumplimientoFiltroViewModel();
                        await App.Navigator.PushAsync(new CumplimientoFiltroPage());
                        break;
                    case "AprobacionGerentePage":
                        MainViewModel.GetInstance().AprobacionGerente = new AprobacionGerenteViewModel();
                        Application.Current.MainPage = new GerenteMasterPage();
                        break;
                    case "ComboProveedoresPage":
                        MainViewModel.GetInstance().ComboProveedores = new ComboProveedoresViewModel(true);
                        await App.Navigator.PushAsync(new ComboProveedoresPage());
                        break;
                    case "MiBonoPage":
                        if (string.IsNullOrEmpty(Settings.Sucursal) || string.IsNullOrEmpty(Settings.Viaje))
                        {
                            await Application.Current.MainPage.DisplayAlert(
                              "Alerta",
                              "Para ingresar a mi bono debe escoger barco y viaje",
                              "Aceptar");

                            MainViewModel.GetInstance().FiltroJefeSupervisor = new JefeSupervisorFiltroViewModel(Settings.RolApro.ToUpper());
                            Application.Current.MainPage = new NavigationPage(new JefeSupervisorFiltroPage());
                        }
                        else
                        {
                            MainViewModel.GetInstance().MiBono = new MiBonoViewModel();
                            await App.Navigator.PushAsync(new MiBonoPage());
                        }
                        break;
                    case "CreacionBitacorasPage":
                        MainViewModel.GetInstance().CreacionBitacoras = new CreacionBitacorasViewModel();
                        await App.Navigator.PushAsync(new CreacionBitacorasPage());
                        break;
                }
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
