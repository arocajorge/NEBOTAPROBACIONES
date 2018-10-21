using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
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
        #endregion

        #region Constructor
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
            
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
