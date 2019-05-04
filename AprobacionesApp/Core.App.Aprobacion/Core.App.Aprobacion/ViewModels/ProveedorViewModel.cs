using System;
using System.Windows.Input;
using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class ProveedorViewModel : BaseViewModel
    {
        #region Variables
        private ProveedorModel _Proveedor;
        private decimal _Duracion;
        ApiService apiService;
        private bool _IsEnabled;
        #endregion

        #region Propiedades
        public ProveedorModel Proveedor
        {
            get { return this._Proveedor; }
            set
            {
                SetValue(ref this._Proveedor, value);
            }
        }

        public decimal Duracion
        {
            get { return this._Duracion; }
            set
            {
                SetValue(ref this._Duracion, value);
            }
        }

        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set
            {
                SetValue(ref this._IsEnabled, value);
            }
        }
        #endregion

        #region Constructor
        public ProveedorViewModel(ProveedorModel model)
        {
            apiService = new ApiService();
            GetProveedor(model);
            Duracion = model.Duracion == null ? 0 : Convert.ToDecimal(model.Duracion);
            IsEnabled = true;

        }
        #endregion


        #region Metodos
        private async void GetProveedor(ProveedorModel model)
        {
            IsEnabled = false;
            try
            {
                Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
                if (!con.IsSuccess)
                {
                    string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                    con = await apiService.CheckConnection(UrlDistinto);
                    if (!con.IsSuccess)
                    {
                        this.IsEnabled = true;
                        await Application.Current.MainPage.DisplayAlert(
                            "Alerta",
                            con.Message,
                            "Aceptar");
                        return;
                    }
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }

                var response_pro = await apiService.GetObject<ProveedorModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Proveedor", "CODIGO=" + model.Codigo);
                if (!response_pro.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_pro.Message,
                        "Aceptar");
                    return;
                }
                var prov = (ProveedorModel)response_pro.Result;
                if (prov != null)
                    model.DuracionAcumulada = prov.DuracionAcumulada;

                Proveedor = model;
                IsEnabled = true;
            }
            catch (Exception ex)
            {
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                           "Alerta",
                           ex.Message,
                           "Aceptar");
                return;
            }
        }
        #endregion

        #region Comandos
        public ICommand GuardarCommand
        {
            get { return new RelayCommand(Guardar); }
        }

        private async void Guardar()
        {
            IsEnabled = false;
            try
            {
                Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
                if (!con.IsSuccess)
                {
                    string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                    con = await apiService.CheckConnection(UrlDistinto);
                    if (!con.IsSuccess)
                    {
                        this.IsEnabled = true;
                        await Application.Current.MainPage.DisplayAlert(
                            "Alerta",
                            con.Message,
                            "Aceptar");
                        return;
                    }
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }
                Proveedor.Duracion = Duracion;
                var response_sinc = await apiService.Post<ProveedorModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "Proveedor",
                this.Proveedor);
                if (!response_sinc.IsSuccess)
                {
                    this.IsEnabled = true;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_sinc.Message,
                        "Aceptar");
                    return;
                }

                MainViewModel.GetInstance().ModificarProveedor(Proveedor.Codigo, Duracion);

                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Actualización exitosa",
                    "Aceptar");

                await App.Navigator.Navigation.PopAsync();

                IsEnabled = true;
            }
            catch (Exception ex)
            {
                IsEnabled = true;
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
