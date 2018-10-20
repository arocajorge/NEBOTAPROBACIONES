using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class NoHayOrdenesPendientesViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsEnabled;
        private bool _IsRunning;
        private ApiService apiService;
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

        #region Constructores
        public NoHayOrdenesPendientesViewModel()
        {
            this.IsEnabled = true;
            this.IsRunning = false;
            apiService = new ApiService();
        }
        #endregion

        #region Comandos
        public ICommand BuscarCommand
        {
            get
            {
                return new RelayCommand(Buscar);
            }
        }

        private async void Buscar()
        {
            if (string.IsNullOrEmpty(Settings.UrlConexionActual))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Dispositivo no configurado",
                    "Aceptar");
                return;
            }

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

            var response_cs = await apiService.GetObject<OrdenModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "OrdenTrabajo", "");
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

            var Orden = (OrdenModel)response_cs.Result;
            if (Orden != null && Orden.NumeroOrden != 0)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                MainViewModel.GetInstance().AprobacionGerente = new AprobacionGerenteViewModel();
                Application.Current.MainPage = new AprobacionGerentePage();
            }
            else
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "No existen órdenes pendientes de aprobar",
                    "Aceptar");
                return;
            }
        }
        #endregion
    }
}
