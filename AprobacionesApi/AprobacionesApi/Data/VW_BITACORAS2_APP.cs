//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AprobacionesApi.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class VW_BITACORAS2_APP
    {
        public int ID { get; set; }
        public short LINEA { get; set; }
        public short LINEA_DETALLE { get; set; }
        public string NUMERO_ORDEN { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        public Nullable<short> CUMPLIMIENTO { get; set; }
        public Nullable<short> EMPRESA { get; set; }
        public string TIPO { get; set; }
        public Nullable<decimal> CINV_NUM { get; set; }
        public string NOMPROVEEDOR { get; set; }
        public string CINV_COM1 { get; set; }
        public string ESTADO { get; set; }
        public string COLOR_ESTADO { get; set; }
    }
}