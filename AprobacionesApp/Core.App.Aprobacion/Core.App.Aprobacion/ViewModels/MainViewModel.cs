using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region ViewModels
        public LoginViewModel Login { get; set; }
        public ConfiguracionViewModel Configuracion { get; set; }
        public AprobacionGerenteViewModel AprobacionGerente { get; set; }
        public NoHayOrdenesPendientesViewModel NoHayOrdenesPendientes { get; set; }
        public PopUpBuscarOrdenViewModel PopUpBuscarOrden { get; set; }
        public FiltroJefeSupervisorViewModel FiltroJefeSupervisor { get; set; }
        public NoHayConexionViewModel NoHayConexion { get; set; }
        public JefeSupervisorOrdenesViewModel JefeSupervisorOrdenes { get; set; }
        public ObservableCollection<JefeSupervisorMenuItemViewModel> Menus { get; set; }
        public JefeSupervisorBitacorasViewModel JefeSupervisorBitacoras { get; set; }
        public JefeSupervisorOrdenViewModel JefeSupervisorOrden { get; set; }
        public JefeSupervisorBitacoraViewModel JefeSupervisorBitacora { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
            loadMenu();
        }
        #endregion

        #region SingleTon
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            else
                return instance;
        }
        #endregion

        #region Metodos
        private void loadMenu()
        {
            this.Menus = new ObservableCollection<JefeSupervisorMenuItemViewModel>();
            
            this.Menus.Add(new JefeSupervisorMenuItemViewModel
            {
                Icon = "ic_filter_1",
                PageName = "JefeSupervisorOrdenesPage",
                Title = "Ordenes"
            });
            this.Menus.Add(new JefeSupervisorMenuItemViewModel
            {
                Icon = "ic_filter_2",
                PageName = "JefeSupervisorBitacorasPage",
                Title = "Bitácoras"
            });
            this.Menus.Add(new JefeSupervisorMenuItemViewModel
            {
                Icon = "ic_location_on",
                PageName = "JefeSupervisorFiltrosPage",
                Title = "Filtros"
            });
            this.Menus.Add(new JefeSupervisorMenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = "Cerrar sesión"
            });
        }
        #endregion

        #region Comandos
        public ICommand BuscarOrdenCommand
        {
            get
            {
                return new RelayCommand(BuscarOrden);
            }
        }

        private async void BuscarOrden()
        {
            try
            {
                PopUpBuscarOrden = new PopUpBuscarOrdenViewModel();
                await PopupNavigation.PushAsync(new PopUpBuscarOrdenPage());
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                       "Alerta",
                       ex.Message,
                       "Aceptar");
                return;
            }
            
        }
        #endregion

      
    }
}
