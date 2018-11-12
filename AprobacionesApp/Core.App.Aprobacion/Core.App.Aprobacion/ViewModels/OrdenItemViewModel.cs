using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Core.App.Aprobacion.ViewModels
{
    public class OrdenItemViewModel : OrdenModel
    {
        #region Comandos
        public ICommand SelectOrdenCommand
        {
            get { return new RelayCommand(SelectOrden); }
        }

        private void SelectOrden()
        {
            App.MasterJefeSupervisor.IsPresented = false;
            MainViewModel.GetInstance().JefeSupervisorOrden = new JefeSupervisorOrdenViewModel(this);
            App.Navigator.Navigation.PushAsync(new JefeSupervisorOrdenPage());
        }

        public ICommand UpdateOrdenCommand
        {
            get { return new RelayCommand(UpdateOrden); }
        }

        private void UpdateOrden()
        {
            App.MasterJefeSupervisor.IsPresented = false;
            MainViewModel.GetInstance().MisOrdenesTrabajoOrden = new MisOrdenesTrabajoOrdenViewModel(this);
            App.Navigator.Navigation.PushAsync(new MisOrdenesTrabajoOrdenPage());
        }
        #endregion
    }
}
