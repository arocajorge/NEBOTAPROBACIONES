using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class CreacionLineaViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsRunning;
        private bool _IsEnabled;
        private BitacoraDetModel _Orden;
        private ApiService apiService;
        private string _Contratista;
        private string _Descripcion;
        private string _Titulo;
        private bool _EsEmpleado;
        #endregion

        #region Propiedades
        public BitacoraDetModel Orden {
            get { return this._Orden; }
            set { SetValue(ref this._Orden, value); }
        }
        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set { SetValue(ref this._IsEnabled, value); }
        }
        public bool IsRunning
        {
            get { return this._IsRunning; }
            set { SetValue(ref this._IsRunning, value); }
        }
        public bool EsEmpleado
        {
            get { return this._EsEmpleado; }
            set { SetValue(ref this._EsEmpleado, value); }
        }
        public string Contratista
        {
            get { return this._Contratista; }
            set { SetValue(ref this._Contratista, value); }
        }
        public string Descripcion
        {
            get { return this._Descripcion; }
            set { SetValue(ref this._Descripcion, value); }
        }
        public string Titulo
        {
            get { return this._Titulo; }
            set { SetValue(ref this._Titulo, value); }
        }
        #endregion

        #region Constructor
        public CreacionLineaViewModel(BitacoraModel Model)
        {
            Orden = new BitacoraDetModel
            {
                Id = Model.Id,
                Linea = Model.Linea
            };

            EsEmpleado = Model.EsEmpleado == 1 ? true : false;
            Contratista = Model.Contratista;
            Descripcion = Model.Descripcion;

            IsEnabled = true;
            apiService = new ApiService();
            
        }
        #endregion

        #region Comandos
         public ICommand GuardarCommand
        {
            get { return new RelayCommand(Guardar); }
        }

        private async void Guardar()
        {
            try
            {
                this.IsEnabled = false;
                this.IsRunning = true;
               
                if (string.IsNullOrEmpty(this.Contratista))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Ingrese el contratista",
                        "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(this.Descripcion))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Ingrese la descripción",
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

                this.Orden.Nomproveedor = Contratista;
                this.Orden.Detalleot = Descripcion;

                var response_sinc = await apiService.Post<BitacoraDetModel>(
                    Settings.UrlConexionActual,
                    Settings.RutaCarpeta,
                    "CreacionLinea",
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
                        Orden.Linea > 0 ? "Se ha modificado la obra exitósamente" : "Se ha creado la obra exitósamente" ,
                        "Aceptar");

                MainViewModel.GetInstance().CreacionLineas.LoadLista();
                await App.Navigator.Navigation.PopAsync();
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");

                return;
            }
        }
        #endregion
    }
}
