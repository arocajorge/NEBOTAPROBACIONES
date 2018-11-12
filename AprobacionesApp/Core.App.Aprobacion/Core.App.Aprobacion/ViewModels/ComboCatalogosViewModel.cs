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
    public class ComboCatalogosViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<CatalogoItemViewModel> _lstDet;        
        private string _filter;
        private ApiService apiService;
        private bool _IsRefreshing;
        private Enumeradores.eCombo Combo;
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
        public ObservableCollection<CatalogoItemViewModel> LstCatalogo
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
        public ComboCatalogosViewModel(Enumeradores.eCombo _Combo)
        {
            apiService = new ApiService();
            Combo = _Combo;
            LoadLista();
        }
        #endregion

        #region Metodos
        private IEnumerable<CatalogoItemViewModel> ToCatalogoItemModel()
        {
            var temp = MainViewModel.GetInstance().ListaCatalogos.Where(q=>q.Combo == Combo).Select(l => new CatalogoItemViewModel
            {
                Codigo = l.Codigo,
                Descripcion = l.Descripcion,
                Combo = l.Combo
            });
            return temp;
        }
        public async void LoadLista()
        {
            IsRefreshing = true;
            try
            {
                this.LstCatalogo = new ObservableCollection<CatalogoItemViewModel>(ToCatalogoItemModel());
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
                    this.LstCatalogo = new ObservableCollection<CatalogoItemViewModel>(ToCatalogoItemModel());
                else
                    this.LstCatalogo = new ObservableCollection<CatalogoItemViewModel>(
                        ToCatalogoItemModel().Where(q => q.Descripcion.ToLower().Contains(filter.ToLower())
                        ).OrderBy(q => q.Descripcion));
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
