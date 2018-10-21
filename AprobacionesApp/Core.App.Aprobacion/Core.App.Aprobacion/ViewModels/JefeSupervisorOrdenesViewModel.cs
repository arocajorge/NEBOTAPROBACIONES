using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Core.App.Aprobacion.ViewModels
{
    public class JefeSupervisorOrdenesViewModel : BaseViewModel
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
        public ObservableCollection<OrdenItemViewModel> ObservableCollection
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


        #region Metodos
        private IEnumerable<OrdenItemViewModel> ToOrdenItemModel()
        {
            return _lstOrden.Select(l => new OrdenItemViewModel
            {
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
                EstadoSupervisor = l.EstadoSupervisor
            });
        }
        #endregion

        #region Comandos
        public ICommand BuscarCommand
        {
            get { return new RelayCommand(Buscar); }
        }

        private void Buscar()
        {
            if (string.IsNullOrEmpty(filter))
                this.ObservableCollection = new ObservableCollection<OrdenItemViewModel>(ToOrdenItemModel());
            else
                this.ObservableCollection = new ObservableCollection<OrdenItemViewModel>(
                    ToOrdenItemModel().Where(q => q.NumeroOrden.ToString().Contains(filter.ToLower()) 
                    || q.NombreProveedor.ToLower().Contains(filter.ToLower()) 
                    || q.NombreSolicitante.ToLower().Contains(filter.ToLower())
                    || q.Fecha.ToShortDateString().Contains(filter.ToLower())
                    || q.Comentario.ToLower().Contains(filter.ToLower())
                    ).OrderBy(q => q.Fecha));
        }
        #endregion
    }
}
