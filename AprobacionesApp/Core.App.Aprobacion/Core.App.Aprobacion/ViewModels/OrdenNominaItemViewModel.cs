using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class OrdenNominaItemViewModel : OrdenNominaModel
    {
        #region Comandos
        public ICommand SelectOrdenCommand
        {
            get { return new RelayCommand(SelectOrden); }
        }

        private async void SelectOrden()
        {
            await CargarOrden();

            App.MasterReferidos.IsPresented = false;
            MainViewModel.GetInstance().ReferidosOrdenNomina = new ReferidosOrdenNominaViewModel(this);
            await App.Navigator.Navigation.PushAsync(new ReferidosOrdenNominaPage());
        }
        #endregion

        #region Metodos
        private async Task CargarOrden()
        {
            try
            {
                ApiService apiService = new ApiService();

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

                var response_cs = await apiService.GetList<OrdenNominaDetalleModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "OrdenNominaDet", "DINV_CTINV=" + this.NumeroOrden);
                if (!response_cs.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }

                this.LstDet = (List<OrdenNominaDetalleModel>)response_cs.Result;
                if (this != null && this.NumeroOrden != 0 && this.LstDet.Count != 0)
                {
                    string TipoDocumento = this.TipoDocumento;

                    Color = this.Estado == "A" ? "Black" : (this.Estado == "P" ? "Green" : (this.Estado == "X" ? "Red" : "Black"));
                    this.Estado = this.Estado == "A" ? "Pendiente" : (this.Estado == "P" ? "Aprobada" : (this.Estado == "X" ? "Anulada" : "Pendiente"));

                    this.Titulo = TipoDocumento + " No. " + this.NumeroOrden;

                    this.LstDet.ForEach(q => q.Estado = q.EstadoString == "P" ? true : false);
                }
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
