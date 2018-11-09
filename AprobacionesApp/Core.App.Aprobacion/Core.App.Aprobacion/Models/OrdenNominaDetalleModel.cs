namespace Core.App.Aprobacion.Models
{
    using Newtonsoft.Json;
    public class OrdenNominaDetalleModel
    {
        [JsonProperty("DINV_CTINV")]
        public int NumeroOrden { get; set; }
        [JsonProperty("DINV_LINEA")]
        public short Linea { get; set; }
        [JsonProperty("DINV_ST")]
        public string EstadoString { get; set; }
        [JsonProperty("DINV_DETALLEDSCTO")]
        public string Detalle { get; set; }
        public bool Estado { get; set; }
    }
}
