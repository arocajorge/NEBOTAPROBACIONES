namespace Core.App.Aprobacion.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;
    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        const string urlConexionExterna = "urlConexionExterna";
        const string urlConexionInterna = "urlConexionInterna";
        const string urlConexionActual = "urlConexionActual";
        const string rutaCarpeta = "rutaCarpeta";
        const string idUsuario = "idUsuario";
        static readonly string stringDefault = string.Empty;

        public static string UrlConexionExterna
        {
            get
            {
                return AppSettings.GetValueOrDefault(urlConexionExterna, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(urlConexionExterna, value);
            }
        }
        public static string UrlConexionInterna
        {
            get
            {
                return AppSettings.GetValueOrDefault(urlConexionInterna, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(urlConexionInterna, value);
            }
        }
        public static string UrlConexionActual
        {
            get
            {
                return AppSettings.GetValueOrDefault(urlConexionActual, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(urlConexionActual, value);
            }
        }
        public static string RutaCarpeta
        {
            get
            {
                return AppSettings.GetValueOrDefault(rutaCarpeta, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(rutaCarpeta, value);
            }
        }

        public static string IdUsuario
        {
            get
            {
                return AppSettings.GetValueOrDefault(idUsuario, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(idUsuario, value);
            }
        }
    }
}
