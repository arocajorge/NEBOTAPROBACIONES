using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class PresupuestoModel
    {
        public short EMPRESA { get; set; }
        public string BARCO { get; set; }
        public string VIAJE { get; set; }
        public string DATA_VIAJE { get; set; }
        public decimal PRESUPUESTO { get; set; }
        public Nullable<System.DateTime> FECHA_ULT_MOD { get; set; }
        public decimal APROBADO { get; set; }
        public decimal PENDIENTE { get; set; }
        public decimal SALDO { get; set; }

        #region Campos que no estan en la vista
        public string CINV_TDOC { get; set; }
        #endregion

    }
}