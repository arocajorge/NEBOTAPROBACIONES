using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class BitacoraDetModel
    {
        public int ID { get; set; }
        public short LINEA { get; set; }
        public short LINEA_DETALLE { get; set; }
        public string NUMERO_ORDEN { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        public Nullable<short> CUMPLIMIENTO { get; set; }
        public Nullable<short> EMPRESA { get; set; }
        public string NOMPROVEEDOR { get; set; }
        public string DETALLEOT { get; set; }
        public bool ELIMINAR { get; set; }
        public int CINV_NUM { get; set; }
        public string CINV_COM1 { get; set; }
    }
}