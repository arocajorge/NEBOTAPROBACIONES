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
        private ObservableCollection<BitacoraItemViewModel> _lstBitacoras;
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
            get { return this._lstBitacoras; }
            set
            {
                SetValue(ref this._lstBitacoras, value);
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
                Fecingreso = l.Fecingreso,
                Barco = l.Barco,
                NomBarco = l.NomBarco,
                Login = l.Login,
                Estado = l.Estado,
                Linea = l.Linea,
                Descripcion = l.Descripcion,
                Contratista = l.Contratista,
                LineaDetalle = l.LineaDetalle,
                NumeroOrden = l.NumeroOrden,
                Valor = l.Valor,
                EstadoJefe = l.EstadoJefe,
                EstadoSupervisor = l.EstadoSupervisor,
                Storden = l.Storden
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
                parameters += "&BODEGA=" + Settings.Bodega;
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
                        ToBitacoraItemModel().Where(q => q.Contratista.ToLower().Contains(filter.ToLower()) || q.Descripcion.ToLower().Contains(filter.ToLower())
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
