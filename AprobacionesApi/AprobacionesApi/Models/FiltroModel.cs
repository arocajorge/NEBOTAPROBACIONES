using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class FiltroModel
    {
        public string USUARIO { get; set; }
        public string BARCO { get; set; }
        public string VIAJE { get; set; }
        public string SOLICITANTE { get; set; }
        public string BODEGA { get; set; }

        #region Campos que no existen en la tabla
        public string NOMBREBARCO { get; set; }
        public string NOMBREBODEGA { get; set; }
        public string NOMBREVIAJE { get; set; }
        public string NOMBRESOLICITANTE { get; set; }
        #endregion

    }
}