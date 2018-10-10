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
        const string urlConexion = "urlConexion";
        const string rutaCarpeta = "rutaCarpeta";
        const string idUsuario = "idUsuario";
        static readonly string stringDefault = string.Empty;
        static readonly int intDefault = 0;

        public static string UrlConexion
        {
            get
            {
                return AppSettings.GetValueOrDefault(urlConexion, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(urlConexion, value);
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
