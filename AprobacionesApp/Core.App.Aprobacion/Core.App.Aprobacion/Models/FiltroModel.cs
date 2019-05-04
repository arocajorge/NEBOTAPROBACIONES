using System;
using Newtonsoft.Json;

namespace Core.App.Aprobacion.Models
{
    public class FiltroModel
    {
        [JsonProperty("USUARIO")]
        public string Usuario { get; set; }
        [JsonProperty("BARCO")]
        public string Sucursal { get; set; }
        [JsonProperty("VIAJE")]
        public string Viaje { get; set; }
        [JsonProperty("SOLICITANTE")]
        public string Solicitante { get; set; }
        [JsonProperty("BODEGA")]
        public string Bodega { get; set; }
        [JsonProperty("NOMBREBARCO")]
        public string NombreBarco { get; set; }
        [JsonProperty("NOMBREVIAJE")]
        public string NombreViaje { get; set; }
        [JsonProperty("NOMBRESOLICITANTE")]
        public string NombreSolicitante { get; set; }
        [JsonProperty("NOMBREBODEGA")]
        public string NombreBodega { get; set; }
    }
}
