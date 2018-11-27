using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.ViewModels;
using Core.App.Aprobacion.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Core.App.Aprobacion
{
    public partial class App : Application
	{
        #region Variables
        private ApiService apiService;
        #endregion
        #region Propiedades
        public static NavigationPage Navigator { get; internal set; }
        public static JefeSupervisorMasterPage MasterJefeSupervisor { get; set; }
        public static ReferidosMasterPage MasterReferidos { get; set; }
        public static GerenteMasterPage MasterGerente { get; set; }
        public static CumplimientoMasterPage MasterCumplimiento { get; set; }
        #endregion

        #region Constructor
        public App()
        {
            InitializeComponent();
            apiService = new ApiService();
            MainPage = new NavigationPage(new LoadingPage());
            ValidarLogin();
        }
        #endregion

        #region Acciones
        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
            ValidarLogin();
        }
        #endregion

        #region Metodos
        private async void ValidarLogin()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.IdUsuario))
                {
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    MainPage = new NavigationPage(new LoginPage());
                    return;
                }
                else
                {
                    Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
                    if (!con.IsSuccess)
                    {
                        string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                        con = await apiService.CheckConnection(UrlDistinto);
                        if (!con.IsSuccess)
                        {
                            MainViewModel.GetInstance().NoHayConexion = new NoHayConexionViewModel();
                            MainPage = new NavigationPage(new NoHayConexionPage());
                            return;
                        }
                        else
                            Settings.UrlConexionActual = UrlDistinto;
                    }

                    var response_cs = await apiService.GetObject<UsuarioModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Usuario", "USUARIO=" + Settings.IdUsuario);
                    if (!response_cs.IsSuccess)
                    {
                        MainViewModel.GetInstance().NoHayConexion = new NoHayConexionViewModel();
                        MainPage = new NavigationPage(new NoHayConexionPage());
                        return;
                    }
                    var usuario = (UsuarioModel)response_cs.Result;
                    if (usuario != null)
                    {
                        if (string.IsNullOrEmpty(usuario.RolApro))
                        {
                            MainViewModel.GetInstance().Login = new LoginViewModel();
                            MainPage = new NavigationPage(new LoginPage());
                            return;
                        }

                        await MainViewModel.GetInstance().LoadCombos();
                        await MainViewModel.GetInstance().loadMenu(usuario.LstMenu);

                        if (usuario.MenuFiltro == "AprobacionGerentePage")
                        {
                            MainViewModel.GetInstance().AprobacionGerente = new AprobacionGerenteViewModel();
                            MainPage = new GerenteMasterPage();
                            return;
                        }
                        if (usuario.MenuFiltro == "JefeSupervisorFiltroPage")
                        {
                            MainViewModel.GetInstance().FiltroJefeSupervisor = new JefeSupervisorFiltroViewModel(usuario.RolApro.Trim().ToUpper());
                            MainPage = new NavigationPage(new JefeSupervisorFiltroPage());
                            return;
                        }
                        if (usuario.MenuFiltro == "JefeSupervisorOrdenesPage")
                        {
                            if (string.IsNullOrEmpty(Settings.FechaInicio))
                            {
                                MainViewModel.GetInstance().FiltroJefeSupervisor = new JefeSupervisorFiltroViewModel(usuario.RolApro.Trim().ToUpper());
                                MainPage = new NavigationPage(new JefeSupervisorFiltroPage());
                                return;
                            }
                            else
                            {
                                MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
                                MainPage = new JefeSupervisorMasterPage();
                                return;
                            }
                        }
                        if (usuario.MenuFiltro == "ReferidosOrdenesNominaPage")
                        {
                            if (string.IsNullOrEmpty(Settings.FechaInicio))
                            {
                                MainViewModel.GetInstance().ReferidosFiltro = new ReferidosFiltroViewModel();
                                MainPage = new NavigationPage(new ReferidosFiltroPage());
                                return;
                            }
                            else
                            {
                                MainViewModel.GetInstance().ReferidosOrdenesNomina = new ReferidosOrdenesNominaViewModel();
                                MainPage = new ReferidosMasterPage();
                                return;
                            }
                        }
                    }
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
