//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class CAB_BITACORA
    {
        public int ID { get; set; }
        public string VIAJE { get; set; }
        public string DATA_VIAJE { get; set; }
        public Nullable<System.DateTime> FECINGRESO { get; set; }
        public string BARCO { get; set; }
        public string LOGIN { get; set; }
        public string ESTADO { get; set; }
        public Nullable<short> EMPRESA { get; set; }
        public Nullable<System.DateTime> FECHA_ARRIBO { get; set; }
        public Nullable<System.DateTime> FECHA_ZARPE { get; set; }
        public Nullable<System.DateTime> FECHA_ZARPE_REAL { get; set; }
    }
}
