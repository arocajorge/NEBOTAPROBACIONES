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
    public class ComboProveedoresViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<ProveedorItemViewModel> _lstDet;
        private string _filter;
        private ApiService apiService;
        private bool _IsRefreshing;
        private bool _ModificaProveedor;
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
        public bool ModificaProveedor
        {
            get { return this._ModificaProveedor; }
            set
            {
                SetValue(ref this._ModificaProveedor, value);
            }
        }
        public ObservableCollection<ProveedorItemViewModel> LstProveedores
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
        public ComboProveedoresViewModel(bool Modifica = false)
        {
            ModificaProveedor = Modifica;
            apiService = new ApiService();
            LoadLista();
        }
        #endregion

        #region Metodos
        private IEnumerable<ProveedorItemViewModel> ToProveedorItemModel()
        {
            var temp = MainViewModel.GetInstance().ListaProveedores.Select(l => new ProveedorItemViewModel
            {
                Codigo = l.Codigo,
                Nombre = l.Nombre,
                Identificacion = l.Identificacion,
                EMail = l.EMail,
                ModificaProveedor = ModificaProveedor,
                Duracion = l.Duracion,
                Telefonos = l.Telefonos,
            });
            return temp;
        }
        public async void LoadLista()
        {
            IsRefreshing = true;
            try
            {   
                this.LstProveedores = new ObservableCollection<ProveedorItemViewModel>(ToProveedorItemModel());
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
                    this.LstProveedores = new ObservableCollection<ProveedorItemViewModel>(ToProveedorItemModel());
                else
                    this.LstProveedores = new ObservableCollection<ProveedorItemViewModel>(
                        ToProveedorItemModel().Where(q => q.Nombre.ToLower().Contains(filter.ToLower())
                        ).OrderBy(q => q.Nombre));
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
