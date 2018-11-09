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

                switch (this.PageName)
                {
                    case "JefeSupervisorFiltrosPage":

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
                        #region Limpio los settings
                        #endregion
                        MainViewModel.GetInstance().Login = new LoginViewModel();
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                        break;
                    case "JefeSupervisorOrdenesPage":
                        MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
                        await App.Navigator.PushAsync(new JefeSupervisorOrdenesPage());
                        break;
                    case "JefeSupervisorBitacorasPage":
                        if (string.IsNullOrEmpty(Settings.Sucursal) || string.IsNullOrEmpty(Settings.Viaje))
                        {
                            await Application.Current.MainPage.DisplayAlert(
                              "Alerta",
                              "Para ingresar a bitácoras debe escoger barco y viaje",
                              "Aceptar");

                            Response conection = await apiService.CheckConnection(Settings.UrlConexionActual);
                            if (!conection.IsSuccess)
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

                            var response_cs2 = await apiService.GetObject<UsuarioModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Usuario", "USUARIO=" + Settings.IdUsuario);
                            if (!response_cs2.IsSuccess)
                            {
                                MainViewModel.GetInstance().NoHayConexion = new NoHayConexionViewModel();
                                Application.Current.MainPage = new NavigationPage(new NoHayConexionPage());
                                return;
                            }
                            var usuario1 = (UsuarioModel)response_cs2.Result;
                            if (usuario1 != null)
                            {
                                MainViewModel.GetInstance().FiltroJefeSupervisor = new JefeSupervisorFiltroViewModel(usuario1.RolApro.ToUpper());
                                Application.Current.MainPage = new NavigationPage(new JefeSupervisorFiltroPage());
                            }
                        }
                        else
                        {
                            MainViewModel.GetInstance().JefeSupervisorBitacoras = new JefeSupervisorBitacorasViewModel();
                            Application.Current.MainPage = new NavigationPage(new JefeSupervisorBitacorasPage());
                        }
                        break;

                    case "ReferidosFiltroPage":
                        MainViewModel.GetInstance().ReferidosFiltro = new ReferidosFiltroViewModel();                        
                        Application.Current.MainPage = new NavigationPage(new ReferidosFiltroPage());
                        break;
                    case "ReferidosOrdenesNominaPage":
                        MainViewModel.GetInstance().ReferidosOrdenesNomina = new ReferidosOrdenesNominaViewModel();
                        Application.Current.MainPage = new ReferidosMasterPage();
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
