using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class NoHayConexionViewModel : BaseViewModel
    {
        #region Variables
        ApiService apiService;
        private bool _IsEnabled;
        private bool _IsRunning;
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
        #endregion

        #region Constructor
        public NoHayConexionViewModel()
        {
            apiService = new ApiService();
        }
        #endregion

        #region Comandos
        public ICommand ReConectarCommand
        {
            get
            {
                return new RelayCommand(ReConectar);
            }
        }

        private async void ReConectar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(Settings.IdUsuario))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
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
            }
            this.IsEnabled = true;
            this.IsRunning = false;
        }
        #endregion
    }
}
