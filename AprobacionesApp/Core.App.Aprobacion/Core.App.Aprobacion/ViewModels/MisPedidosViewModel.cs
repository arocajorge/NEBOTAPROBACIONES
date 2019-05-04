using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class MisPedidosViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<PedidoItemViewModel> _lstOrdenes;
        private List<PedidoModel> _lstOrden;
        private string _filter;
        private ApiService apiService;
        private bool _IsRefreshing;
        #endregion

        #region Propiedades
        public bool IsRefreshing
        {
            get { return this._IsRefreshing; }
            set
            {
                SetValue(ref this._IsRefreshing, value);
            }
        }
        public ObservableCollection<PedidoItemViewModel> LstOrdenes
        {
            get { return this._lstOrdenes; }
            set
            {
                SetValue(ref this._lstOrdenes, value);
            }
        }
        public string filter
        {
            get { return this._filter; }
            set
            {
                SetValue(ref this._filter, value);
                Buscar();
            }
        }
        #endregion

        #region Constructor
        public MisPedidosViewModel()
        {
            apiService = new ApiService();
            LoadLista();
        }
        #endregion

        #region Metodos
        private IEnumerable<PedidoItemViewModel> ToOrdenItemModel()
        {
            var temp = _lstOrden.Select(l => new PedidoItemViewModel
            {
                ID = l.ID,
                Fecha = l.Fecha,
                IdBodega = l.IdBodega,
                IdViaje = l.IdViaje,
                Estado = l.Estado,
                IdSolicitante = l.IdSolicitante,
                NombreEmpleado = l.NombreEmpleado,
                Titulo = l.Titulo,
                IdSucursal = l.IdSucursal,
                NombreBodega = l.NombreBodega,
                NombreViaje = l.NombreViaje,
                NombreSucursal = l.NombreSucursal,
                Observacion = l.Observacion,
                Usuario = l.Usuario,
                Color = l.Color,
                EstadoAprobacion = l.EstadoAprobacion,
                ComentarioAprobacion = l.ComentarioAprobacion
            });
            return temp;
        }

        public async void LoadLista()
        {
            IsRefreshing = true;
            try
            {
                this.LstOrdenes = new ObservableCollection<PedidoItemViewModel>();
                if (string.IsNullOrEmpty(Settings.UrlConexionActual))
                {
                    this.IsRefreshing = false;
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
                        this.IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert(
                            "Alerta",
                            con.Message,
                            "Aceptar");
                        return;
                    }
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }
                string parameters = "USUARIO=" + Settings.IdUsuario;
                if(!string.IsNullOrEmpty( Settings.Sucursal))
                    parameters += "&BARCO=" + Settings.Sucursal;
                if(!string.IsNullOrEmpty(Settings.Viaje))
                    parameters += "&VIAJE=" + Settings.Viaje;

                var response_cs = await apiService.GetList<PedidoModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Pedidos", parameters);
                if (!response_cs.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }
                _lstOrden = (List<PedidoModel>)response_cs.Result;
                this.LstOrdenes = new ObservableCollection<PedidoItemViewModel>(ToOrdenItemModel());
                IsRefreshing = false;
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                           "Alerta",
                           ex.Message,
                           "Aceptar");
                return;
            }
        }
        #endregion

        #region Comandos
        public ICommand BuscarCommand
        {
            get { return new RelayCommand(Buscar); }
        }
        public ICommand RefreshCommand
        {
            get { return new RelayCommand(LoadLista); }
        }

        private async void Buscar()
        {
            IsRefreshing = true;
            try
            {
                if (string.IsNullOrEmpty(filter))
                    this.LstOrdenes = new ObservableCollection<PedidoItemViewModel>(ToOrdenItemModel());
                else
                    this.LstOrdenes = new ObservableCollection<PedidoItemViewModel>(
                        ToOrdenItemModel().Where(q => q.ID.ToString().Contains(filter.ToLower())
                        || q.Fecha.ToShortDateString().Contains(filter.ToLower())
                        || q.Observacion.ToLower().Contains(filter.ToLower())
                        ).OrderBy(q => q.Fecha));
                IsRefreshing = false;
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
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
