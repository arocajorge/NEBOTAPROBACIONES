using System;

namespace Core.Info.general
{
    public class BITACORAS_INFO
    {
        public Nullable<short> EMPRESA { get; set; }
        public int ID { get; set; }
        public string VIAJE { get; set; }
        public string NOM_VIAJE { get; set; }
        public Nullable<System.DateTime> FECINGRESO { get; set; }
        public string BARCO { get; set; }
        public string NOM_BARCO { get; set; }
        public string LOGIN { get; set; }
        public string ESTADO { get; set; }
        public short LINEA { get; set; }
        public string DESCRIPCION { get; set; }
        public string CONTRATISTA { get; set; }
        public Nullable<short> LINEA_DETALLE { get; set; }
        public string NUMERO_ORDEN { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        public string STCUMPLI1 { get; set; }
        public string STCUMPLI2 { get; set; }
        public string STORDEN { get; set; }

        #region Campos que no existen en la tabla
        public int CANTLINEAS { get; set; }
        #endregion
    }
}
