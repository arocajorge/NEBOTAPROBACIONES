using System;
using Newtonsoft.Json;

namespace Core.App.Aprobacion.Models
{
    public class PresupuestoModel
    {
        [JsonProperty("BARCO")]
        public string Barco { get; set; }

        [JsonProperty("VIAJE")]
        public string Viaje { get; set; }

        [JsonProperty("PRESUPUESTO")]
        public decimal Presupuesto { get; set; }

        [JsonProperty("APROBADO")]
        public decimal Aprobado { get; set; }

        [JsonProperty("PENDIENTE")]
        public decimal Pendiente { get; set; }

        [JsonProperty("SALDO")]
        public decimal Saldo { get; set; }

        [JsonProperty("CINV_TDOC")]
        public string Tipo { get; set; }
    }
}
