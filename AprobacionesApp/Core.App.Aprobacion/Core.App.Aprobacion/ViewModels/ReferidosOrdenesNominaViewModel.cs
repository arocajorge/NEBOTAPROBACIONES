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
    public class ReferidosOrdenesNominaViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<OrdenNominaItemViewModel> _lstOrdenes;
        private List<OrdenNominaModel> _lstOrden;
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
        
        public ObservableCollection<OrdenNominaItemViewModel> LstOrdenes
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
        public ReferidosOrdenesNominaViewModel()
        {
            apiService = new ApiService();
            LoadLista();
        }
        #endregion

        #region Metodos
        private IEnumerable<OrdenNominaItemViewModel> ToOrdenItemModel()
        {
            var temp = _lstOrden.Select(l => new OrdenNominaItemViewModel
            {
                TipoDocumento = l.TipoDocumento,
                NumeroOrden = l.NumeroOrden,
                Fecha = l.Fecha,
                Estado = l.Estado,
                Imagen = l.Imagen,

                EstadoReferido = l.EstadoReferido,
                EstadoAntecedentes = l.EstadoAntecedentes,
                EstadoPerfil = l.EstadoPerfil,
                EstadoPoligrafo = l.EstadoPoligrafo,
                EstadoPsicologo = l.EstadoPsicologo,

                ComentarioAntecedentes = l.ComentarioAntecedentes,
                ComentarioAntecedentes2 = l.ComentarioAntecedentes2,
                ComentarioPerfil = l.ComentarioPerfil,
                ComentarioPoligrafo = l.ComentarioPoligrafo,
                ComentarioPsicologo = l.ComentarioPsicologo,
                ComentarioReferido = l.ComentarioReferido,
                NombreSolicitado = l.NombreSolicitado,
                NombreCentroCosto = l.NombreCentroCosto,
                CedulaSolicitado = l.CedulaSolicitado,
                MotivoAnulacion = l.MotivoAnulacion,
                
                ImagenPsicologo = l.ImagenPsicologo,
                ImagenPoligrafo = l.ImagenPoligrafo,
                ImagenPerfil = l.ImagenPerfil,
                ImagenAntecedentes = l.ImagenAntecedentes,
                Color = l.Color,
                Titulo = l.Titulo,
                NombreCargo = l.NombreCargo
            });
            return temp;
        }

        public async void LoadLista()
        {
            IsRefreshing = true;
            try
            {
                this.LstOrdenes = new ObservableCollection<OrdenNominaItemViewModel>();
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
                string parameters = "ESTADOGERENTE=" + Settings.EstadoGerente;
                if (!string.IsNullOrEmpty(Settings.Sucursal)) parameters += "&BARCO=" + Settings.Sucursal;
                DateTime FechaIni = Convert.ToDateTime(Settings.FechaInicio);
                DateTime FechaFin = DateTime.Now.Date;

                parameters += "&DIAINI=" + FechaIni.Day + "&MESINI=" + FechaIni.Month + "&ANIOINI=" + FechaIni.Year;
                parameters += "&DIAFIN=" + FechaFin.Day + "&MESFIN=" + FechaFin.Month + "&ANIOFIN=" + FechaFin.Year;

                var response_cs = await apiService.GetList<OrdenNominaModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "OrdenNomina", parameters);
                if (!response_cs.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }
                _lstOrden = (List<OrdenNominaModel>)response_cs.Result;
                if (_lstOrden.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(
                           "Alerta",
                           "No existen resultados para los filtros seleccionados ",//+parameters,
                           "Aceptar");
                }
                _lstOrden.ForEach(l =>
                {
                    l.Imagen = (
                        (l.EstadoReferido == null || l.EstadoReferido.Trim().ToUpper() == "A") ? "ic_access_time.png"
                        : (l.EstadoReferido.Trim().ToUpper() == "P" ? "ic_assignment_turned_in.png"
                        : "ic_assignment_late.png"));
                    l.ImagenAntecedentes = (l.EstadoAntecedentes == null || l.EstadoAntecedentes.ToUpper() == "A" ? "ic_check_box_outline_blank"
                        : (l.EstadoAntecedentes.ToUpper() == "P" ? "ic_check_box"
                        : (l.EstadoAntecedentes.ToUpper() == "X" ? "ic_indeterminate_check_box" : "")));
                    l.ImagenPerfil = (l.EstadoPerfil == null || l.EstadoPerfil.ToUpper() == "A" ? "ic_check_box_outline_blank"
                        : (l.EstadoPerfil.ToUpper() == "P" ? "ic_check_box"
                        : (l.EstadoPerfil.ToUpper() == "X" ? "ic_indeterminate_check_box" : "")));
                    l.ImagenPoligrafo = (l.EstadoPoligrafo == null || l.EstadoPoligrafo.ToUpper() == "A" ? "ic_check_box_outline_blank"
                        : (l.EstadoPoligrafo.ToUpper() == "P" ? "ic_check_box"
                        : (l.EstadoPoligrafo.ToUpper() == "X" ? "ic_indeterminate_check_box" : "")));
                    l.ImagenPsicologo = (l.EstadoPsicologo == null || l.EstadoPsicologo.ToUpper() == "A" ? "ic_check_box_outline_blank"
                        : (l.EstadoPsicologo.ToUpper() == "P" ? "ic_check_box"
                        : (l.EstadoPsicologo.ToUpper() == "X" ? "ic_indeterminate_check_box" : "")));
                    l.Estado = l.Estado == "X" ? "Anulado"
                        : (l.Estado == "P" ? "Aprobado"
                        : (l.Estado == "T" ? "Aprobado un viaje"
                        : "Pendiente"));
                    l.Color = l.Estado == "X" ? "Red"
                        : (l.Estado == "P" ? "Green"
                        : (l.Estado == "T" ? "Blue"
                        : "Black"));
                });

                this.LstOrdenes = new ObservableCollection<OrdenNominaItemViewModel>(ToOrdenItemModel());
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
                    this.LstOrdenes = new ObservableCollection<OrdenNominaItemViewModel>(ToOrdenItemModel());
                else
                    this.LstOrdenes = new ObservableCollection<OrdenNominaItemViewModel>(
                        ToOrdenItemModel().Where(q => q.NumeroOrden.ToString().Contains(filter.ToLower())
                        || q.NombreCentroCosto.ToLower().Contains(filter.ToLower())
                        || q.NombreSolicitado.ToLower().Contains(filter.ToLower())
                        || q.Fecha.ToShortDateString().Contains(filter.ToLower())
                        || q.CedulaSolicitado.ToLower().Contains(filter.ToLower())
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
