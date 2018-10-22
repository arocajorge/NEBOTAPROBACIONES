using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private IEnumerable<OrdenItemViewModel> ToOrdenItemModel()
        {
            var temp = _lstOrden.Select(l => new OrdenItemViewModel
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
                EstadoSupervisor = l.EstadoSupervisor,
                Imagen = l.Imagen
            });
            return temp;
        }

        public async void LoadLista()
        {
            IsRefreshing = true;
            try
            {
                this.LstOrdenes = new ObservableCollection<OrdenItemViewModel>();
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
                string parameters ="USUARIO=" + Settings.IdUsuario;
                if (!string.IsNullOrEmpty(Settings.Sucursal)) parameters += "&BARCO=" + Settings.Sucursal;
                if (!string.IsNullOrEmpty(Settings.Bodega)) parameters += "&BODEGA=" + Settings.Bodega;
                if (!string.IsNullOrEmpty(Settings.Viaje)) parameters += "&VIAJE=" + Settings.Viaje;
                if (!string.IsNullOrEmpty(Settings.NumeroOrden)) parameters += "&CINV_NUM=" + Settings.NumeroOrden;
                if (!string.IsNullOrEmpty(Settings.EstadoJefe)) parameters += "&ESTADOJEFE=" + Settings.EstadoJefe;
                if (!string.IsNullOrEmpty(Settings.EstadoSupervisor)) parameters += "&ESTADOSUPERVISOR=" + Settings.EstadoSupervisor;
                DateTime FechaIni = Convert.ToDateTime(Settings.FechaInicio);
                DateTime FechaFin = Convert.ToDateTime(Settings.FechaFin);

                parameters += "&DIAINI=" + FechaIni.Day + "&MESINI=" + FechaIni.Month + "&ANIOINI=" + FechaIni.Year;
                parameters += "&DIAFIN=" + FechaFin.Day + "&MESFIN=" + FechaFin.Month + "&ANIOFIN=" + FechaFin.Year;

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
                if (_lstOrden.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(
                           "Alerta",
                           "No existen resultados para los filtros seleccionados ",//+parameters,
                           "Aceptar");
                }
                _lstOrden.ForEach(l => l.Imagen = (Settings.RolApro == "J" ?
                ((l.EstadoJefe == null || l.EstadoJefe.Trim().ToLower() == "pendiente") 
                ? "ic_access_time.png" 
                : (l.EstadoJefe.Trim().ToLower() == "aprobado" ? "ic_assignment_turned_in.png" : "ic_assignment_late.png")) :

                ((l.EstadoSupervisor == null  || l.EstadoSupervisor.Trim().ToLower() == "pendiente") 
                ? "ic_access_time.png" 
                : (l.EstadoSupervisor.Trim().ToLower() == "aprobado" 
                ? "ic_assignment_turned_in.png" : "ic_assignment_late.png"))));

                this.LstOrdenes = new ObservableCollection<OrdenItemViewModel>(ToOrdenItemModel());
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
                    this.LstOrdenes = new ObservableCollection<OrdenItemViewModel>(ToOrdenItemModel());
                else
                    this.LstOrdenes = new ObservableCollection<OrdenItemViewModel>(
                        ToOrdenItemModel().Where(q => q.NumeroOrden.ToString().Contains(filter.ToLower())
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
