using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class BitacoraItemViewModel : BitacoraModel
    {
        #region Comandos
        public ICommand SelectBitacoraCommand
        {
            get { return new RelayCommand(SelectBitacora); }
        }

        private void SelectBitacora()
        {
            App.MasterJefeSupervisor.IsPresented = false;
            MainViewModel.GetInstance().JefeSupervisorBitacora = new JefeSupervisorBitacoraViewModel(this);
            App.Navigator.Navigation.PushAsync(new JefeSupervisorBitacoraPage());
        }

        public ICommand SelectCumplimientoCommand
        {
            get { return new RelayCommand(SelectCumplimiento); }
        }

        private void SelectCumplimiento()
        {
            App.MasterCumplimiento.IsPresented = false;
            MainViewModel.GetInstance().CumplimientoLineas = new CumplimientoLineasViewModel(this);
            App.Navigator.Navigation.PushAsync(new CumplimientoLineasPage());
        }

        public ICommand AprobarCommand
        {
            get { return new RelayCommand(Aprobar); }
        }

        private async void Aprobar()
        {
            ApiService apiService = new ApiService();
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

            this.Usuario = Settings.IdUsuario;
            this.Estado = "P";

            var response_sinc = await apiService.Post<BitacoraModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "BitacorasJefeSup",
                this);

            if (!response_sinc.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    response_sinc.Message,
                    "Aceptar");
                return;
            }else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Actualización de estado exitosa",
                    "Aceptar");

                MainViewModel.GetInstance().CumplimientoLineas.LoadLista();
                MainViewModel.GetInstance().CumplimientoBitacoras.LoadLista();
            }            
        }
        #endregion
    }
}
