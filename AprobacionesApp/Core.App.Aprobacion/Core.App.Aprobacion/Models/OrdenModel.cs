namespace Core.App.Aprobacion.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class OrdenModel
    {
        [JsonProperty("CINV_SEC")]
        public long Secuencia { get; set; }
        [JsonProperty("CINV_TDOC")]
        public string TipoDocumento { get; set; }
        [JsonProperty("CINV_NUM")]
        public int NumeroOrden { get; set; }
        [JsonProperty("CINV_ID")]
        public string IdProveedor { get; set; }
        [JsonProperty("CINV_NOMID")]
        public string NombreProveedor { get; set; }

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

        [JsonProperty("CODIGOTR")]
        public string IdSucursal { get; set; }
        [JsonProperty("NOM_CENTROCOSTO")]
        public string NomCentroCosto { get; set; }

        [JsonProperty("CINV_FPAGO")]
        public string IdViaje { get; set; }
        [JsonProperty("NOM_VIAJE")]
        public string NomViaje { get; set; }


        [JsonProperty("CINV_COM1")]
        public string Comentario { get; set; }
        [JsonProperty("CINV_STCUMPLI1")]
        public string EstadoJefe { get; set; }
        [JsonProperty("CINV_STCUMPLI2")]
        public string EstadoSupervisor { get; set; }

        [JsonProperty("CINV_BOD")]
        public string IdBodega { get; set; }
        [JsonProperty("NOM_BODEGA")]
        public string NombreBodega { get; set; }

        [JsonProperty("CINV_COM2")]
        public string IdSolicitante { get; set; }
        [JsonProperty("NOM_SOLICITADO")]
        public string NombreSolicitante { get; set; }

        [JsonProperty("CINV_COM3")]
        public string Valor { get; set; }
        [JsonProperty("CINV_COM4")]
        public string ValorIva { get; set; }
        [JsonProperty("NOM_OBRAS")]
        public string NombreObras { get; set; }
        [JsonProperty("DURACION")]
        public decimal? Duracion { get; set; }
        [JsonProperty("CINV_PEDIDO")]
        public int? Pedido { get; set; }

        #region Presupuesto
        [JsonProperty("PRESUPUESTO")]
        public decimal Presupuesto { get; set; }

        [JsonProperty("APROBADO")]
        public decimal Aprobado { get; set; }

        [JsonProperty("PENDIENTE")]
        public decimal Pendiente { get; set; }

        [JsonProperty("SALDO")]
        public decimal Saldo { get; set; }
        #endregion


        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public string Color { get; set; }
        public bool EsModificacion { get; set; }
        public bool EsAprobacion { get; set; }
        [JsonProperty("lst")]
        public List<OrdenDetalleModel> lst { get; set; }
    }
}
