using System;
using Newtonsoft.Json;

namespace Core.App.Aprobacion.Models
{
    public class BonoDetModel
    {
        [JsonProperty("TITULO")]
        public string Titulo { get; set; }

        [JsonProperty("DESCRIPCION")]
        public string Descripcion { get; set; }

        [JsonProperty("FECHA")]
        public DateTime? Fecha { get; set; }

        [JsonProperty("DIAS")]
        public int? Dias { get; set; }

        [JsonProperty("COLOR")]
        public string Color { get; set; }
    }
}
