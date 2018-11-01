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
    public class JefeSupervisorBitacorasViewModel : BaseViewModel
    {

        #region Variables
        private ObservableCollection<BitacoraItemViewModel> _lstDet;
        private List<BitacoraModel> _lstBitacora;
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
        public JefeSupervisorBitacorasViewModel()
        {
            apiService = new ApiService();
            LoadLista();
        }
        #endregion

        #region Metodos
        private IEnumerable<BitacoraItemViewModel> ToBitacoraItemModel()
        {
            var temp = _lstBitacora.Select(l => new BitacoraItemViewModel
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
                Estado = l.Estado
            });
            return temp;
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
                parameters += "&BARCO=" + Settings.Sucursal;
                parameters += "&VIAJE=" + Settings.Viaje;

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
                if (Settings.RolApro == "J")
                {
                    _lstBitacora.ForEach(q => { q.Imagen = q.CantidadLineas == 0 ? ("ic_keyboard_arrow_right") : (q.PendienteJefe == 0 ? "ic_assignment_turned_in" : "ic_access_time");
                        q.Color = q.EstadoJefe == "P" ? "Green" : (q.EstadoJefe == "X" ? "Red" : "Black");
                        q.Estado = q.EstadoJefe == "P" ? "Cumplida" : (q.EstadoJefe == "X" ? "Incumplida" : "Pendiente");
                    });
                }else
                    _lstBitacora.ForEach(q => { q.Imagen = q.CantidadLineas == 0 ? ("ic_keyboard_arrow_right") : (q.PendienteSupervisor == 0 ? "ic_assignment_turned_in" : "ic_access_time");
                        q.Color = q.EstadoSupervisor == "P" ? "Green" : (q.EstadoSupervisor == "X" ? "Red" : "Black");
                        q.Estado = q.EstadoSupervisor == "P" ? "Cumplida" : (q.EstadoSupervisor == "X" ? "Incumplida" : "Pendiente");
                    });

                _lstBitacora = _lstBitacora.OrderBy(q => q.Linea).ToList();
                this.LstBitacoras = new ObservableCollection<BitacoraItemViewModel>(ToBitacoraItemModel());
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
                    this.LstBitacoras = new ObservableCollection<BitacoraItemViewModel>(ToBitacoraItemModel());
                else
                    this.LstBitacoras = new ObservableCollection<BitacoraItemViewModel>(
                        ToBitacoraItemModel().Where(q => q.Contratista.ToLower().Contains(filter.ToLower()) || q.Linea.ToString().Contains(filter.ToLower()) || q.Descripcion.ToLower().Contains(filter.ToLower())
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
    }
}
