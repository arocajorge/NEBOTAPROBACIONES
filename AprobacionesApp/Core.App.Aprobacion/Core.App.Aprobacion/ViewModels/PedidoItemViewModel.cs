using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class PedidoItemViewModel : PedidoModel
    {
        #region Comandos

        public ICommand UpdatePedidoCommand
        {
            get { return new RelayCommand(UpdatePedido); }
        }

        private async void UpdatePedido()
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
                string parameters = "PED_ID=" + this.ID;
                var response_cs = await apiService.GetList<PedidoModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Pedidos", parameters);
                if (!response_cs.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }
                var LPedido = (List<PedidoModel>)response_cs.Result;
                var Pedido = LPedido.FirstOrDefault();
                Pedido.Titulo =  "Pedido de compra # " + Pedido.ID.ToString();

                MainViewModel.GetInstance().MisPedidosPedido = new MisPedidosPedidoViewModel(Pedido);
                await App.Navigator.Navigation.PushAsync(new MisPedidosPedidoPage());

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
