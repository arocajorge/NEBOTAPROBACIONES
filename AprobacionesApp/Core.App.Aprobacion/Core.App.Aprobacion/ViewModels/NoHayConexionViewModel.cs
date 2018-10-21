using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
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
                var response_cs = await apiService.GetObject<UsuarioModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Usuario", "USUARIO=" + Settings.IdUsuario);
                if (!response_cs.IsSuccess)
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    MainViewModel.GetInstance().NoHayConexion = new NoHayConexionViewModel();
                    Application.Current.MainPage = new NavigationPage(new NoHayConexionPage());
                }
                var usuario = (UsuarioModel)response_cs.Result;
                if (usuario != null)
                {
                    if (string.IsNullOrEmpty(usuario.RolApro))
                    {
                        this.IsEnabled = true;
                        this.IsRunning = false;
                        MainViewModel.GetInstance().Login = new LoginViewModel();
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                    }

                    if (usuario.RolApro.Trim().ToUpper() == "G")
                    {
                        this.IsEnabled = true;
                        this.IsRunning = false;
                        MainViewModel.GetInstance().AprobacionGerente = new AprobacionGerenteViewModel();
                        await Application.Current.MainPage.Navigation.PushAsync(new AprobacionGerentePage());
                    }

                    if (usuario.RolApro.Trim().ToUpper() == "J" || usuario.RolApro.Trim().ToUpper() == "S")
                    {
                        if (string.IsNullOrEmpty(Settings.FechaInicio) || Convert.ToDateTime(Settings.FechaFin) != DateTime.Now.Date)
                        {
                            this.IsEnabled = true;
                            this.IsRunning = false;
                            MainViewModel.GetInstance().FiltroJefeSupervisor = new FiltroJefeSupervisorViewModel();
                            Application.Current.MainPage = new NavigationPage(new FiltroJefeSupervisorPage());
                        }
                        else
                        {
                            this.IsEnabled = true;
                            this.IsRunning = false;
                            MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
                            Application.Current.MainPage = new JefeSupervisorMasterPage();
                        }
                    }
                }
            }
            this.IsEnabled = true;
            this.IsRunning = false;
        }
        #endregion
    }
}
