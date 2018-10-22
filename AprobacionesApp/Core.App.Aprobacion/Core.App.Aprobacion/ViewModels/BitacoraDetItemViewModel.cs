using Core.App.Aprobacion.Models;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Core.App.Aprobacion.ViewModels
{
    public class BitacoraDetItemViewModel : BitacoraDetModel
    {


        #region Comandos
        public ICommand EliminarOrdenCommand
        {
            get { return new RelayCommand(EliminarOrden); }
        }

        private async void EliminarOrden()
        {
                            
        }
        #endregion
    }
}
