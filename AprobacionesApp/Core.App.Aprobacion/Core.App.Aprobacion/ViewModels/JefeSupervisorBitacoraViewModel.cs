using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class JefeSupervisorBitacoraViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsRunning;
        private bool _IsEnabled;
        private ApiService apiService;
        private BitacoraModel _bitacora;
        private int _Height;
        #endregion

        #region Propiedades
        public BitacoraModel Bitacora
        {
            get { return this._bitacora; }
            set { SetValue(ref this._bitacora, value); }
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
        #endregion

        #region Constructor
        public JefeSupervisorBitacoraViewModel(BitacoraModel Model)
        {
            apiService = new ApiService();
            Bitacora = new BitacoraModel();
            Bitacora = Model;
            CargarBitacora();
        }

        #region Metodos
        private async void CargarBitacora()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            try
            {
                Height = 0;
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

                var response_cs = await apiService.GetObject<BitacoraDetModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "BitacorasDetJefeSup", "ID="+Bitacora.Id+"&LINEA="+Bitacora.Linea);
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

                Bitacora.lst = (List<BitacoraDetModel>)response_cs.Result;
                if (Bitacora != null && Bitacora.NumeroOrden != 0)
                {                  

                    Height = Bitacora.lst == null ? 0 : Bitacora.lst.Count * 40;
                }
                IsEnabled = true;
                IsRunning = false;
            }
            catch (System.Exception ex)
            {
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
        #endregion
    }
}
