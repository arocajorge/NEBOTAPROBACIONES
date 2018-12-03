using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Core.App.Aprobacion.ViewModels
{
    public class BitacoraItemViewModel : BitacoraModel
    {
        #region Comandos
        public ICommand SelectBitacoraCommand
        {
            get { return new RelayCommand(SelectBitacora); }
        }

        private void SelectBitacora()
        {
            App.MasterJefeSupervisor.IsPresented = false;
            MainViewModel.GetInstance().JefeSupervisorBitacora = new JefeSupervisorBitacoraViewModel(this);
            App.Navigator.Navigation.PushAsync(new JefeSupervisorBitacoraPage());
        }

        public ICommand SelectCumplimientoCommand
        {
            get { return new RelayCommand(SelectCumplimiento); }
        }

        private void SelectCumplimiento()
        {
            App.MasterCumplimiento.IsPresented = false;
            MainViewModel.GetInstance().CumplimientoLineas = new CumplimientoLineasViewModel(this);
            App.Navigator.Navigation.PushAsync(new CumplimientoLineasPage());
        }

        public ICommand AprobarCommand
        {
            get { return new RelayCommand(Aprobar); }
        }

        private void Aprobar()
        {
            this.Color = "LightGreen";
            this.EstadoSupervisor = "P";
            this.ImagenSupervisor = "ic_check_box";
        }
        #endregion
    }
}
