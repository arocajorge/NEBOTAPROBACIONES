using System;
using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class MiBonoViewModel : BaseViewModel
    {
        #region Variabeles
        private ApiService apiService;
        private BonoModel _bono;
        #endregion

        #region Propiedades
        public BonoModel Bono
        {
            get { return this._bono; }
            set { SetValue(ref this._bono, value); }
        }
        #endregion

        #region Constructor
        public MiBonoViewModel()
        {
            apiService = new ApiService();
            CargarBono();
        }
        #endregion

        #region Metodos
        private async void CargarBono()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.UrlConexionActual))
                {
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
                        await Application.Current.MainPage.DisplayAlert(
                            "Alerta",
                            con.Message,
                            "Aceptar");
                        return;
                    }
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }

                var response_cs = await apiService.GetObject<BonoModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Bono", "BARCO=" + Settings.Sucursal+"&VIAJE="+Settings.Viaje);
                if (!response_cs.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }

                Bono = (BonoModel)response_cs.Result;
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
