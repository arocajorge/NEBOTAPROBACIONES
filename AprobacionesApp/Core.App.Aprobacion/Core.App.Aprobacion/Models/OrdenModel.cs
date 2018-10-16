﻿namespace Core.App.Aprobacion.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class OrdenModel
    {
        [JsonProperty("CINV_TDOC")]
        public string TipoDocumento { get; set; }
        [JsonProperty("CINV_NUM")]
        public int NumeroOrden { get; set; }
        [JsonProperty("CINV_NOMID")]
        public string NombreProveedor { get; set; }
        [JsonProperty("NOM_SOLICITADO")]
        public string NombreSolicitante { get; set; }
        [JsonProperty("VALOR_OT")]
        public decimal ValorOrden { get; set; }
        [JsonProperty("CINV_FECING")]
        public DateTime Fecha { get; set; }
        [JsonProperty("CINV_ST")]
        public string Estado { get; set; }        
        [JsonProperty("CINV_MOTIVOANULA")]
        public string Observacion { get; set; }
        [JsonProperty("CINV_LOGIN")]
        public string Usuario { get; set; }
        public string Titulo { get; set; }
        public List<OrdenDetalleModel> lst { get; set; }
    }
}