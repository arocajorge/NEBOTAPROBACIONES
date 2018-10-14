namespace Core.App.Aprobacion.ViewModels
{
    using Core.App.Aprobacion.Helpers;
    using Core.App.Aprobacion.Models;
    using Core.App.Aprobacion.Services;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AprobacionGerenteViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsRunning;
        private bool _IsEnabled;
        private ApiService apiService;
        private OrdenModel _orden;
        private int _Height;
        #endregion

        #region Propiedades
        public OrdenModel Orden
        {
            get { return this._orden; }
            set { SetValue(ref this._orden, value); }
        }
        public bool IsRunning
        {
            get { return this._IsRunning; }
            set { SetValue(ref this._IsRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set { SetValue(ref this._IsEnabled, value); }
        }
        public int Height
        {
            get { return this._Height; }
            set { SetValue(ref this._Height, value); }
        }
        #endregion

        #region Constructor
        public AprobacionGerenteViewModel()
        {
            apiService = new ApiService();
            CargarOrden();
        }
        #endregion

        #region Metodos
        private async void CargarOrden()
        {
            Height = 0;
            Response con = await apiService.CheckConnection(Settings.UrlConexion);
            if (!con.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    con.Message,
                    "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Settings.UrlConexion))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Dispositivo no configurado",
                    "Aceptar");
                return;
            }

            var response_cs = await apiService.GetObject<OrdenModel>(Settings.UrlConexion, Settings.RutaCarpeta, "OrdenTrabajo","");
            if (!response_cs.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    response_cs.Message,
                    "Aceptar");
                return;
            }

            Orden = (OrdenModel)response_cs.Result;
            if (Orden != null)
            {
                Orden.Titulo = Orden.TipoDocumento + " # " + Orden.NumeroOrden;
            }
            else
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "No existen órdenes pendientes",
                    "Aceptar");
                return;
            }

            Height = Orden.lst.Count * 40;
            this.IsEnabled = true;
            this.IsRunning = false;
        }
        #endregion

        #region Command
        public ICommand AprobarCommand
        {
            get
            {
                return new RelayCommand(Aprobar);
            }
        }

        private async void Aprobar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(this.Orden.Observacion))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Debe ingresar la observación",
                    "Aceptar");
                return;
            }
            this.Orden.Usuario = Settings.IdUsuario;
            this.Orden.Estado = "P";
            var response_sinc = await apiService.Post<OrdenModel>(
                Settings.UrlConexion,
                Settings.RutaCarpeta,
                "OrdenTrabajo",
                this.Orden);
            if (!response_sinc.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    response_sinc.Message,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Actualización de estado exitosa",
                    "Aceptar");
            

            CargarOrden();

            this.IsEnabled = true;
            this.IsRunning = false;
            
        }

        public ICommand ReprobarCommand
        {
            get
            {
                return new RelayCommand(Reprobar);
            }
        }

        private async void Reprobar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(this.Orden.Observacion))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Debe ingresar la observación",
                    "Aceptar");
                return;
            }
            this.Orden.Usuario = Settings.IdUsuario;
            this.Orden.Estado = "X";
            var response_sinc = await apiService.Post<OrdenModel>(
                Settings.UrlConexion,
                Settings.RutaCarpeta,
                "OrdenTrabajo",
                this.Orden);
            if (!response_sinc.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    response_sinc.Message,
                    "Aceptar");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Actualización de estado exitosa",
                    "Aceptar");

            CargarOrden();

            this.IsEnabled = true;
            this.IsRunning = false;
            
        }
        #endregion
    }
}
