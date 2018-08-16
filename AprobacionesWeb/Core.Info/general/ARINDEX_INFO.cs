using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.general
{
    public class ARINDEX_INFO
    {
        public string DATA { get; set; }
        public string CODIGO { get; set; }
        public string NOMBRE { get; set; }
        public string ESTADO { get; set; }
        public string COMENTARIO { get; set; }
        public Nullable<decimal> CONTROL_FACTURAS { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONOS { get; set; }
        public Nullable<decimal> TIPO_COMISION { get; set; }
        public string PADRE { get; set; }
        public Nullable<System.DateTime> FECHA_INGRESO { get; set; }
        public Nullable<System.DateTime> FECHA_SALIDA { get; set; }
        public Nullable<decimal> CUENTA { get; set; }
        public string CEDULA { get; set; }
        public string REFERENCIA { get; set; }
        public short EMPRESA { get; set; }
        public string COD_UND_CAE { get; set; }
    }
}
