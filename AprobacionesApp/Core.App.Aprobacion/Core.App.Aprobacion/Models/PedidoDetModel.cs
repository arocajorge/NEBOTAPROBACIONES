using System;
using Newtonsoft.Json;

namespace Core.App.Aprobacion.Models
{
    public class PedidoDetModel
    {
        [JsonProperty("PED_ID")]
        public long ID { get; set; }
        [JsonProperty("PED_SEC")]
        public int Secuencia { get; set; }
        [JsonProperty("PED_CANT")]
        public Nullable<decimal> Cantidad { get; set; }
        [JsonProperty("PED_DETALLE")]
        public string Detalle { get; set; }
    }
}
