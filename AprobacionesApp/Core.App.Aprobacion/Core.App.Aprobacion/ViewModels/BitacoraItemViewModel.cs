using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
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
            App.Master.IsPresented = false;
            MainViewModel.GetInstance().JefeSupervisorBitacora = new JefeSupervisorBitacoraViewModel(this);
            App.Navigator.Navigation.PushAsync(new JefeSupervisorBitacoraPage());
        }
        #endregion
    }
}
