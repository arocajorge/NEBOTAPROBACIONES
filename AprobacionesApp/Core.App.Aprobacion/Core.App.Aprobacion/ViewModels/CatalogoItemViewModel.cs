using Core.App.Aprobacion.Models;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Core.App.Aprobacion.ViewModels
{
    public class CatalogoItemViewModel : CatalogoModel
    {
        #region Comandos
        public ICommand SelectCatalogoCommand
        {
            get { return new RelayCommand(SelectCatalogo); }
        }

        private void SelectCatalogo()
        {
            App.MasterJefeSupervisor.IsPresented = false;
            MainViewModel.GetInstance().MisOrdenesTrabajoOrden.SetCombo(this.Codigo.ToString(), this.Descripcion, this.Combo);
            App.Navigator.Navigation.PopAsync();
        }
        #endregion
    }
}
