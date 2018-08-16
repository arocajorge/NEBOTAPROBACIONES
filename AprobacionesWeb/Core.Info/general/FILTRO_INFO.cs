using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.general
{
    public class FILTRO_INFO
    {
        public string IdSucursal { get; set; }
        public string IdBodega { get; set; }
        public string IdProveedor { get; set; }        
        public string IdViaje { get; set; }
        public string num_ot { get; set; }
        public string solicitado_por { get; set; }
        public string estado_jefe_bahia { get; set; }
        public string estado_supervisor { get; set; }
        public DateTime Fecha_inicio { get; set; }
        public DateTime Fecha_fin { get; set; }
        public string mensaje_exito { get; set; }
        public string tipo_doc { get; set; }

        public List<ORDEN_TRABAJO_INFO> lst_ot { get; set; }
    }
}
