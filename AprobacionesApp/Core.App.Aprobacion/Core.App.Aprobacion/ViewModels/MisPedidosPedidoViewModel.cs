using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class MisPedidosPedidoViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsRunning;
        private bool _IsEnabled;
        private PedidoModel _Orden;
        private ApiService apiService;
        private DateTime _Fecha;
        private string _NombreBodega;
        private string _NombreSucursal;
        private string _NomViaje;
        private string _NombreSolicitante;
        private NumberFormatInfo provider;
        private string _Comentario;
        private string _Titulo;
        private ObservableCollection<string> _ListaTipoOrden;
        private string _TipoSelectedIndex;
        #endregion

        #region Propiedades
        public string TipoSelectedIndex
        {
            get { return this._TipoSelectedIndex; }
            set { SetValue(ref this._TipoSelectedIndex, value); }
        }
        public ObservableCollection<string> ListaTipoOrden
        {
            get { return this._ListaTipoOrden; }
            set
            {
                SetValue(ref this._ListaTipoOrden, value);                
            }
        }
        public PedidoModel Orden {
            get { return this._Orden; }
            set { SetValue(ref this._Orden, value); }
        }
        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set { SetValue(ref this._IsEnabled, value); }
        }
        public bool IsRunning
        {
            get { return this._IsRunning; }
            set { SetValue(ref this._IsRunning, value); }
        }
        public DateTime Fecha
        {
            get { return this._Fecha; }
            set { SetValue(ref this._Fecha, value); }
        }
        public string NombreBodega
        {
            get { return this._NombreBodega; }
            set { SetValue(ref this._NombreBodega, value); }
        }
        public string NombreSucursal
        {
            get { return this._NombreSucursal; }
            set { SetValue(ref this._NombreSucursal, value); }
        }
        public string NomViaje
        {
            get { return this._NomViaje; }
            set { SetValue(ref this._NomViaje, value); }
        }
        public string NombreSolicitante
        {
            get { return this._NombreSolicitante; }
            set { SetValue(ref this._NombreSolicitante, value); }
        }
        public string Comentario
        {
            get { return this._Comentario; }
            set { SetValue(ref this._Comentario, value); }
        }
        public string Titulo
        {
            get { return this._Titulo; }
            set { SetValue(ref this._Titulo, value); }
        }
        #endregion

        #region Constructor
        public MisPedidosPedidoViewModel(PedidoModel Model)
        {
            provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            Orden = Model;
            Titulo = Orden.Titulo;
            Fecha = Orden.Fecha;


            NombreBodega = Orden.NombreBodega;
            NombreSolicitante = Orden.NombreEmpleado;
            NombreSucursal = Orden.NombreSucursal;
            NomViaje = Orden.NombreViaje;
            Comentario = Orden.Observacion;

            IsEnabled = true;
            apiService = new ApiService();
            
        }
        #endregion

        #region Metodos
        public void SetCombo(string Codigo, string Nombre, Enumeradores.eCombo Combo)
        {
            switch (Combo)
            {
                case Enumeradores.eCombo.BARCO:
                    Orden.IdSucursal = Codigo;
                    Orden.NombreSucursal = Nombre;
                    NombreSucursal = Nombre;
                    break;
                case Enumeradores.eCombo.VIAJE:
                    Orden.IdViaje = Codigo;
                    Orden.NombreViaje = Nombre;
                    NomViaje = Nombre;
                    break;
                case Enumeradores.eCombo.SOLICITANTE:
                    Orden.IdSolicitante = Codigo;
                    Orden.NombreEmpleado = Nombre;
                    NombreSolicitante = Nombre;
                    break;
                case Enumeradores.eCombo.BODEGA:
                    Orden.IdBodega = Codigo;
                    Orden.NombreBodega = Nombre;
                    NombreBodega = Nombre;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Comandos

        public ICommand BuscarSolicitanteCommand
        {
            get { return new RelayCommand(BuscarSolicitante); }
        }

        private async void BuscarSolicitante()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.SOLICITANTE,"P");
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }

        public ICommand BuscarSucursalCommand
        {
            get { return new RelayCommand(BuscarSucursal); }
        }

        private async void BuscarSucursal()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.BARCO, "P");
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }

        public ICommand BuscarViajeCommand
        {
            get { return new RelayCommand(BuscarViaje); }
        }

        private async void BuscarViaje()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.VIAJE, "P");
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }
        public ICommand BuscarBodegaCommand
        {
            get { return new RelayCommand(BuscarBodega); }
        }

        private async void BuscarBodega()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.BODEGA, "P");
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }

        public ICommand GuardarCommand
        {
            get { return new RelayCommand(Guardar); }
        }

        private async void Guardar()
        {
            try
            {
                this.IsEnabled = false;
                this.IsRunning = true;
                if (string.IsNullOrEmpty(this.NombreBodega))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Seleccione la bodega",
                        "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(this.NombreSucursal))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Seleccione el barco",
                        "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(this.NomViaje))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Seleccione el viaje",
                        "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(this.NombreSolicitante))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Seleccione el solicitante",
                        "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(this.Comentario))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Ingrese el comentario",
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

                this.Orden.Usuario = Settings.IdUsuario;
                this.Orden.Observacion = Comentario;
                this.Orden.Fecha = Fecha;
                this.Orden.LstDet = new System.Collections.Generic.List<PedidoDetModel>();

                var response_sinc = await apiService.Post<PedidoModel>(
                    Settings.UrlConexionActual,
                    Settings.RutaCarpeta,
                    "Pedidos",
                    this.Orden);

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

                #region Actualizo filtros
                Settings.Solicitante = this.Orden.IdSolicitante;
                Settings.Bodega = this.Orden.IdBodega;
                Settings.Viaje = this.Orden.IdViaje;
                Settings.Sucursal = this.Orden.IdSucursal;

                Settings.NombreViaje = this.Orden.NombreViaje;
                Settings.NombreSucursal = this.Orden.NombreSucursal;
                Settings.NombreSolicitante = this.Orden.NombreEmpleado;
                Settings.NombreBodega = this.Orden.NombreBodega;

                FiltroModel filtro = new FiltroModel
                {
                    Usuario = Settings.IdUsuario,
                    Sucursal = Settings.Sucursal,
                    Viaje = Settings.Viaje,
                    Bodega = Settings.Bodega,
                    Solicitante = Settings.Solicitante
                };
                var response_fil = await apiService.Post<FiltroModel>(
                Settings.UrlConexionActual,
                Settings.RutaCarpeta,
                "Filtro",
                filtro);
                #endregion

                await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        Orden.ID > 0 ? "Se ha modificado el pedido exitósamente" : "Se ha creado el pedido exitósamente" ,
                        "Aceptar");

                MainViewModel.GetInstance().MisPedidos.LoadLista();
                await App.Navigator.Navigation.PopAsync();
            }
            catch (System.Exception ex)
            {
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
