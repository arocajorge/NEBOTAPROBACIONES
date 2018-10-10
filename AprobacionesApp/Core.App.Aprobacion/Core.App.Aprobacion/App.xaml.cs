using Core.App.Aprobacion.Helpers;
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
        #region Constructor
        public App()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(Settings.IdUsuario))
            {
                MainViewModel.GetInstance().Login = new LoginViewModel();
                MainPage = new NavigationPage(new LoginPage());
            }else
            {
                MainViewModel.GetInstance().AprobacionGerente = new AprobacionGerenteViewModel();
                MainPage = new NavigationPage(new AprobacionGerentePage());
            }            
        }
        #endregion

        #region Metodos
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
			// Handle when your app resumes
		}
        #endregion
	}
}
