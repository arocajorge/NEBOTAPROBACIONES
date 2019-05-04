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
            if (this.Pantalla == "O")
            {
                MainViewModel.GetInstance().MisOrdenesTrabajoOrden.SetCombo(this.Codigo, this.Descripcion, this.Combo);
            }
            else
                if (this.Pantalla == "B")
            {
                MainViewModel.GetInstance().CreacionBitacora.SetCombo(this.Codigo, this.Descripcion, this.Combo);
            }
            else
                MainViewModel.GetInstance().MisPedidosPedido.SetCombo(this.Codigo, this.Descripcion, this.Combo);

            App.Navigator.Navigation.PopAsync();
        }
        #endregion
    }
}
