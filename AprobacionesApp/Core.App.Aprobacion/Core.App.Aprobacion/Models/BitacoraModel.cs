using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core.App.Aprobacion.Models
{
    public class BitacoraModel
    {
        [JsonProperty("EMPRESA")]
        public long Empresa { get; set; }

        [JsonProperty("ID")]
        public short Id { get; set; }

        [JsonProperty("VIAJE")]
        public string Viaje { get; set; }

        [JsonProperty("NOM_VIAJE")]
        public string NomViaje { get; set; }

        [JsonProperty("FECINGRESO")]
        public DateTime Fecingreso { get; set; }

        [JsonProperty("BARCO")]
        public string Barco { get; set; }

        [JsonProperty("NOM_BARCO")]
        public string NomBarco { get; set; }

        [JsonProperty("LOGIN")]
        public string Login { get; set; }

        [JsonProperty("ESTADO")]
        public string Estado { get; set; }

        [JsonProperty("LINEA")]
        public short Linea { get; set; }

        [JsonProperty("DESCRIPCION")]
        public string Descripcion { get; set; }

        [JsonProperty("CONTRATISTA")]
        public string Contratista { get; set; }

        [JsonProperty("LINEA_DETALLE")]
        public short? LineaDetalle { get; set; }

        [JsonProperty("NUMERO_ORDEN")]
        public int? NumeroOrden { get; set; }

        [JsonProperty("VALOR")]
        public double? Valor { get; set; }

        [JsonProperty("STCUMPLI1")]
        public string EstadoJefe { get; set; }

        [JsonProperty("STCUMPLI2")]
        public string EstadoSupervisor { get; set; }

        [JsonProperty("STORDEN")]
        public string Storden { get; set; }

        public List<BitacoraDetModel> lst { get; set; }
    }
}
