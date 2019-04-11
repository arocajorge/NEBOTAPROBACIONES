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
        public Nullable<System.DateTime> LINEA_FECCUMPLI1 { get; set; }
        public string LINEA_STCUMPLI1 { get; set; }
        public string LINEA_LOGINCUMPLI1 { get; set; }
        public Nullable<System.DateTime> LINEA_FECCUMPLI2 { get; set; }
        public string LINEA_STCUMPLI2 { get; set; }
        public string LINEA_LOGINCUMPLI2 { get; set; }
        public string ROL_APRO { get; set; }
        public string ESTADOAPRO { get; set; }
        public Nullable<short> ST_EMPLEADO { get; set; }
        public Nullable<System.DateTime> FECHA_ARRIBO { get; set; }
        public Nullable<System.DateTime> FECHA_ZARPE { get; set; }
        public Nullable<System.DateTime> FECHA_ZARPE_REAL { get; set; }
        public Nullable<System.DateTime> FECHAOT { get; set; }
        public string TIPO { get; set; }

        #region Cumplimientos
        public Nullable<decimal> STCUMPLI_JEFE { get; set; }
        public Nullable<decimal> STCUMPLI_SUP { get; set; }
        public Nullable<decimal> STCUMPLI_TOTAL { get; set; }
        public Nullable<decimal> STCUMPLI_APRO { get; set; }
        public decimal? DURACION { get; set; }
        public int ANULADOS { get; set; }
        public string MENSAJE_ANULADOS { get; set; }
        public int APROBADOS { get; set; }
        #endregion

    }
}