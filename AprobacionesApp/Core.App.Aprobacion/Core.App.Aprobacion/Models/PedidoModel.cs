using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.App.Aprobacion.Models
{
    public class PedidoModel
    {
        [JsonProperty("PED_ID")]
        public long ID { get; set; }
        [JsonProperty("PED_FECHA")]
        public DateTime Fecha { get; set; }
        [JsonProperty("PED_BOD")]
        public string IdBodega { get; set; }
        [JsonProperty("PED_VIAJE")]
        public string IdViaje { get; set; }
        [JsonProperty("PED_SUC")]
        public string IdSucursal { get; set; }
        [JsonProperty("PED_SOL")]
        public string IdSolicitante { get; set; }
        [JsonProperty("PED_OBS")]
        public string Observacion { get; set; }
        [JsonProperty("PED_ST")]
        public string Estado { get; set; }
        [JsonProperty("NOM_BODEGA")]
        public string NombreBodega { get; set; }
        [JsonProperty("NOM_SUCURSAL")]
        public string NombreSucursal { get; set; }
        [JsonProperty("NOM_VIAJE")]
        public string NombreViaje { get; set; }
        [JsonProperty("NOM_EMPLEADO")]
        public string NombreEmpleado { get; set; }
        [JsonProperty("PED_LOGIN")]
        public string Usuario { get; set; }
        [JsonProperty("PED_COM")]
        public string ComentarioAprobacion { get; set; }
        [JsonProperty("ESTADO")]
        public string EstadoAprobacion { get; set; }
        [JsonProperty("COLOR")]
        public string Color { get; set; }


        [JsonProperty("LstDet")]
        public List<PedidoDetModel> LstDet { get; set; }

        #region Campos que no existen en el servicio
        public string Titulo { get; internal set; }
        #endregion

    }
}
