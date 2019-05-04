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
    public class CreacionBitacoraViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsRunning;
        private bool _IsEnabled;
        private BitacoraModel _Orden;
        private ApiService apiService;
        private DateTime _FechaArribo;
        private DateTime _FechaZarpe;
        private string _NombreSucursal;
        private string _NomViaje;
        private string _Titulo;
        #endregion

        #region Propiedades
        public BitacoraModel Orden {
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
        public DateTime FechaArribo
        {
            get { return this._FechaArribo; }
            set { SetValue(ref this._FechaArribo, value); }
        }
        public DateTime FechaZarpe
        {
            get { return this._FechaZarpe; }
            set { SetValue(ref this._FechaZarpe, value); }
        }
        public string NombreSucursal
        {
            get { return this._NombreSucursal; }
            set { SetValue(ref this._NombreSucursal, value); }
        }
        public string NomViaje
        {
            get { return this._NomViaje; }
            set { SetValue(ref this._NomViaje, value); }
        }
        public string Titulo
        {
            get { return this._Titulo; }
            set { SetValue(ref this._Titulo, value); }
        }
        #endregion

        #region Constructor
        public CreacionBitacoraViewModel(BitacoraModel Model)
        {
            Orden = Model;
            Titulo = Orden.Titulo;
            FechaArribo = Orden.FechaArribo ?? DateTime.Now.Date;
            FechaZarpe = Orden.FechaZarpe ?? DateTime.Now.Date;

            NombreSucursal = Orden.NomBarco;
            NomViaje = Orden.NomViaje;

            IsEnabled = true;
            apiService = new ApiService();
            
        }
        #endregion

        #region Metodos
        public void SetCombo(string Codigo, string Nombre, Enumeradores.eCombo Combo)
        {
            switch (Combo)
            {
                case Enumeradores.eCombo.BARCO:
                    Orden.Barco = Codigo;
                    Orden.NomBarco = Nombre;
                    NombreSucursal = Nombre;
                    break;
                case Enumeradores.eCombo.VIAJE:
                    Orden.Viaje = Codigo;
                    Orden.NomViaje = Nombre;
                    NomViaje = Nombre;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Comandos

        public ICommand BuscarSucursalCommand
        {
            get { return new RelayCommand(BuscarSucursal); }
        }

        private async void BuscarSucursal()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.BARCO, "B");
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }

        public ICommand BuscarViajeCommand
        {
            get { return new RelayCommand(BuscarViaje); }
        }

        private async void BuscarViaje()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.VIAJE, "B");
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }
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
               
                if (string.IsNullOrEmpty(this.NombreSucursal))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Seleccione el barco",
                        "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(this.NomViaje))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Seleccione el viaje",
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

                this.Orden.Usuario = Settings.IdUsuario;
                this.Orden.FechaArribo = FechaArribo;
                this.Orden.FechaZarpe = FechaZarpe;
                this.Orden.FechaZarpeReal = null;
                var response_sinc = await apiService.Post<BitacoraModel>(
                    Settings.UrlConexionActual,
                    Settings.RutaCarpeta,
                    "CreacionBitacora",
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
                        Orden.Id > 0 ? "Se ha modificado la bitácora exitósamente" : "Se ha creado la bitácora exitósamente" ,
                        "Aceptar");

                MainViewModel.GetInstance().CreacionBitacoras.LoadLista();
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
