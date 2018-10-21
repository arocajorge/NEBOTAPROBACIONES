using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.ViewModels;
using Core.App.Aprobacion.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Core.App.Aprobacion
{
	public partial class App : Application
	{
        #region Variables
        private ApiService apiService;
        #endregion
        #region Propiedades
        public static NavigationPage Navigator { get; internal set; }
        public static JefeSupervisorMasterPage Master { get; internal set; }
        #endregion

        #region Constructor
        public App()
        {
            InitializeComponent();
            apiService = new ApiService();
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
            if (string.IsNullOrEmpty(Settings.IdUsuario))
            {
                MainViewModel.GetInstance().Login = new LoginViewModel();
                MainPage = new NavigationPage(new LoginPage());
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
                    }
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }
            }

            var response_cs = await apiService.GetObject<UsuarioModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Usuario", "USUARIO=" + Settings.IdUsuario );
            if (!response_cs.IsSuccess)
            {
                MainViewModel.GetInstance().NoHayConexion = new NoHayConexionViewModel();
                MainPage = new NavigationPage(new NoHayConexionPage());
            }
            var usuario = (UsuarioModel)response_cs.Result;
            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.RolApro))
                {
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    MainPage = new NavigationPage(new LoginPage());
                }

                if (usuario.RolApro.Trim().ToUpper() == "G")
                {
                    MainViewModel.GetInstance().AprobacionGerente = new AprobacionGerenteViewModel();
                    await Application.Current.MainPage.Navigation.PushAsync(new AprobacionGerentePage());
                }

                if (usuario.RolApro.Trim().ToUpper() == "J" || usuario.RolApro.Trim().ToUpper() == "S")
                {
                    if (string.IsNullOrEmpty(Settings.FechaInicio) || Convert.ToDateTime(Settings.FechaFin) != DateTime.Now.Date)
                    {
                        MainViewModel.GetInstance().FiltroJefeSupervisor = new FiltroJefeSupervisorViewModel();
                        MainPage = new NavigationPage(new FiltroJefeSupervisorPage());
                    }
                    else
                    {
                        MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
                        MainPage = new JefeSupervisorMasterPage();
                    }                    
                }
            }
        }
        #endregion
    }
}
