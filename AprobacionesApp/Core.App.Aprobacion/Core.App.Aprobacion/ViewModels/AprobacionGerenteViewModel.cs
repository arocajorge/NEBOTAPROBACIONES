namespace Core.App.Aprobacion.ViewModels
{
    using Core.App.Aprobacion.Helpers;
    using Core.App.Aprobacion.Models;
    using Core.App.Aprobacion.Services;
    using Core.App.Aprobacion.Views;
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
        private bool _EsChatarraVisible;
        private bool _NoEsChatarraVisible;
        private string _color;
        private bool _mostrarAnular;
        #endregion

        #region Propiedades
        public string Color
        {
            get { return this._color; }
            set { SetValue(ref this._color, value); }
        }
        public bool MostrarAnular
        {
            get { return this._mostrarAnular; }
            set { SetValue(ref this._mostrarAnular, value); }
        }
        public OrdenModel Orden
        {
            get { return this._orden; }
            set { SetValue(ref this._orden, value);}
        }
        public bool IsRunning
        {
            get { return this._IsRunning; }
            set { SetValue(ref this._IsRunning, value); }
        }
        public bool EsChatarraVisible
        {
            get { return this._EsChatarraVisible; }
            set { SetValue(ref this._EsChatarraVisible, value); }
        }
        public bool NoEsChatarraVisible
        {
            get { return this._NoEsChatarraVisible; }
            set { SetValue(ref this._NoEsChatarraVisible, value); }
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
        public AprobacionGerenteViewModel(OrdenModel model)
        {
            Color = "Black";
            this.IsEnabled = true;
            this.IsRunning = false;
            apiService = new ApiService();
            Orden = model;
            if (Orden.TipoDocumento == "OC")
            {
                NoEsChatarraVisible = false;
                EsChatarraVisible = true;
            }
            else
            {
                NoEsChatarraVisible = true;
                EsChatarraVisible = false;
            }

            Height = Orden.lst == null ? 0 : Orden.lst.Count * 50;
        }
        public AprobacionGerenteViewModel()
        {
            Color = "Black";
            this.IsEnabled = true;
            this.IsRunning = false;
            apiService = new ApiService();            
            CargarOrden();
        }
        #endregion

        #region Metodos
        private async void CargarOrden()
        {
            try
            {
                Height = 0;
                MostrarAnular = true;
                if (string.IsNullOrEmpty(Settings.UrlConexionActual))
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Alerta",
                        "Dispositivo no configurado",
                        "Aceptar");
                    return;
                }

                Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
                if (!con.IsSuccess)
                {
                    string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                    con = await apiService.CheckConnection(UrlDistinto);
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
                    else
                        Settings.UrlConexionActual = UrlDistinto;
                }

                var response_cs = await apiService.GetObject<OrdenModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "OrdenTrabajo", "");
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
                if (Orden != null && Orden.NumeroOrden != 0)
                {
                    string TipoDocumento = Orden.TipoDocumento;
                    switch (TipoDocumento)
                    {
                        case "OC":
                            TipoDocumento = "Orden de compra";
                            break;
                        case "OK":
                            TipoDocumento = "Orden de caja chica";
                            break;
                        case "OT":
                            TipoDocumento = "Orden de trabajo";
                            break;
                    }

                    if (Orden.TipoDocumento == "OC")
                    {
                        NoEsChatarraVisible = false;
                        EsChatarraVisible = true;
                    }
                    else
                    {
                        NoEsChatarraVisible = true;
                        EsChatarraVisible = false;
                    }
                    MostrarAnular = true;
                    if (Orden.Estado == "X")
                    {
                        Color = "Red";
                        MostrarAnular = false;
                        Orden.Estado = "Anulada";
                    }else
                        if (Orden.Estado == "A")
                    {
                        Color = "Black";
                        Orden.Estado = "Pendiente";
                    }else
                    if (Orden.Estado == "P")
                    {
                        Color = "Green";
                        Orden.Estado = "Aprobada";
                    }


                    Orden.Titulo = TipoDocumento + " No. " + Orden.NumeroOrden;
                    Height = Orden.lst == null ? 0 : Orden.lst.Count * 50;

                    IsEnabled = true;
                    IsRunning = false;
                }
                else
                {
                    MainViewModel.GetInstance().NoHayOrdenesPendientes = new NoHayOrdenesPendientesViewModel();
                    Application.Current.MainPage = new NavigationPage(new NoHayOrdenesPendientesPage());
                }
            }
            catch (System.Exception ex)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    ex.Message,
                    "Aceptar");

                return;
            }
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
                this.Orden.Observacion = string.Empty;
            }

            Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
            if (!con.IsSuccess)
            {
                string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                con = await apiService.CheckConnection(UrlDistinto);
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
                else
                    Settings.UrlConexionActual = UrlDistinto;
            }

            this.Orden.Usuario = Settings.IdUsuario;
            this.Orden.Estado = "P";
            var response_sinc = await apiService.Post<OrdenModel>(
                Settings.UrlConexionActual,
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
                this.Orden.Observacion = string.Empty;
            }

            Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
            if (!con.IsSuccess)
            {
                string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                con = await apiService.CheckConnection(UrlDistinto);
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
                else
                    Settings.UrlConexionActual = UrlDistinto;
            }

            this.Orden.Usuario = Settings.IdUsuario;
            this.Orden.Estado = "X";
            var response_sinc = await apiService.Post<OrdenModel>(
                Settings.UrlConexionActual,
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
        public ICommand PasarCommand
        {
            get
            {
                return new RelayCommand(Pasar);
            }
        }

        private async void Pasar()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
            if (string.IsNullOrEmpty(this.Orden.Observacion))
            {
                this.Orden.Observacion = string.Empty;
            }

            Response con = await apiService.CheckConnection(Settings.UrlConexionActual);
            if (!con.IsSuccess)
            {
                string UrlDistinto = Settings.UrlConexionActual == Settings.UrlConexionExterna ? Settings.UrlConexionInterna : Settings.UrlConexionExterna;
                con = await apiService.CheckConnection(UrlDistinto);
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
                else
                    Settings.UrlConexionActual = UrlDistinto;
            }

            this.Orden.Usuario = Settings.IdUsuario;
            this.Orden.Estado = "PASAR";
            var response_sinc = await apiService.Post<OrdenModel>(
                Settings.UrlConexionActual,
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

            CargarOrden();

            this.IsEnabled = true;
            this.IsRunning = false;

        }
        #endregion
    }
}
