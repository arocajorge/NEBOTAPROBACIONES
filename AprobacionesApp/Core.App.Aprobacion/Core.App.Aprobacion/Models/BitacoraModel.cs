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

        public string ImagenJefe { get; set; }
        public string ImagenSupervisor { get; set; }

        [JsonProperty("LINEA_FECCUMPLI1")]
        public DateTime? FechaAproJefe { get; set; }
        [JsonProperty("LINEA_FECCUMPLI2")]
        public DateTime? FechaAproSupervisor { get; set; }

        public string Imagen { get; set; }
        public string Color { get; set; }

        [JsonProperty("STCUMPLI_JEFE")]
        public Nullable<decimal> CantidadJefe { get; set; }
        [JsonProperty("STCUMPLI_SUP")]
        public Nullable<decimal> CantidadSupervisor { get; set; }
        [JsonProperty("STCUMPLI_APRO")]
        public Nullable<decimal> CantidadApro { get; set; }
        [JsonProperty("STCUMPLI_TOTAL")]
        public Nullable<decimal> CantidadTotal { get; set; }

        [JsonProperty("NUMERO_ORDEN")]
        public string NumeroOrden { get; set; }
        [JsonProperty("ST_EMPLEADO")]
        public Nullable<short> EsEmpleado { get; set; }
        [JsonProperty("FECHA_ZARPE")]
        public Nullable<DateTime> FechaZarpe { get; set; }
        [JsonProperty("FECHA_ARRIBO")]
        public Nullable<DateTime> FechaArribo { get; set; }
        [JsonProperty("FECHA_ZARPE_REAL")]
        public Nullable<DateTime> FechaZarpeReal { get; set; }
        [JsonProperty("DURACION")]
        public decimal? Duracion { get; set; }
        [JsonProperty("FECHAOT")]
        public Nullable<DateTime> FechaOT { get; set; }
        [JsonProperty("TIPO")]
        public string Tipo { get; set; }
        [JsonProperty("MENSAJE_ANULADOS")]
        public string MensajeAnulados { get; set; }

        public double DiferenciaDiasZA { get; set; }
        public double DiferenciaDiasZRA { get; set; }
        public double DuracionReal { get; set; }
        public string Titulo { get; set; }


        public List<BitacoraDetModel> lst { get; set; }
    }
}
