﻿using Core.App.Aprobacion.Helpers;
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
            IsEnabled = true;
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
                    await Application.Current.MainPage.DisplayAlert(
                         "Alerta",
                         con.Message,
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
                        MainViewModel.GetInstance().Login = new LoginViewModel();
                        Application.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    await MainViewModel.GetInstance().LoadCombos();
                    await MainViewModel.GetInstance().loadMenu(usuario.LstMenu);
                    this.IsEnabled = true;
                    this.IsRunning = false;

                    if (usuario.MenuFiltro == "AprobacionGerentePage")
                    {
                        MainViewModel.GetInstance().AprobacionGerente = new AprobacionGerenteViewModel();
                        Application.Current.MainPage = new GerenteMasterPage();
                        return;
                    }
                    if (usuario.MenuFiltro == "JefeSupervisorFiltroPage")
                    {
                        MainViewModel.GetInstance().FiltroJefeSupervisor = new JefeSupervisorFiltroViewModel(usuario.RolApro.Trim().ToUpper());
                        Application.Current.MainPage = new NavigationPage(new JefeSupervisorFiltroPage());
                        return;
                    }
                    if (usuario.MenuFiltro == "JefeSupervisorOrdenesPage")
                    {
                        if (string.IsNullOrEmpty(Settings.FechaInicio))
                        {
                            MainViewModel.GetInstance().FiltroJefeSupervisor = new JefeSupervisorFiltroViewModel(usuario.RolApro.Trim().ToUpper());
                            Application.Current.MainPage = new NavigationPage(new JefeSupervisorFiltroPage());
                            return;
                        }
                        else
                        {
                            MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
                            Application.Current.MainPage = new JefeSupervisorMasterPage();
                            return;
                        }
                    }
                    if (usuario.MenuFiltro == "ReferidosOrdenesNominaPage")
                    {
                        if (string.IsNullOrEmpty(Settings.FechaInicio))
                        {
                            MainViewModel.GetInstance().ReferidosFiltro = new ReferidosFiltroViewModel();
                            Application.Current.MainPage = new NavigationPage(new ReferidosFiltroPage());
                            return;
                        }
                        else
                        {
                            MainViewModel.GetInstance().ReferidosOrdenesNomina = new ReferidosOrdenesNominaViewModel();
                            Application.Current.MainPage = new ReferidosMasterPage();
                            return;
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
