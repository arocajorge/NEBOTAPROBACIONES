﻿namespace Core.App.Aprobacion.ViewModels
{
    using Core.App.Aprobacion.Helpers;
    using Core.App.Aprobacion.Models;
    using Core.App.Aprobacion.Services;
    using Core.App.Aprobacion.Views;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Xamarin.Forms;
    public class LoginViewModel : BaseViewModel
    {
        #region Variables
        private string _usuario;
        private string _contrasenia;
        private bool _IsEnabled;
        private bool _IsRunning;
        private ApiService apiService;
        #endregion

        #region Propiedades
        public string usuario
        {
            get { return this._usuario; }
            set { SetValue(ref this._usuario, value); }
        }
        public string contrasenia
        {
            get { return this._contrasenia; }
            set { SetValue(ref this._contrasenia, value); }
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
        public LoginViewModel()
        {
            IsEnabled = true;
            usuario = string.IsNullOrEmpty(Settings.IdUsuario) ? "" : Settings.IdUsuario;
            contrasenia = string.IsNullOrEmpty(Settings.IdUsuario) ? "" : string.Empty;
            apiService = new ApiService();
        }
        #endregion

        #region Comandos
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }
        private async void Login()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(this.usuario))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Debe ingresar un usuario",
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.contrasenia))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Debe ingresar una contraseña",
                    "Aceptar");
                return;
            }
            bool ValidarUsuarioServicio = true;
            if (this.usuario.ToLower() == "indigoadmin" && this.contrasenia.ToLower() == "indigo")
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                this.usuario = string.Empty;
                this.contrasenia = string.Empty;
                ValidarUsuarioServicio = false;
                Settings.IdUsuario = "";
                MainViewModel.GetInstance().Configuracion = new ConfiguracionViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new ConfiguracionPage());
            }
            if (ValidarUsuarioServicio)
            {
                Response con = await apiService.CheckConnection(Settings.UrlConexion);
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

                if (string.IsNullOrEmpty(Settings.UrlConexion))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Dispositivo no configurado",
                        "Aceptar");
                    return;
                }

                var response_cs = await apiService.GetObject<UsuarioModel>(Settings.UrlConexion, Settings.RutaCarpeta, "Usuario", "USUARIO="+this.usuario+"&CLAVE="+this.contrasenia);
                if (!response_cs.IsSuccess)
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }
                var usuario = (UsuarioModel)response_cs.Result;
                if (usuario != null)
                {
                    if (string.IsNullOrEmpty(usuario.RolApro))
                    {
                        this.IsEnabled = true;
                        this.IsRunning = false;
                        await Application.Current.MainPage.DisplayAlert(
                       "Alerta",
                       "El usuario no tiene permisos para el uso de esta aplicación",
                       "Aceptar");
                        return;
                    }

                    if (usuario.RolApro.Trim().ToUpper() == "G")
                    {
                        this.IsEnabled = true;
                        this.IsRunning = false;
                        Settings.IdUsuario = this.usuario;
                        MainViewModel.GetInstance().AprobacionGerente = new AprobacionGerenteViewModel();
                        await Application.Current.MainPage.Navigation.PushAsync(new AprobacionGerentePage());
                    }
                }
                else
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Credenciales incorrectas",
                        "Aceptar");
                    return;
                }
                this.IsEnabled = true;
                this.IsRunning = false;
            }
        }
        #endregion
    }
}