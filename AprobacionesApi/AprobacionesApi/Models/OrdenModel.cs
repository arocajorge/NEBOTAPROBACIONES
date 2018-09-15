using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class OrdenModel
    {
        public string CINV_TDOC { get; set; }
        public int CINV_NUM { get; set; }
        public string CINV_NOMID { get; set; }
        public string NOM_SOLICITADO { get; set; }
        public Decimal VALOR_OT { get; set; }
        public DateTime CINV_FECING { get; set; }
        public string CINV_ST { get; set; }
        public string CINV_COM1 { get;   set; }
        public string CODIGO1 { get;   set; }
        public string CINV_FPAGO { get;   set; }
        public string CODIGOTR { get;   set; }
        public string CINV_BOD { get;   set; }
        public string CINV_STCUMPLI2 { get;   set; }
        public string CINV_STCUMPLI1 { get;   set; }
        public string CINV_COM3 { get;   set; }
        public string CINV_COM4 { get;   set; }
        public decimal? VALOR_OC { get;   set; }
        public List<OrdenDetalleModel> lst { get; set; }
        public string CINV_MOTIVOANULA { get; set; }
        public string CINV_LOGIN { get; set; }
    }
}