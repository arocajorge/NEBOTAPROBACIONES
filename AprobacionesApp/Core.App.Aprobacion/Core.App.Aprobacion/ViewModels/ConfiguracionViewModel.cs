using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class ConfiguracionViewModel : BaseViewModel
    {
        #region Variables
        private string _urlServidorExterno;
        private string _urlServidorInterno;
        private string _RutaCarpeta;
        private bool _IsRunning;
        private bool _IsEnabled;
        private ApiService apiService;
        #endregion

        #region Propiedades
        public string UrlServidorExterno
        {
            get { return this._urlServidorExterno; }
            set { SetValue(ref this._urlServidorExterno, value); }
        }
        public string UrlServidorInterno
        {
            get { return this._urlServidorInterno; }
            set { SetValue(ref this._urlServidorInterno, value); }
        }
        public string RutaCarpeta
        {
            get { return this._RutaCarpeta; }
            set { SetValue(ref this._RutaCarpeta, value); }
        }
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
        public ConfiguracionViewModel()
        {
            this.IsEnabled = true;
            apiService = new ApiService();
            this.UrlServidorExterno = string.IsNullOrEmpty(Settings.UrlConexionExterna) ? "http://190.110.211.82:20000" : Settings.UrlConexionExterna;
            this.UrlServidorInterno = string.IsNullOrEmpty(Settings.UrlConexionInterna) ? "http://192.168.1.7:20000" : Settings.UrlConexionInterna;
            this.RutaCarpeta = string.IsNullOrEmpty(Settings.RutaCarpeta) ? "/Api" : Settings.RutaCarpeta;
        }
        #endregion

        #region Metodos
        public ICommand SincronizarCommand
        {
            get
            {
                return new RelayCommand(Sincronizar);
            }
        }

        private async void Sincronizar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;

            #region Validaciones
            if (string.IsNullOrEmpty(UrlServidorExterno))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Debe ingresar la url del servidor externo",
                    "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(UrlServidorInterno))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Debe ingresar la url del servidor interno",
                    "Aceptar");
                return;
            }

            Response con = await apiService.CheckConnection(this.UrlServidorExterno);
            if (!con.IsSuccess)
            {
                con = await apiService.CheckConnection(this.UrlServidorInterno);
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
                    Settings.UrlConexionActual = this.UrlServidorInterno;
            }
            else
                Settings.UrlConexionActual = this.UrlServidorExterno;
            #endregion

            Settings.UrlConexionExterna = this.UrlServidorExterno;
            Settings.UrlConexionInterna = this.UrlServidorInterno;
            Settings.RutaCarpeta = this.RutaCarpeta;            
           
            #region Limpio los settings
            Settings.IdUsuario = string.Empty;
            #endregion

            this.IsEnabled = true;
            this.IsRunning = false;
            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Configuración OK",
                    "Aceptar");

            MainViewModel.GetInstance().Login = new LoginViewModel();
            Application.Current.MainPage = new LoginPage();
        }
        #endregion
    }
}
