namespace Core.App.Aprobacion.Models
{
    using Core.App.Aprobacion.ViewModels;
    using Newtonsoft.Json;
    public class OrdenDetalleModel 
    {
        [JsonProperty("DINV_LINEA")]
        public int Linea { get; set; }
        [JsonProperty("DINV_DETALLEDSCTO")]
        public string Detalle { get; set; }
        [JsonProperty("DINV_COS")]
        public decimal CostoUnitario { get; set; }
        [JsonProperty("DINV_PRCT_DSC")]
        public decimal Descuento { get; set; }
        [JsonProperty("DINV_VTA")]
        public decimal Subtotal { get; set; }
        [JsonProperty("TOTAL")]
        public decimal Total { get; set; }
        [JsonProperty("CINV_COM3")]
        public string CINV_COM3 { get; set; }
        [JsonProperty("CINV_COM4")]
        public string CINV_COM4 { get; set; }
        [JsonProperty("DINV_IVA")]
        public decimal Iva { get; set; }
        [JsonProperty("CINV_TDOC")]
        public string TipoDocumento { get; set; }
        [JsonProperty("UNIDAD")]
        public string Unidad { get; set; }
        [JsonProperty("ESCHATARRA")]
        public bool EsChatarra { get; set; }
        [JsonProperty("DINV_CANT")]
        public double Cantidad { get; set; }
    }
}
