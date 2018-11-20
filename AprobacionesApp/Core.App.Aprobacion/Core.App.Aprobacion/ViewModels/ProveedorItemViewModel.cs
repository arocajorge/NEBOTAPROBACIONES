namespace Core.App.Aprobacion.ViewModels
{
    using Core.App.Aprobacion.Models;
    using Core.App.Aprobacion.Views;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    public class ProveedorItemViewModel : ProveedorModel
    {
        #region Comandos
        public ICommand SelectProveedorCommand
        {
            get { return new RelayCommand(SelectProveedor); }
        }

        private void SelectProveedor()
        {
            App.MasterJefeSupervisor.IsPresented = false;
            MainViewModel.GetInstance().MisOrdenesTrabajoOrden.SetCombo(this.Codigo.ToString(), this.Nombre, Helpers.Enumeradores.eCombo.PROVEEDOR);
            App.Navigator.Navigation.PopAsync();
        }
        #endregion
    }
}
