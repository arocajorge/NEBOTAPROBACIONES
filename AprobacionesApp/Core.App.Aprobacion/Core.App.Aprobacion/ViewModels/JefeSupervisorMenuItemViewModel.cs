using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class JefeSupervisorMenuItemViewModel
    {
        #region Propiedades
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        #endregion

        #region Comandos
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        private void Navigate()
        {
            App.Master.IsPresented = false;
            switch (this.PageName)
            {
                case "LoginPage":
                    #region Limpio los settings
                    #endregion
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
                case "JefeSupervisorFiltrosPage":
                    //MainViewModel.GetInstance().FiltroJefeSupervisor = new FiltroJefeSupervisorViewModel();
                    Application.Current.MainPage = new NavigationPage(new FiltroJefeSupervisorPage());
                    break;
                case "JefeSupervisorOrdenesPage":
                    MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
                    App.Navigator.PushAsync(new JefeSupervisorOrdenesPage());
                    break;
                case "JefeSupervisorBitacorasPage":
                    MainViewModel.GetInstance().JefeSupervisorBitacoras = new JefeSupervisorBitacorasViewModel();
                    App.Navigator.PushAsync(new JefeSupervisorBitacorasPage());
                    break;
            }
        }
        #endregion
    }
}
