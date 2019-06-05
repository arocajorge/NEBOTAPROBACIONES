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
    public class MiBonoDetalleViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<BonoDetModel> _lstDet;
        private ApiService apiService;
        private bool _IsRefreshing;
        private long ID;
        private string Opcion;
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
        public ObservableCollection<BonoDetModel> LstCatalogo
        {
            get { return this._lstDet; }
            set
            {
                SetValue(ref this._lstDet, value);
            }
        }
        #endregion

        #region Constructor
        public MiBonoDetalleViewModel(long _ID, string _Opcion)
        {
            apiService = new ApiService();
            ID = _ID;
            Opcion = _Opcion;
            LoadLista();
        }
        #endregion

        #region Metodos
        public async void LoadLista()
        {
            IsRefreshing = true;
            try
            {
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

                var response_cs = await apiService.GetList<BonoDetModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "BonoDet", "ID=" + ID + "&OPCION=" + Opcion);
                if (!response_cs.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }
                this.LstCatalogo = new ObservableCollection<BonoDetModel>((List<BonoDetModel>)response_cs.Result);
                
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
        public ICommand RefreshCommand
        {
            get { return new RelayCommand(LoadLista); }
        }
        #endregion
    }
}
