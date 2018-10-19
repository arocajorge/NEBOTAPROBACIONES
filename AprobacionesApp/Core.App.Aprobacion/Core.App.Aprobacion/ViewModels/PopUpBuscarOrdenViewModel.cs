using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Models;

namespace Core.App.Aprobacion.ViewModels
{
    public class PopUpBuscarOrdenViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<string> _ListaTipos;
        private string _TipoSelectedIndex;
        private int _NumeroOrden;
        private ApiService apiService;
        private bool _IsEnabled;
        private bool _IsRunning;
        #endregion

        #region Propiedades        
        public int NumeroOrden
        {
            get { return this._NumeroOrden; }
            set { SetValue(ref this._NumeroOrden, value); }
        }
        public ObservableCollection<string> ListaTipos
        {
            get { return this._ListaTipos; }
            set { SetValue(ref this._ListaTipos, value); }
        }
        public string TipoSelectedIndex
        {
            get { return this._TipoSelectedIndex; }
            set { SetValue(ref this._TipoSelectedIndex, value); }
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
        public PopUpBuscarOrdenViewModel()
        {
            this.IsEnabled = true;
            this.IsRunning = false;
            apiService = new ApiService();
            CargarTipos();            
        }
        #endregion

        #region Metodos
        private void CargarTipos()
        {
            ListaTipos = new ObservableCollection<string>();
            ListaTipos.Add("Orden de trabajo");
            ListaTipos.Add("Orden de compra");
            ListaTipos.Add("Orden de caja chica");
            TipoSelectedIndex = "Orden de trabajo";
        }
        #endregion

        #region Comandos
        public ICommand CerrarCommand
        {
            get { return new RelayCommand(Cerrar); }
        }
        public ICommand SearchCommand
        {
            get { return new RelayCommand(Search); }
        }

        private async void Search()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(this.TipoSelectedIndex))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Debe seleccionar el tipo de orden",
                    "Aceptar");
                return;
            }

            if (this.NumeroOrden == 0 || string.IsNullOrEmpty(this.NumeroOrden.ToString()))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Debe ingresar el número de orden",
                    "Aceptar");
                return;
            }

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
            string CINV_TDOC = string.Empty;
            switch (TipoSelectedIndex)
            {
                case "Orden de trabajo":
                    CINV_TDOC = "OT";
                    break;
                case "Orden de compra":
                    CINV_TDOC = "OC";
                    break;
                case "Orden de caja chica":
                    CINV_TDOC = "OK";
                    break;
            }
            var response_cs = await apiService.GetObject<OrdenModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "OrdenTrabajo", "CINV_TDOC="+CINV_TDOC+"&CINV_NUM="+NumeroOrden);
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

                string TipoDocumento = Orden.TipoDocumento;
                switch (TipoDocumento)
                {
                    case "OC":
                        TipoDocumento = "Orden de compra";
                        break;
                    case "OK":
                        TipoDocumento = "Orden de caja chica";
                        break;
                    case "OT":
                        TipoDocumento = "Orden de trabajo";
                        break;
                }
                Orden.Titulo = TipoDocumento + " No. " + Orden.NumeroOrden;

                MainViewModel.GetInstance().AprobacionGerente = new AprobacionGerenteViewModel(Orden);
                Application.Current.MainPage = new NavigationPage(new AprobacionGerentePage());
            }
            else
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "La "+ TipoSelectedIndex+" No."+NumeroOrden+" no se encuentra pendiente de aprobación",
                    "Aceptar");
                return;
            }
        }

        private async void Cerrar()
        {
            await Application.Current.MainPage.Navigation.PopAllPopupAsync();
        }
        #endregion
    }
}
