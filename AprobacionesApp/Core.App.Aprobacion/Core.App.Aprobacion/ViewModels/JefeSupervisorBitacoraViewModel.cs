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
    public class JefeSupervisorBitacoraViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<BitacoraDetItemViewModel> _lstBitacoras;
        private bool _IsRunning;
        private bool _IsEnabled;
        private ApiService apiService;
        private BitacoraModel _bitacora;
        private int _Height;
        private string _numeroOrden;
        #endregion

        #region Propiedades
        public ObservableCollection<BitacoraDetItemViewModel> LstDet
        {
            get { return this._lstBitacoras; }
            set
            {
                SetValue(ref this._lstBitacoras, value);
            }
        }
        public BitacoraModel Bitacora
        {
            get { return this._bitacora; }
            set { SetValue(ref this._bitacora, value); }
        }
        
        public bool IsRunning
        {
            get { return this._IsRunning; }
            set { SetValue(ref this._IsRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set { SetValue(ref this._IsEnabled, value); }
        }
        public string NumeroOrden
        {
            get { return this._numeroOrden; }
            set { SetValue(ref this._numeroOrden, value); }
        }
        public int Height
        {
            get { return this._Height; }
            set { SetValue(ref this._Height, value); }
        }
        #endregion

        #region Constructor
        public JefeSupervisorBitacoraViewModel(BitacoraModel Model)
        {
            apiService = new ApiService();
            Bitacora = new BitacoraModel();
            Bitacora = Model;
            CargarBitacora();
        }
        #endregion

        #region Metodos
        private IEnumerable<BitacoraDetItemViewModel> ToBitacoraItemModel()
        {
            var temp = Bitacora.lst.Select(l => new BitacoraDetItemViewModel
            {
                Id = l.Id,
                Linea = l.Linea,
                LineaDetalle = l.LineaDetalle,
                NumeroOrden = l.NumeroOrden,
                Valor = l.Valor,
                Nomproveedor = l.Nomproveedor,
                Comentario = l.Comentario,
                CinvNum = l.CinvNum
            });
            return temp;
        }
        private async void CargarBitacora()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            try
            {
                Height = 0;
                LstDet = new ObservableCollection<BitacoraDetItemViewModel>();
                if (string.IsNullOrEmpty(Settings.UrlConexionActual))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
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
                        this.IsEnabled = true;
                        this.IsRunning = false;
                        await Application.Current.MainPage.DisplayAlert(
                            "Alerta",
                            con.Message,
                            "Aceptar");
                        return;
                    }
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }

                var response_cs = await apiService.GetList<BitacoraDetModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "BitacorasDetJefeSup", "ID=" + Bitacora.Id + "&LINEA=" + Bitacora.Linea);
                if (!response_cs.IsSuccess)
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_cs.Message,
                        "Aceptar");
                    return;
                }

                Bitacora.lst = (List<BitacoraDetModel>)response_cs.Result;
                Height = Bitacora.lst.Count * 100;


                LstDet = new ObservableCollection<BitacoraDetItemViewModel>(ToBitacoraItemModel());
                
                IsEnabled = true;
                IsRunning = false;
            }
            catch (System.Exception ex)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");

                return;
            }
        }


        #endregion

        #region Comandos
        public ICommand AgregarCommand
        {
            get
            {
                return new RelayCommand(Agregar);
            }
        }

        public ICommand EliminarCommand
        {
            get
            {
                return new RelayCommand(Eliminar);
            }
        }

        private async void Agregar()
        {
            try
            {
                this.IsEnabled = false;
                this.IsRunning = true;

                if (string.IsNullOrEmpty(this.NumeroOrden))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Ingrese el número de orden",
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
                        this.IsEnabled = true;
                        this.IsRunning = false;
                        await Application.Current.MainPage.DisplayAlert(
                            "Alerta",
                            con.Message,
                            "Aceptar");
                        return;
                    }
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }

                var response_sinc = await apiService.Post<BitacoraDetModel>(
                    Settings.UrlConexionActual,
                    Settings.RutaCarpeta,
                    "BitacorasDetJefeSup",
                    new BitacoraDetModel
                    {
                        Id = Bitacora.Id,
                        Linea = Bitacora.Linea,
                        NumeroOrden = Convert.ToInt32(NumeroOrden),
                    });
                NumeroOrden = string.Empty;
                CargarBitacora();
            }
            catch (Exception ex)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");

                return;
            }
            
        }

        private async void Eliminar()
        {
            try
            {
                this.IsEnabled = false;
                this.IsRunning = true;

                if (string.IsNullOrEmpty(this.NumeroOrden))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Ingrese el número de orden",
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
                        this.IsEnabled = true;
                        this.IsRunning = false;
                        await Application.Current.MainPage.DisplayAlert(
                            "Alerta",
                            con.Message,
                            "Aceptar");
                        return;
                    }
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }

                var response_sinc = await apiService.Post<BitacoraDetModel>(
                    Settings.UrlConexionActual,
                    Settings.RutaCarpeta,
                    "BitacorasDetJefeSup",
                    new BitacoraDetModel
                    {
                        Id = Bitacora.Id,
                        Linea = Bitacora.Linea,
                        NumeroOrden = Convert.ToInt32(NumeroOrden),
                        Eliminar = true
                    });
                NumeroOrden = string.Empty;
                CargarBitacora();
            }
            catch (Exception ex)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");

                return;
            }

        }
        public ICommand AprobarCommand
        {
            get
            {
                return new RelayCommand(Aprobar);
            }
        }

        private async void Aprobar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;

            Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
            if (!con.IsSuccess)
            {
                string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                con = await apiService.CheckConnection(UrlDistinto);
                if (!con.IsSuccess)
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        con.Message,
                        "Aceptar");
                    return;
                }
                else
                    Settings.UrlConexionActual = UrlDistinto;
            }

            this.Bitacora.Usuario = Settings.IdUsuario;
            this.Bitacora.Estado = "P";

            var response_sinc = await apiService.Post<BitacoraModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "BitacorasJefeSup",
                this.Bitacora);

            if (!response_sinc.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    response_sinc.Message,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Actualización de estado exitosa",
                    "Aceptar");

            this.IsEnabled = true;
            this.IsRunning = false;

            MainViewModel.GetInstance().JefeSupervisorBitacoras.LoadLista();
            await App.Navigator.Navigation.PopAsync();
        }
        #endregion
    }
}
