using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class OrdenNominaModel
    {
        public string CED_SOLICITADO { get; set; }
        public string CINV_COMANTE { get; set; }
        public string CINV_COMANTE2 { get; set; }
        public string CINV_COMPERFIL { get; set; }
        public string CINV_COMPOLI { get; set; }
        public string CINV_COMPSICO { get; set; }
        public string CINV_COMREFER { get; set; }
        public DateTime CINV_FECING { get; set; }
        public string CINV_MOTIVOANULA { get; set; }
        public int CINV_NUM { get; set; }
        public string CINV_ST { get; set; }
        public string CINV_STANTE { get; set; }
        public string CINV_STPERFIL { get; set; }
        public string CINV_STPOLI { get; set; }
        public string CINV_STPSICO { get; set; }
        public string CINV_STREFER { get; set; }
        public string CINV_TDOC { get; set; }
        public string NOM_CENTROCOSTO { get; set; }
        public string NOM_SOLICITADO { get; set; }
        public string CINV_LOGIN { get; set; }
        public List<OrdenNominaDetalleModel> ListaDetalle { get; set; }
        public string NOM_CARGO { get; set; }
    }
}