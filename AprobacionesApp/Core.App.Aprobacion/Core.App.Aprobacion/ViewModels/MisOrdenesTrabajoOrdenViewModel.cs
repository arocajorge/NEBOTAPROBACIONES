using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        #endregion

        #region Propiedades
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
        #endregion

        #region Constructor
        public MisOrdenesTrabajoOrdenViewModel(OrdenModel Model)
        {
            provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            Fecha = Model.Fecha;
            Valor =  string.IsNullOrEmpty(Model.Valor) ? 0 : Convert.ToDecimal(Model.Valor,provider);
            ValorIva = string.IsNullOrEmpty(Model.ValorIva) ? 0 : Convert.ToDecimal(Model.ValorIva, provider);
            ValorOrden = Model.ValorOrden;
            IsEnabled = true;
            apiService = new ApiService();
            Orden = Model;
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
                    break;
                case Enumeradores.eCombo.VIAJE:
                    Orden.IdViaje = Codigo;
                    Orden.NomViaje = Nombre;
                    NomViaje = Nombre;
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
                    ValorIva = Valor * (12/100);
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

        private void BuscarSolicitante()
        {

        }

        public ICommand BuscarSucursalCommand
        {
            get { return new RelayCommand(BuscarSucursal); }
        }

        private async void BuscarSucursal()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.BARCO);
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }

        public ICommand BuscarViajeCommand
        {
            get { return new RelayCommand(BuscarViaje); }
        }

        private async void BuscarViaje()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.VIAJE);
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }
        public ICommand BuscarBodegaCommand
        {
            get { return new RelayCommand(BuscarBodega); }
        }

        private async void BuscarBodega()
        {
            MainViewModel.GetInstance().ComboCatalogos = new ComboCatalogosViewModel(Enumeradores.eCombo.BODEGA);
            await App.Navigator.PushAsync(new ComboCatalogosPage());
        }

        public ICommand GuardarCommand
        {
            get { return new RelayCommand(Guardar); }
        }

        private void Guardar()
        {

        }
        #endregion
    }
}
