using System;
using System.Windows.Input;
using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class AprobacionPedidoViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsRunning;
        private bool _IsEnabled;
        private ApiService apiService;
        private PedidoModel _orden;
        private bool _IsVisible;
        private bool _IsVisibleLogo;
        #endregion

        #region Propiedades
        public PedidoModel Orden
        {
            get { return this._orden; }
            set { SetValue(ref this._orden, value); }
        }
        public bool IsRunning
        {
            get { return this._IsRunning; }
            set { SetValue(ref this._IsRunning, value); }
        }
        public bool IsVisible
        {
            get { return this._IsVisible; }
            set { 
                SetValue(ref this._IsVisible, value);
                this.IsVisibleLogo = !this.IsVisible;
            }
        }
        public bool IsVisibleLogo
        {
            get { return this._IsVisibleLogo; }
            set { SetValue(ref this._IsVisibleLogo, value); }
        }
        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set { SetValue(ref this._IsEnabled, value); }
        }
        #endregion

        #region Constructor
        public AprobacionPedidoViewModel()
        {
            this.IsEnabled = true;
            this.IsRunning = false;
            this.IsVisible = false;
            this.IsVisibleLogo = true;
            apiService = new ApiService();
            CargarOrden();
        }
        #endregion

        #region Metodos
        private async void CargarOrden()
        {
            try
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

                var response_cs = await apiService.GetObject<PedidoModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "AprobacionPedido", "");
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

                Orden = (PedidoModel)response_cs.Result;
                if (Orden != null && Orden.ID != 0)
                {
                    IsVisible = true;
                    Orden.Titulo = "Pedido de compra No. " + Orden.ID;
                    IsEnabled = true;
                    IsRunning = false;
                    this.IsVisible = true;
                }
                else
                {
                    Orden.Titulo = "No existen pedidos pendientes de aprobar";
                    this.IsVisible = false;
                }
            }
            catch (System.Exception ex)
            {
                this.IsVisible = false;
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");

                return;
            }
        }
        #endregion

        #region Comandos
        public ICommand AprobarCommand
        {
            get
            {
                return new RelayCommand(Aprobar);
            }
        }

        private async void Aprobar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(this.Orden.ComentarioAprobacion))
            {
                this.Orden.Observacion = string.Empty;
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

            this.Orden.Usuario = Settings.IdUsuario;
            this.Orden.Estado = "P";
            var response_sinc = await apiService.Post<PedidoModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "AprobacionPedido",
                this.Orden);
            if (!response_sinc.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    response_sinc.Message,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Actualización de estado exitosa",
                    "Aceptar");


            CargarOrden();

            this.IsEnabled = true;
            this.IsRunning = false;

        }

        public ICommand AnularCommand
        {
            get
            {
                return new RelayCommand(Anular);
            }
        }

        private async void Anular()
        {
            this.IsEnabled = false;
            this.IsRunning = true;

            if (string.IsNullOrEmpty(this.Orden.ComentarioAprobacion))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Ingrese el comentario de anulación",
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


            this.Orden.Estado = "X";

            var response_sinc = await apiService.Post<PedidoModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "AprobacionPedido",
                this.Orden);
            if (!response_sinc.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    response_sinc.Message,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Actualización de estado exitosa",
                    "Aceptar");


            CargarOrden();

            this.IsEnabled = true;
            this.IsRunning = false;

        }

        #endregion
    }
}
