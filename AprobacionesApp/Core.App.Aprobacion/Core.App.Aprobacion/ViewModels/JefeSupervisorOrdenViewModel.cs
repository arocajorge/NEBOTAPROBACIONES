using Core.App.Aprobacion.Helpers;
using Core.App.Aprobacion.Models;
using Core.App.Aprobacion.Services;
using Core.App.Aprobacion.Views;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin.Forms;

namespace Core.App.Aprobacion.ViewModels
{
    public class JefeSupervisorOrdenViewModel : BaseViewModel
    {
        #region Variables
        private bool _IsRunning;
        private bool _IsEnabled;
        private ApiService apiService;
        private OrdenModel _orden;
        private int _Height;
        private string _color;
        private bool _esJefe;
        private bool _esSupervisor;
        private bool _mostrarAnular;
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
        public bool EsJefe
        {
            get { return this._esJefe; }
            set { SetValue(ref this._esJefe, value); }
        }
        public bool EsSupervisor
        {
            get { return this._esSupervisor; }
            set { SetValue(ref this._esSupervisor, value); }
        }
        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set { SetValue(ref this._IsEnabled, value); }
        }
        public bool MostrarAnular
        {
            get { return this._mostrarAnular; }
            set { SetValue(ref this._mostrarAnular, value); }
        }
        public int Height
        {
            get { return this._Height; }
            set { SetValue(ref this._Height, value); }
        }
        public string Color
        {
            get { return this._color; }
            set { SetValue(ref this._color, value); }
        }
        #endregion

        #region Constructor
        public JefeSupervisorOrdenViewModel(OrdenModel Model)
        {
            Color = "Black";
            apiService = new ApiService();
            Orden = new OrdenModel();
            Orden = Model;
            CargarOrden();
        }
        #endregion

        #region Metodos
        private async void CargarOrden()
        {
            this.IsEnabled = false;
            this.IsRunning = true;
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
                
                var response_cs = await apiService.GetObject<OrdenModel>(Settings.UrlConexionActual, Settings.RutaCarpeta, "OrdenTrabajo", "CINV_TDOC=OT&CINV_NUM="+Orden.NumeroOrden);
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

                    if (Settings.RolApro == "J")
                    {
                        EsJefe = true;
                        EsSupervisor = false;
                        Color = Orden.EstadoJefe == "A" ? "Black" : (Orden.EstadoJefe == "P" ? "Green" : (Orden.EstadoJefe == "X" ? "Red" : "Black"));
                        Orden.EstadoJefe = Orden.EstadoJefe == "A" ? "Pendiente" : (Orden.EstadoJefe == "P" ? "Aprobada" : (Orden.EstadoJefe == "X" ? "Anulada" : "Pendiente"));
                        MostrarAnular = Orden.EstadoJefe == "Anulada" ? false : true;
                    }
                    else
                    {
                        EsJefe = false;
                        EsSupervisor = true;
                        Color = Orden.EstadoSupervisor == "A" ? "Black" : (Orden.EstadoSupervisor == "P" ? "Green" : (Orden.EstadoSupervisor == "X" ? "Red" : "Black"));
                        Orden.EstadoSupervisor = Orden.EstadoSupervisor == "A" ? "Pendiente" : (Orden.EstadoSupervisor == "P" ? "Aprobada" : (Orden.EstadoSupervisor == "X" ? "Anulada" : "Pendiente"));
                        MostrarAnular = Orden.EstadoSupervisor == "Anulada" ? false : true;
                    }

                    Orden.Titulo = TipoDocumento + " No. " + Orden.NumeroOrden;
                    Height = Orden.lst == null ? 0 : Orden.lst.Count * 50;
                    
                }
                IsEnabled = true;
                IsRunning = false;
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
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Alerta",
                    "Ingrese la observación",
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

            this.IsEnabled = true;
            this.IsRunning = false;

            MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
            Application.Current.MainPage = new JefeSupervisorMasterPage();

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
                    "Ingrese la observación",
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

            MainViewModel.GetInstance().JefeSupervisorOrdenes = new JefeSupervisorOrdenesViewModel();
            Application.Current.MainPage = new JefeSupervisorMasterPage();

            this.IsEnabled = true;
            this.IsRunning = false;
        }
        #endregion
    }
}
