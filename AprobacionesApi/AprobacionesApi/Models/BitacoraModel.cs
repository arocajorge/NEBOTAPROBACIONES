using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class BitacoraModel
    {
        public Nullable<short> EMPRESA { get; set; }
        public int ID { get; set; }
        public string VIAJE { get; set; }
        public string NOM_VIAJE { get; set; }
        public Nullable<System.DateTime> FECINGRESO { get; set; }
        public string BARCO { get; set; }
        public string NOM_BARCO { get; set; }
        public string LOGIN { get; set; }
        public string ESTADO { get; set; }
        public short LINEA { get; set; }
        public string DESCRIPCION { get; set; }
        public string CONTRATISTA { get; set; }
        public Nullable<short> LINEA_DETALLE { get; set; }
        public string NUMERO_ORDEN { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        public string STCUMPLI1 { get; set; }
        public string STCUMPLI2 { get; set; }
        public string STORDEN { get; set; }
        public int CANTIDADLINEAS { get; set; }
        public int PENTIENTEJEFE { get; set; }
        public int PENTIENTESUPERVISOR { get; set; }
        public bool CUMPLIJEFE { get; set; }
        public bool CUMPLISUP { get; set; }
    }
}