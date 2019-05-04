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

        public bool ModificaProveedor { get; set; }

        private void SelectProveedor()
        {
            if (!ModificaProveedor)
            {
                if(App.MasterJefeSupervisor != null)
                    App.MasterJefeSupervisor.IsPresented = false;
                if (App.MasterGerente != null)
                    App.MasterGerente.IsPresented = false;
                MainViewModel.GetInstance().MisOrdenesTrabajoOrden.SetCombo(this.Codigo.ToString(), this.Nombre, Helpers.Enumeradores.eCombo.PROVEEDOR);
                App.Navigator.Navigation.PopAsync();
            }
            else
            {
                if (App.MasterJefeSupervisor != null)
                    App.MasterJefeSupervisor.IsPresented = false;
                if (App.MasterGerente != null)
                    App.MasterGerente.IsPresented = false;
                MainViewModel.GetInstance().Proveedor = new ProveedorViewModel(this);
                App.Navigator.Navigation.PushAsync(new ProveedorPage());
            }

        }
        #endregion
    }
}
