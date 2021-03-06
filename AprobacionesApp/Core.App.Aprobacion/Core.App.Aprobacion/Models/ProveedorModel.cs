﻿namespace Core.App.Aprobacion.Models
{
    using Newtonsoft.Json;
    public class ProveedorModel
    {
        [JsonProperty("CODIGO")]        
        public string Codigo { get; set; }

        [JsonProperty("NOMBRE")]
        public string Nombre { get; set; }

        [JsonProperty("CEDULA")]
        public string Cedula { get; set; }
        [JsonProperty("RUC")]
        public string Ruc { get; set; }

        [JsonProperty("E_MAIL")]
        public string EMail { get; set; }

        [JsonProperty("TELEFONOS")]
        public string Telefonos { get; set; }

        [JsonProperty("DURACION")]
        public decimal? Duracion { get; set; }

        [JsonProperty("DURACIONACUMULADA")]
        public decimal? DuracionAcumulada { get; set; }

        [JsonProperty("Color")]
        public string Color { get; set; }

        public string Identificacion { get; set; }
    }
}
