using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class ReferidosOrdenNominaViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsRunning;
        private bool _IsEnabled;
        private ApiService apiService;
        private OrdenNominaModel _orden;
        private int _Height;
        private string _color;
        private string _estado;
        private bool _VisibleBotones;
        private ObservableCollection<OrdenNominaDetalleModel> _ListaDetalle;
        #endregion

        #region Propiedades
        public string Color
        {
            get { return this._color; }
            set { SetValue(ref this._color, value); }
        }
        public OrdenNominaModel Orden
        {
            get { return this._orden; }
            set { SetValue(ref this._orden, value); }
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
        public int Height
        {
            get { return this._Height; }
            set { SetValue(ref this._Height, value); }
        }
        public string Estado
        {
            get { return this._estado; }
            set { SetValue(ref this._estado, value); }
        }
        public bool VisibleBotones
        {
            get { return this._VisibleBotones; }
            set
            {
                SetValue(ref this._VisibleBotones, value);
            }
        }
        public ObservableCollection<OrdenNominaDetalleModel> ListaDetalle
        {
            get { return this._ListaDetalle; }
            set
            {
                SetValue(ref this._ListaDetalle, value);
            }
        }
        #endregion

        #region Constructor
        public ReferidosOrdenNominaViewModel(OrdenNominaModel model)
        {
            Color = "Black";
            this.IsEnabled = true;
            this.IsRunning = false;
            apiService = new ApiService();
            Orden = model;
            if(Orden.Estado == "Anulado")
                VisibleBotones = false;
            else
                VisibleBotones = true;
            switch (model.EstadoReferido)
            {
                case null:
                    Color = "Black";
                    Estado = "Pendiente";
                    break;
                case "A":
                    Color = "Black";
                    Estado = "Pendiente";
                    break;
                case "P":
                    Color = "Green";
                    Estado = "Aprobada";
                    break;
                case "X":
                    Color = "Red";
                    Estado = "Anulada";                    
                    break;
                default:
                    break;
            }

            ListaDetalle = new ObservableCollection<OrdenNominaDetalleModel>(Orden.LstDet);
            Height = (Orden.LstDet.Count * 80); 
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
            if (string.IsNullOrEmpty(this.Orden.ComentarioReferido))
                Orden.ComentarioReferido = string.Empty;
            
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
            Orden.LstDet = new List<OrdenNominaDetalleModel>(ListaDetalle);
            Orden.LstDet.ForEach(q => q.EstadoString = q.Estado == true ? "P" : null);
            this.Orden.EstadoReferido = "P";

            var response_sinc = await apiService.Post<OrdenNominaModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "OrdenNomina",
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

            this.IsEnabled = true;
            this.IsRunning = false;

            MainViewModel.GetInstance().ReferidosOrdenesNomina = new ReferidosOrdenesNominaViewModel();
            Application.Current.MainPage = new ReferidosMasterPage();

        }

        public ICommand ReprobarCommand
        {
            get
            {
                return new RelayCommand(Reprobar);
            }
        }

        private async void Reprobar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(this.Orden.ComentarioReferido))
                Orden.ComentarioReferido = string.Empty;

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
            Orden.LstDet = new List<OrdenNominaDetalleModel>(ListaDetalle);
            Orden.LstDet.ForEach(q => q.EstadoString = q.Estado == true ? "P" : null);
            this.Orden.EstadoReferido = "X";

            var response_sinc = await apiService.Post<OrdenNominaModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "OrdenNomina",
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

            this.IsEnabled = true;
            this.IsRunning = false;

            MainViewModel.GetInstance().ReferidosOrdenesNomina = new ReferidosOrdenesNominaViewModel();
            Application.Current.MainPage = new ReferidosMasterPage();

        }
        public ICommand PasarCommand
        {
            get
            {
                return new RelayCommand(Pasar);
            }
        }

        private async void Pasar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(this.Orden.ComentarioReferido))
                Orden.ComentarioReferido = string.Empty;

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
            Orden.LstDet = new List<OrdenNominaDetalleModel>(ListaDetalle);
            Orden.LstDet.ForEach(q => q.EstadoString = q.Estado == true ? "P" : null);
            this.Orden.EstadoReferido = "A";

            var response_sinc = await apiService.Post<OrdenNominaModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "OrdenNomina",
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

            this.IsEnabled = true;
            this.IsRunning = false;

            MainViewModel.GetInstance().ReferidosOrdenesNomina = new ReferidosOrdenesNominaViewModel();
            Application.Current.MainPage = new ReferidosMasterPage();

        }
        #endregion
    }
}
