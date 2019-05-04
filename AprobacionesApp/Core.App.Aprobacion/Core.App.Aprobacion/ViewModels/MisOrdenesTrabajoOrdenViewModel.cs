using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class MisOrdenesTrabajoOrdenViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsRunning;
        private bool _IsEnabled;
        private OrdenModel _Orden;
        private ApiService apiService;
        private string _NombreProveedor;
        private DateTime _Fecha;
        private string _NombreBodega;
        private string _NombreSucursal;
        private string _NomViaje;
        private string _NombreSolicitante;
        private decimal _Valor;
        private decimal _ValorIva;
        private decimal _ValorOrden;
        private bool _TieneIva;
        private NumberFormatInfo provider;
        private string _Comentario;
        private string _Titulo;
        private ObservableCollection<string> _ListaTipoOrden;
        private string _TipoSelectedIndex;
        private decimal _Duracion;
        private decimal _Presupuesto;
        private decimal _Aprobado;
        private decimal _Pendiente;
        private decimal _Saldo;
        #endregion

        #region Propiedades
        public string TipoSelectedIndex
        {
            get { return this._TipoSelectedIndex; }
            set { SetValue(ref this._TipoSelectedIndex, value); }
        }
        public decimal Duracion
        {
            get { return this._Duracion; }
            set { SetValue(ref this._Duracion, value); }
        }
        public decimal Presupuesto
        {
            get { return this._Presupuesto; }
            set { SetValue(ref this._Presupuesto, value); }
        }
        public decimal Aprobado
        {
            get { return this._Aprobado; }
            set { SetValue(ref this._Aprobado, value); }
        }
        public decimal Pendiente
        {
            get { return this._Pendiente; }
            set { SetValue(ref this._Pendiente, value); }
        }
        public decimal Saldo
        {
            get { return this._Saldo; }
            set { SetValue(ref this._Saldo, value); }
        }
        public ObservableCollection<string> ListaTipoOrden
        {
            get { return this._ListaTipoOrden; }
            set
            {
                SetValue(ref this._ListaTipoOrden, value);                
            }
        }
        public OrdenModel Orden {
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
        public string NombreProveedor
        {
            get { return this._NombreProveedor; }
            set { SetValue(ref this._NombreProveedor, value); }
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
        public decimal Valor
        {
            get { return this._Valor; }
            set { SetValue(ref this._Valor, value);
                Calcular();
            }
        }
        public decimal ValorIva
        {
            get { return this._ValorIva; }
            set { SetValue(ref this._ValorIva, value); }
        }
        public decimal ValorOrden
        {
            get { return this._ValorOrden; }
            set { SetValue(ref this._ValorOrden, value); }
        }
        public bool TieneIva
        {
            get { return this._TieneIva; }
            set { SetValue(ref this._TieneIva, value);
                Calcular();
            }
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
        public MisOrdenesTrabajoOrdenViewModel(OrdenModel Model)
        {
            provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            Orden = Model;
            Titulo = Orden.Titulo;
            Fecha = Orden.Fecha;
            Duracion = Orden.Duracion == null ? 0 : Convert.ToDecimal(Orden.Duracion);
            
            Valor =  string.IsNullOrEmpty(Orden.Valor) ? 0 : Convert.ToDecimal(Orden.Valor,provider);
            ValorIva = string.IsNullOrEmpty(Orden.ValorIva) ? 0 : Convert.ToDecimal(Orden.ValorIva, provider);
            TieneIva = ValorIva > 0 ? true : false;
            ValorOrden = Orden.ValorOrden;

            NombreBodega = Orden.NombreBodega;
            NombreProveedor = Orden.NombreProveedor;
            NombreSolicitante = Orden.NombreSolicitante;
            NombreSucursal = Orden.NomCentroCosto;
            NomViaje = Orden.NomViaje;
            Comentario = Orden.Comentario;

            IsEnabled = true;
            apiService = new ApiService();

            CargarTipos();
            GetPresupuesto();
            
        }
        private void CargarTipos()
        {
            ListaTipoOrden = new ObservableCollection<string>();
            ListaTipoOrden.Add("Orden de trabajo");
            ListaTipoOrden.Add("Orden de caja chica");
            if (Orden.NumeroOrden > 0)
                TipoSelectedIndex = Orden.TipoDocumento == "OT" ? "Orden de trabajo" : "Orden de caja chica";
            else
                TipoSelectedIndex = "Orden de trabajo";
        }
        #endregion

        #region Metodos
        public void SetCombo(string Codigo, string Nombre, Enumeradores.eCombo Combo)
        {
            switch (Combo)
            {
                case Enumeradores.eCombo.PROVEEDOR:
                    Orden.IdProveedor = Codigo;
                    Orden.NombreProveedor = Nombre;
                    NombreProveedor = Nombre;
                    break;
                case Enumeradores.eCombo.BARCO:
                    Orden.IdSucursal = Codigo;
                    Orden.NomCentroCosto = Nombre;
                    NombreSucursal = Nombre;
                    GetPresupuesto();
                    break;
                case Enumeradores.eCombo.VIAJE:
                    Orden.IdViaje = Codigo;
                    Orden.NomViaje = Nombre;
                    NomViaje = Nombre;
                    GetPresupuesto();
                    break;
                case Enumeradores.eCombo.SOLICITANTE:
                    Orden.IdSolicitante = Codigo;
                    Orden.NombreSolicitante = Nombre;
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

        private async void Calcular()
        {
            try
            {
                if (TieneIva)
                    ValorIva = Convert.ToDecimal(Math.Round(Convert.ToDouble(Valor) * (0.12),2,MidpointRounding.AwayFromZero));
                else
                    ValorIva = 0;

                ValorOrden = Valor + ValorIva;
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

        #region Comandos
        public ICommand BuscarProveedorCommand
        {
            get { return new RelayCommand(BuscarProveedor); }
        }

        private async void BuscarProveedor()
        {
            MainViewModel.GetInstance().ComboProveedores = new  ComboProveedoresViewModel();
            await App.Navigator.PushAsync(new ComboProveedoresPage());
        }

        public ICommand BuscarSolicitanteCommand
        {
            get { return new RelayCommand(BuscarSolicitante); }
        }

        private async void BuscarSolicitante()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.SOLICITANTE, "O");
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }

        public ICommand BuscarSucursalCommand
        {
            get { return new RelayCommand(BuscarSucursal); }
        }

        private async void BuscarSucursal()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.BARCO, "O");
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }

        public ICommand BuscarViajeCommand
        {
            get { return new RelayCommand(BuscarViaje); }
        }

        private async void BuscarViaje()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.VIAJE, "O");
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }
        public ICommand BuscarBodegaCommand
        {
            get { return new RelayCommand(BuscarBodega); }
        }

        private async void BuscarBodega()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.BODEGA, "O");
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
                if (string.IsNullOrEmpty(this.NombreProveedor))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Seleccione el proveedor",
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
                if (string.IsNullOrEmpty(this.ValorOrden.ToString()))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Ingrese el valor de la orden",
                        "Aceptar");
                    return;
                }

                if (string.IsNullOrEmpty(this.Duracion.ToString()))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Ingrese la duración de la orden",
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

                #region ValidacionProveedor
                var response_pro = await apiService.GetObject<ProveedorModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Proveedor", "CODIGO="+this.Orden.IdProveedor);
                if (!response_pro.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        response_pro.Message,
                        "Aceptar");
                    return;
                }
                var proveedor = (ProveedorModel)response_pro.Result;
                if (proveedor == null || string.IsNullOrEmpty(proveedor.Codigo))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "El proveedor seleccionado no existe.",
                        "Aceptar");
                    return;
                }
                decimal DuracionAcumulada = (proveedor.DuracionAcumulada == null ? 0 : Convert.ToDecimal(proveedor.DuracionAcumulada));
                if (proveedor.Duracion > 0 && proveedor.Duracion < (DuracionAcumulada + Duracion))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Ha superado la duración en días que el proveedor puede ser contratado {"+proveedor.Duracion+"} para órdenes de globales de la compañía.",
                        "Aceptar");
                    return;
                }
                #endregion

                this.Orden.Usuario = Settings.IdUsuario;
                this.Orden.Comentario = Comentario;
                this.Orden.Fecha = Fecha;
                this.Orden.Valor = Valor.ToString();
                this.Orden.ValorIva = ValorIva.ToString();
                this.Orden.TipoDocumento = TipoSelectedIndex == "Orden de trabajo" ? "OT" : "OK";
                this.Orden.Duracion = Duracion;



                var response_sinc = await apiService.Post<OrdenModel>(
                    Settings.UrlConexionActual,
                    Settings.RutaCarpeta,
                    "CreacionOrdenTrabajo",
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

                Settings.NombreViaje = this.Orden.NomViaje;
                Settings.NombreSucursal = this.Orden.NomCentroCosto;
                Settings.NombreSolicitante = this.Orden.NombreSolicitante;
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
                        Orden.NumeroOrden > 0 ? "Se ha modificado la orden exitósamente" : "Se ha creado la orden exitósamente" ,
                        "Aceptar");



                MainViewModel.GetInstance().MisOrdenesTrabajo.LoadLista();
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

        private async void GetPresupuesto()
        {
            this.Presupuesto = 0;
            this.Aprobado = 0;
            this.Pendiente = 0;
            this.Saldo = 0;

            if (string.IsNullOrEmpty(Orden.IdSucursal))
            {
                return;
            }
            if (string.IsNullOrEmpty(Orden.IdViaje))
            {
                return;
            }
            this.Orden.TipoDocumento = TipoSelectedIndex == "Orden de trabajo" ? "OT" : "OK";
            var response_pro = await apiService.GetObject<PresupuestoModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "Presupuesto", "BARCO=" + this.Orden.IdSucursal+"&VIAJE="+this.Orden.IdViaje+"&CINV_TDOC="+this.Orden.TipoDocumento);
            if (!response_pro.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    response_pro.Message,
                    "Aceptar");
                return;
            }
            var Presu = (PresupuestoModel)response_pro.Result;
            if(Presu != null)
            {
                this.Presupuesto = Presu.Presupuesto;
                this.Aprobado = Presu.Aprobado;
                this.Pendiente = Presu.Pendiente;
                this.Saldo = Presu.Saldo;
            }

        }
        #endregion
    }
}
