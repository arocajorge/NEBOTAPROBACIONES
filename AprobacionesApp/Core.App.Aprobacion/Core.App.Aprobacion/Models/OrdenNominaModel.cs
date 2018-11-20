namespace Core.App.Aprobacion.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    public class OrdenNominaModel
    {
        [JsonProperty("CINV_TDOC")]
        public string TipoDocumento { get; set; }
        [JsonProperty("CINV_NUM")]
        public int NumeroOrden { get; set; }
        [JsonProperty("CINV_FECING")]
        public DateTime Fecha { get; set; }

        [JsonProperty("CINV_STREFER")]
        public string EstadoReferido { get; set; }
        [JsonProperty("CINV_COMREFER")]
        public string ComentarioReferido { get; set; }

        [JsonProperty("CINV_ST")]
        public string Estado { get; set; }
        [JsonProperty("CINV_MOTIVOANULA")]
        public string MotivoAnulacion { get; set; }

        [JsonProperty("CINV_STPOLI")]
        public string EstadoPoligrafo { get; set; }
        [JsonProperty("CINV_COMPOLI")]
        public string ComentarioPoligrafo { get; set; }
        public string ImagenPoligrafo { get; set; }

        [JsonProperty("CINV_STPSICO")]
        public string EstadoPsicologo { get; set; }
        [JsonProperty("CINV_COMPSICO")]
        public string ComentarioPsicologo { get; set; }
        public string ImagenPsicologo { get; set; }
        
        [JsonProperty("CINV_STANTE")]
        public string EstadoAntecedentes { get; set; }
        [JsonProperty("CINV_COMANTE")]
        public string ComentarioAntecedentes { get; set; }
        [JsonProperty("CINV_COMANTE2")]
        public string ComentarioAntecedentes2 { get; set; }
        public string ImagenAntecedentes { get; set; }

        [JsonProperty("CINV_STPERFIL")]
        public string EstadoPerfil { get; set; }
        [JsonProperty("CINV_COMPERFIL")]
        public string ComentarioPerfil { get; set; }
        public string ImagenPerfil { get; set; }

        [JsonProperty("NOM_SOLICITADO")]
        public string NombreSolicitado { get; set; }
        [JsonProperty("CED_SOLICITADO")]
        public string CedulaSolicitado { get; set; }

        [JsonProperty("NOM_CENTROCOSTO")]
        public string NombreCentroCosto { get; set; }
        [JsonProperty("NOM_CARGO")]
        public string NombreCargo { get; set; }

        [JsonProperty("ListaDetalle")]
        public List<OrdenNominaDetalleModel> LstDet { get; set; }
        public string Imagen { get; set; }
        public string Color { get; set; }
        public string Titulo { get; set; }
    }
}
