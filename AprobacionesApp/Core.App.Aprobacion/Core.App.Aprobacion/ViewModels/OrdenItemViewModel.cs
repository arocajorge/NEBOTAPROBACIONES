using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class OrdenItemViewModel : OrdenModel
    {
        #region Comandos
        public ICommand SelectOrdenCommand
        {
            get { return new RelayCommand(SelectOrden); }
        }

        private void SelectOrden()
        {
            App.MasterJefeSupervisor.IsPresented = false;
            MainViewModel.GetInstance().JefeSupervisorOrden = new JefeSupervisorOrdenViewModel(this);
            App.Navigator.Navigation.PushAsync(new JefeSupervisorOrdenPage());
        }

        public ICommand UpdateOrdenCommand
        {
            get { return new RelayCommand(UpdateOrden); }
        }

        private async void UpdateOrden()
        {
            try
            {
                App.MasterJefeSupervisor.IsPresented = false;
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
                string parameters = "CINV_SEC=" + this.Secuencia;
                var response_cs = await apiService.GetObject<OrdenModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "ModificacionOrdenTrabajo", parameters);
                if (!response_cs.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }
                var Orden = (OrdenModel)response_cs.Result;
                Orden.Titulo = (Orden.TipoDocumento == "OT" ? "Orden de trabajo # " : "Orden de compra #")+Orden.NumeroOrden;
                
                MainViewModel.GetInstance().MisOrdenesTrabajoOrden = new MisOrdenesTrabajoOrdenViewModel(Orden);
                await App.Navigator.Navigation.PushAsync(new MisOrdenesTrabajoOrdenPage());

            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");
            }
        }
        #endregion
    }
}
