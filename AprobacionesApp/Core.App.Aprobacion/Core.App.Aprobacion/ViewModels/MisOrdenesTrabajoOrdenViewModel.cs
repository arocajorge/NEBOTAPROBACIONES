﻿using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

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
        #endregion

        #region Constructor
        public MisOrdenesTrabajoOrdenViewModel(OrdenModel Model)
        {
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
                    break;
                case Enumeradores.eCombo.VIAJE:
                    Orden.IdViaje = Codigo;
                    Orden.NomViaje = Nombre;
                    break;
                case Enumeradores.eCombo.SOLICITANTE:
                    Orden.IdSolicitante = Codigo;
                    Orden.NombreSolicitante = Nombre;
                    break;
                case Enumeradores.eCombo.BODEGA:
                    Orden.IdBodega = Codigo;
                    Orden.NombreBodega = Nombre;
                    break;
                default:
                    break;
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

        public ICommand BuscarBarcoCommand
        {
            get { return new RelayCommand(BuscarBarco); }
        }

        private void BuscarBarco()
        {

        }

        public ICommand BuscarViajeCommand
        {
            get { return new RelayCommand(BuscarViaje); }
        }

        private void BuscarViaje()
        {

        }
        public ICommand BuscarBodegaCommand
        {
            get { return new RelayCommand(BuscarBodega); }
        }

        private void BuscarBodega()
        {

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
