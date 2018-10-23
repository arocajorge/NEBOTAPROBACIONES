using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.general
{
    public class TBCINV_APPCORREOS_INFO
    {
        public string CINV_TDOC { get; set; }
        public int CINV_NUM { get; set; }
        public string CINV_LOGIN { get; set; }
        public string CINV_ST { get; set; }
        public Nullable<System.DateTime> FECHA_ENVIO { get; set; }
        public Nullable<System.DateTime> FECHA_APRO { get; set; }
        public string COMENTARIO { get; set; }
        public string ROL_APRO { get; set; }
    }
}
