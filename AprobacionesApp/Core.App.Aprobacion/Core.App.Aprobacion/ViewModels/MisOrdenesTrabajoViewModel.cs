﻿using Core.App.Aprobacion.Helpers;
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
    public class MisOrdenesTrabajoViewModel : BaseViewModel
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
        public MisOrdenesTrabajoViewModel()
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
                Secuencia = l.Secuencia,
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
                Imagen = l.Imagen,
                NombreBodega = l.NombreBodega,
                IdBodega = l.IdBodega,
                IdProveedor = l.IdProveedor,
                IdSolicitante = l.IdSolicitante,
                IdSucursal = l.IdSucursal,
                IdViaje = l.IdViaje,
                Usuario = l.Usuario  ,
                Duracion = l.Duracion              
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
                string parameters = "CINV_LOGIN=" + Settings.IdUsuario;
                var response_cs = await apiService.GetList<OrdenModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "CreacionOrdenTrabajo", parameters);
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
