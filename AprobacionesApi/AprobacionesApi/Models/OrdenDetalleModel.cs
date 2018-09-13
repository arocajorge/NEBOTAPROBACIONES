using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class OrdenDetalleModel
    {
        public string UNIDAD { get; set; }
        public decimal DINV_CANT { get; set; }
        public string DINV_DETALLEDSCTO { get; set; }
        public decimal DINV_COS { get; set; }
        public decimal DINV_PRCT_DSC { get; set; }
        public decimal DINV_VTA { get; set; }
        public decimal TOTAL { get; set; }
        public int DINV_LINEA { get; set; }
        public string CINV_COM4 { get; set; }
        public string CINV_COM3 { get; set; }
        public decimal DINV_IVA { get; set; }
        public string CINV_TDOC { get; set; }
    }
}