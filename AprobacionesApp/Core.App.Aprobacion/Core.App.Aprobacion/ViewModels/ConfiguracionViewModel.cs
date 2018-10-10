using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class ConfiguracionViewModel : BaseViewModel
    {
        #region Variables
        private string _urlServidor;
        private string _RutaCarpeta;
        private bool _IsRunning;
        private bool _IsEnabled;
        private ApiService apiService;
        #endregion

        #region Propiedades
        public string UrlServidor
        {
            get { return this._urlServidor; }
            set { SetValue(ref this._urlServidor, value); }
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
            this.UrlServidor = string.IsNullOrEmpty(Settings.UrlConexion) ? "http://192.168.1.122" : Settings.UrlConexion;
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
            if (string.IsNullOrEmpty(UrlServidor))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Debe ingresar la url del servidor",
                    "Aceptar");
                return;
            }
            Response con = await apiService.CheckConnection(this.UrlServidor);
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
            #endregion

            Settings.UrlConexion = this.UrlServidor;
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
        }
        #endregion
    }
}
