using System;
using Newtonsoft.Json;

namespace Core.App.Aprobacion.Models
{
    public class BonoModel
    {
        [JsonProperty("ID")]
        public long Id { get; set; }

        [JsonProperty("BARCO")]
        public string Barco { get; set; }

        [JsonProperty("VIAJE")]
        public string Viaje { get; set; }

        [JsonProperty("OPCION1")]
        public string Opcion1 { get; set; }

        [JsonProperty("OP1_PREMIO")]
        public double Op1Premio { get; set; }

        [JsonProperty("OP1_MULTA")]
        public double Op1Multa { get; set; }

        [JsonProperty("OPCION2")]
        public string Opcion2 { get; set; }

        [JsonProperty("OP2_PREMIO")]
        public long Op2Premio { get; set; }

        [JsonProperty("OPCION3")]
        public string Opcion3 { get; set; }

        [JsonProperty("OPCION3_VERDE")]
        public string Opcion3Verde { get; set; }

        [JsonProperty("OPCION3_AZUL")]
        public string Opcion3Azul { get; set; }

        [JsonProperty("OPCION3_ROJO")]
        public string Opcion3Rojo { get; set; }

        [JsonProperty("OP3_VERDE")]
        public double Op3Verde { get; set; }

        [JsonProperty("OP3_AZUL")]
        public double Op3Azul { get; set; }

        [JsonProperty("OP3_ROJO")]
        public double Op3Rojo { get; set; }

        [JsonProperty("OPCION4")]
        public string Opcion4 { get; set; }

        [JsonProperty("OP4_PREMIO")]
        public double Op4Premio { get; set; }

        [JsonProperty("FECHA_ARRIBO")]
        public DateTime? FechaArribo { get; set; }

        [JsonProperty("FECHA_ZARPE")]
        public DateTime? FechaZarpe { get; set; }

        [JsonProperty("FECHA_ZARPE_REAL")]
        public DateTime? FechaZarpeReal { get; set; }

        [JsonProperty("TOTAL")]
        public double Total { get; set; }

        [JsonProperty("COLOR")]
        public string Color { get; set; }
    }
}