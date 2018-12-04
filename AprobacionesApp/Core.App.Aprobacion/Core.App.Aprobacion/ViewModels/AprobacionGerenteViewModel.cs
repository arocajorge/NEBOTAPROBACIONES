namespace Core.App.Aprobacion.ViewModels
{
    using Core.App.Aprobacion.Helpers;
    using Core.App.Aprobacion.Models;
    using Core.App.Aprobacion.Services;
    using Core.App.Aprobacion.Views;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using System.Linq;
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
        private string _estado;
        private ObservableCollection<OrdenDetalleModel> _ListaDetalle;
        #endregion

        #region Propiedades
        public ObservableCollection<OrdenDetalleModel> ListaDetalle
        {
            get { return this._ListaDetalle; }
            set { SetValue(ref this._ListaDetalle, value); }
        }
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
        public string Estado
        {
            get { return this._estado; }
            set { SetValue(ref this._estado, value); }
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
            MostrarAnular = true;
            if (Orden.Estado.Trim().ToUpper() == "X")
            {
                Color = "Red";
                MostrarAnular = false;
                Estado = "Anulada";
            }
            else
                if (Orden.Estado.Trim().ToUpper() == "A")
            {
                Color = "Black";
                Estado = "Pendiente";
            }
            else
            if (Orden.Estado.Trim().ToUpper() == "P")
            {
                Color = "Green";
                Estado = "Aprobada";
            }

            int Longitud = Orden.lst.Max(q => q.Cantidad.ToString().Length);

            Orden.lst.ForEach(q => {
                q.SecuenciaDetalle = q.Linea - 1;
                q.Longitud = Longitud * 15;
            });
            ListaDetalle = new ObservableCollection<OrdenDetalleModel>(Orden.lst);
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

                var response_cs = await apiService.GetObject<OrdenModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "OrdenTrabajo", "CINV_LOGIN="+Settings.IdUsuario);
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
                        case "OR":
                            TipoDocumento = "Orden recurrente";
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
                    if (Orden.Estado.Trim().ToUpper() == "X")
                    {
                        Color = "Red";
                        MostrarAnular = false;
                        Estado = "Anulada";
                    }else
                        if (Orden.Estado.Trim().ToUpper() == "A")
                    {
                        Color = "Black";
                        Estado = "Pendiente";
                    }else
                    if (Orden.Estado.Trim().ToUpper() == "P")
                    {
                        Color = "Green";
                       Estado = "Aprobada";
                    }
                    Orden.Titulo = TipoDocumento + " No. " + Orden.NumeroOrden;
                    Height = Orden.lst == null ? 0 : Orden.lst.Count * 50;

                    int Longitud = Orden.lst.Max(q => q.Cantidad.ToString().Length);
                    Orden.lst.ForEach(q => {
                        q.SecuenciaDetalle = q.Linea - 1;
                        q.Longitud = Longitud * 15;
                    });

                    Orden.lst.ForEach(q => q.SecuenciaDetalle = q.Linea - 1);
                    ListaDetalle = new ObservableCollection<OrdenDetalleModel>(Orden.lst);
                    
                    IsEnabled = true;
                    IsRunning = false;
                }
                else
                {
                    MainViewModel.GetInstance().NoHayOrdenesPendientes = new NoHayOrdenesPendientesViewModel();
                    Application.Current.MainPage = new NoHayOrdenesMasterPage();
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
            this.Orden.lst = new System.Collections.Generic.List<OrdenDetalleModel>(ListaDetalle);
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
            this.Orden.lst = new System.Collections.Generic.List<OrdenDetalleModel>(ListaDetalle);
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
