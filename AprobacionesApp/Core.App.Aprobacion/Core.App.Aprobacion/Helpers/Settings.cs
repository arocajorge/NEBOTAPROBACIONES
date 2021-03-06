﻿namespace Core.App.Aprobacion.Helpers
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

        const string sucursal = "sucursal";
        const string viaje = "viaje";
        const string bodega = "bodega";
        const string solicitante = "solicitante";
        const string nombreSucursal = "nombreSucursal";
        const string nombreViaje = "nombreViaje";
        const string nombreBodega = "nombreBodega";
        const string nombreSolicitante = "nombreSolicitante";
        const string fechaInicio = "fechaInicio";
        const string estadoGerente = "estadoGerente";
        const string rolApro = "rolApro";

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
        public static string RolApro
        {
            get
            {
                return AppSettings.GetValueOrDefault(rolApro, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(rolApro, value);
            }
        }

        public static string Sucursal
        {
            get
            {
                return AppSettings.GetValueOrDefault(sucursal, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(sucursal, value);
            }
        }
        public static string Viaje
        {
            get
            {
                return AppSettings.GetValueOrDefault(viaje, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(viaje, value);
            }
        }
        public static string NombreSucursal
        {
            get
            {
                return AppSettings.GetValueOrDefault(nombreSucursal, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nombreSucursal, value);
            }
        }
        public static string NombreViaje
        {
            get
            {
                return AppSettings.GetValueOrDefault(nombreViaje, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nombreViaje, value);
            }
        }
        public static string EstadoGerente
        {
            get
            {
                return AppSettings.GetValueOrDefault(estadoGerente, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(estadoGerente, value);
            }
        }
        public static string FechaInicio
        {
            get
            {
                return AppSettings.GetValueOrDefault(fechaInicio, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(fechaInicio, value);
            }
        }
        public static string Solicitante
        {
            get
            {
                return AppSettings.GetValueOrDefault(solicitante, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(solicitante, value);
            }
        }
        public static string Bodega
        {
            get
            {
                return AppSettings.GetValueOrDefault(bodega, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(bodega, value);
            }
        }
        public static string NombreSolicitante
        {
            get
            {
                return AppSettings.GetValueOrDefault(nombreSolicitante, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nombreSolicitante, value);
            }
        }
        public static string NombreBodega
        {
            get
            {
                return AppSettings.GetValueOrDefault(nombreBodega, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(nombreBodega, value);
            }
        }
    }
}
