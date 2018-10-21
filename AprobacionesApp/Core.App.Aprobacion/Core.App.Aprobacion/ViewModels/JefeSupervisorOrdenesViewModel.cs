using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
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
    public class JefeSupervisorOrdenesViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<OrdenItemViewModel> _lstOrdenes;
        private List<OrdenModel> _lstOrden;
        private string _filter;
        private ApiService apiService;
        private bool _IsRefreshing;
        private int size = 20;
        private int Contador;
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
        public ObservableCollection<OrdenItemViewModel> LstOrdenes
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
        public JefeSupervisorOrdenesViewModel()
        {
            apiService = new ApiService();
            LoadLista();
        }
        #endregion

        #region Metodos
        private IEnumerable<OrdenItemViewModel> ToOrdenItemModel(int page)
        {
            var temp = _lstOrden.Skip((page - 1) * size).Take(size).Select(l => new OrdenItemViewModel
            {
                TipoDocumento = l.TipoDocumento,
                NumeroOrden = l.NumeroOrden,
                NombreProveedor = l.NombreProveedor,
                NombreSolicitante = l.NombreSolicitante,
                ValorOrden = l.ValorOrden,
                Fecha = l.Fecha,
                Estado = l.Estado,
                NomCentroCosto = l.NomCentroCosto,
                NomViaje = l.NomViaje,
                Comentario = l.Comentario,
                EstadoJefe = l.EstadoJefe,
                EstadoSupervisor = l.EstadoSupervisor
            });
            page++;
            return temp;
        }

        public async void LoadLista()
        {
            IsRefreshing = true;
            try
            {
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
                string parameters = "FECHAINI=" + Settings.FechaInicio + "&FECHAFIN=" + Settings.FechaFin + "&USUARIO=" + Settings.IdUsuario;
                if (!string.IsNullOrEmpty(Settings.Bodega)) parameters += "&BODEGA=" + Settings.Bodega;
                if (!string.IsNullOrEmpty(Settings.Viaje)) parameters += "&VIAJE=" + Settings.Viaje;
                if (!string.IsNullOrEmpty(Settings.NumeroOrden)) parameters += "&CINV_NUM=" + Settings.NumeroOrden;
                if (!string.IsNullOrEmpty(Settings.EstadoJefe)) parameters += "&ESTADOJEFE=" + Settings.EstadoJefe;
                if (!string.IsNullOrEmpty(Settings.EstadoSupervisor)) parameters += "&ESTADOSUPERVISOR=" + Settings.EstadoSupervisor;
                var response_cs = await apiService.GetList<OrdenModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "AprobacionJefeSup", parameters);
                if (!response_cs.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }

                _lstOrden = (List<OrdenModel>)response_cs.Result;
                this.LstOrdenes = new ObservableCollection<OrdenItemViewModel>(ToOrdenItemModel(0));
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

        private async void Buscar()
        {
            IsRefreshing = true;
            try
            {
                if (string.IsNullOrEmpty(filter))
                    this.LstOrdenes = new ObservableCollection<OrdenItemViewModel>(ToOrdenItemModel(0));
                else
                    this.LstOrdenes = new ObservableCollection<OrdenItemViewModel>(
                        ToOrdenItemModel(0).Where(q => q.NumeroOrden.ToString().Contains(filter.ToLower())
                        || q.NombreProveedor.ToLower().Contains(filter.ToLower())
                        || q.NombreSolicitante.ToLower().Contains(filter.ToLower())
                        || q.Fecha.ToShortDateString().Contains(filter.ToLower())
                        || q.Comentario.ToLower().Contains(filter.ToLower())
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
