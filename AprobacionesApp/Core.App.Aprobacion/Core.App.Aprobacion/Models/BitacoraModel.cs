using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core.App.Aprobacion.Models
{
    public class BitacoraModel
    {

        [JsonProperty("ID")]
        public short Id { get; set; }

        [JsonProperty("VIAJE")]
        public string Viaje { get; set; }

        [JsonProperty("NOM_VIAJE")]
        public string NomViaje { get; set; }
        
        [JsonProperty("BARCO")]
        public string Barco { get; set; }

        [JsonProperty("NOM_BARCO")]
        public string NomBarco { get; set; }
        
        [JsonProperty("LINEA")]
        public short Linea { get; set; }

        [JsonProperty("DESCRIPCION")]
        public string Descripcion { get; set; }

        [JsonProperty("CONTRATISTA")]
        public string Contratista { get; set; }
        [JsonProperty("CANTIDADLINEAS")]
        public int CantidadLineas { get; set; }
        [JsonProperty("PENTIENTEJEFE")]
        public int PendienteJefe { get; set; }
        [JsonProperty("PENTIENTESUPERVISOR")]
        public int PendienteSupervisor { get; set; }

        [JsonProperty("LOGIN")]
        public string Usuario { get; set; }
        [JsonProperty("ESTADOAPRO")]
        public string Estado { get; set; }

        [JsonProperty("LINEA_STCUMPLI1")]
        public string EstadoJefe { get; set; }
        [JsonProperty("LINEA_STCUMPLI2")]
        public string EstadoSupervisor { get; set; }


        public string Imagen { get; set; }
        public string Color { get; set; }

        public List<BitacoraDetModel> lst { get; set; }
    }
}
