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
    public class CumplimientoLineasViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<BitacoraItemViewModel> _lstDet;
        private List<BitacoraModel> _lstBitacora;
        private string _filter;
        private ApiService apiService;
        private bool _IsRefreshing;
        private BitacoraModel Bitacora;
        private ObservableCollection<string> _ListaColores;
        private string _ColorSelectedIndex;
        #endregion

        #region Propiedades
        public ObservableCollection<string> ListaColores
        {
            get { return this._ListaColores; }
            set { SetValue(ref this._ListaColores, value); }
        }
        public string ColorSelectedIndex
        {
            get { return this._ColorSelectedIndex; }

            set
            {
                SetValue(ref this._ColorSelectedIndex, value);
                Buscar();
            }
        }
        public bool IsRefreshing
        {
            get { return this._IsRefreshing; }
            set
            {
                SetValue(ref this._IsRefreshing, value);
            }
        }
        public ObservableCollection<BitacoraItemViewModel> LstBitacoras
        {
            get { return this._lstDet; }
            set
            {
                SetValue(ref this._lstDet, value);
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
        public CumplimientoLineasViewModel(BitacoraModel model)
        {
            Bitacora = model;
            apiService = new ApiService();
            LoadLista();
            CargarColores();
        }
        #endregion

        #region Metodos
        private IEnumerable<BitacoraItemViewModel> ToBitacoraItemModel()
        {
            if(_lstBitacora != null)
            return _lstBitacora.Select(l => new BitacoraItemViewModel
            {
                Id = l.Id,
                Viaje = l.Viaje,
                NomViaje = l.NomViaje,
                Barco = l.Barco,
                NomBarco = l.NomBarco,
                Linea = l.Linea,
                Descripcion = l.Descripcion,
                Contratista = l.Contratista,
                CantidadLineas = l.CantidadLineas,
                Imagen = l.Imagen,
                Color = l.Color,
                Estado = l.Estado,
                EstadoJefe = l.EstadoJefe,
                EstadoSupervisor = l.EstadoSupervisor,
                ImagenJefe = l.ImagenJefe,
                ImagenSupervisor = l.ImagenSupervisor,
                NumeroOrden = l.NumeroOrden,
                Duracion = l.Duracion == null ? 0 : l.Duracion,
                FechaAproJefe = l.FechaAproJefe,
                FechaAproSupervisor = l.FechaAproSupervisor,
                FechaOT = l.FechaOT,
                Tipo = l.Tipo,

                DuracionReal =  l.FechaOT == null ? 0 :
                (l.FechaAproJefe == null && l.FechaAproSupervisor == null ? (DateTime.Now.Date - Convert.ToDateTime(l.FechaOT).Date).TotalDays : 
                (l.FechaAproJefe != null ? (Convert.ToDateTime(l.FechaAproJefe).Date - Convert.ToDateTime(l.FechaOT).Date).TotalDays :
                (l.FechaAproSupervisor != null ? (Convert.ToDateTime(l.FechaAproSupervisor).Date - Convert.ToDateTime(l.FechaOT).Date).TotalDays :
                (DateTime.Now.Date - Convert.ToDateTime(l.FechaOT).Date).TotalDays))),


                DiferenciaDiasZA = l.FechaAproJefe == null || l.FechaZarpe == null ? 0 : (Convert.ToDateTime(l.FechaAproJefe).Date - Convert.ToDateTime(l.FechaZarpe).Date).TotalDays,
                DiferenciaDiasZRA = l.FechaAproSupervisor == null || l.FechaZarpe == null ? 0 : (Convert.ToDateTime(l.FechaAproSupervisor).Date - Convert.ToDateTime(l.FechaZarpe).Date).TotalDays
            });else
            return new List<BitacoraItemViewModel>();
        }

        public async void LoadLista()
        {
            IsRefreshing = true;
            try
            {
                this.LstBitacoras = new ObservableCollection<BitacoraItemViewModel>();
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
                string parameters = string.Empty;
                parameters += "BARCO=" + Bitacora.Barco;
                parameters += "&VIAJE=" + Bitacora.Viaje;
                parameters += "&MOSTRARTODO=S";
                var response_cs = await apiService.GetList<BitacoraModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "BitacorasJefeSup", parameters);
                if (!response_cs.IsSuccess)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }
                _lstBitacora = (List<BitacoraModel>)response_cs.Result;
                if (_lstBitacora.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(
                           "Alerta",
                           "No existen resultados para los filtros seleccionados ", //+ parameters,
                           "Aceptar");
                }
                _lstBitacora.ForEach(q =>
                {
                    q.ImagenJefe = q.EstadoJefe == "P" ? "ic_check_box" : "ic_check_box_outline_blank";
                    q.ImagenSupervisor = (q.EstadoSupervisor == "P" || q.EstadoSupervisor == "T") ? "ic_check_box" : "ic_check_box_outline_blank";
                    q.Color =
                     q.EstadoSupervisor == "P" ? "LightGreen" :
                     (q.EstadoSupervisor == "T" ? "CornflowerBlue" :
                     (q.EstadoSupervisor == "X" ? "Red" :
                     (q.EstadoSupervisor == "A" || String.IsNullOrEmpty(q.EstadoSupervisor)) && q.EstadoJefe == "P" ? "Yellow" : "White"));
                });
                _lstBitacora = _lstBitacora.OrderBy(q => q.Linea).ToList();
                 Buscar();
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
                string ColorEstado = string.Empty;
                switch (ColorSelectedIndex)
                {
                    case "Todo":
                        ColorEstado = "";
                        break;
                    case "Amarillo / Pendiente":
                        ColorEstado = "Yellow";
                        break;
                    case "Azul / Parcialmente cumplido":
                        ColorEstado = "CornflowerBlue";
                        break;
                    case "Rojo / Anulado":
                        ColorEstado = "Red";
                        break;
                    case "Blanco / No realizado":
                        ColorEstado = "White";
                        break;
                    case "Verde / Cumplido":
                        ColorEstado = "LightGreen";
                        break;
                    default:
                        ColorEstado = "";
                        break;
                }

                string filtro = filter ?? "";
                if (string.IsNullOrEmpty(filtro) && string.IsNullOrEmpty(ColorEstado))
                    this.LstBitacoras = new ObservableCollection<BitacoraItemViewModel>(ToBitacoraItemModel());
                if(string.IsNullOrEmpty(filtro) && !string.IsNullOrEmpty(ColorEstado))
                    this.LstBitacoras = new ObservableCollection<BitacoraItemViewModel>(
                        ToBitacoraItemModel().Where(q => q.Color.Contains(ColorEstado)
                        ).OrderBy(q => q.Linea));
                else
                    this.LstBitacoras = new ObservableCollection<BitacoraItemViewModel>(
                        ToBitacoraItemModel().Where(q => q.Contratista.ToLower().Contains(filtro.ToLower()) || q.Linea.ToString().Contains(filtro.ToLower()) || q.Descripcion.ToLower().Contains(filtro.ToLower())
                        || q.NumeroOrden.Contains(filtro.ToLower())
                        ||  q.Color.Contains(ColorEstado)
                        ).OrderBy(q => q.Linea));
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

        #region Metodos
        private void CargarColores()
        {
            ListaColores = new ObservableCollection<string>();
            ListaColores.Add("Todo");
            ListaColores.Add("Amarillo / Pendiente");
            ListaColores.Add("Blanco / No realizado");
            ListaColores.Add("Verde / Cumplido");
            ListaColores.Add("Rojo / Anulado");
            ListaColores.Add("Azul / Parcialmente cumplido");
            ColorSelectedIndex = "Todo";
        }
        #endregion
    }
}
