using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.App.Aprobacion.Models
{
    public class BitacoraDetModel
    {
        [JsonProperty("ID")]
        public int Id { get; set; }

        [JsonProperty("LINEA")]
        public int Linea { get; set; }

        [JsonProperty("LINEA_DETALLE")]
        public int LineaDetalle { get; set; }

        [JsonProperty("NUMERO_ORDEN")]
        public int NumeroOrden { get; set; }

        [JsonProperty("VALOR")]
        public double Valor { get; set; }

        [JsonProperty("CUMPLIMIENTO")]
        public long Cumplimiento { get; set; }

        [JsonProperty("EMPRESA")]
        public string Empresa { get; set; }

        [JsonProperty("NOMPROVEEDOR")]
        public string Nomproveedor { get; set; }

        [JsonProperty("DETALLEOT")]
        public string Detalleot { get; set; }

        [JsonProperty("ELIMINAR")]
        public bool Eliminar { get; set; }

        [JsonProperty("CINV_NUM")]
        public long CinvNum { get; set; }

        [JsonProperty("CINV_COM1")]
        public string Comentario { get; set; }
    }
}
