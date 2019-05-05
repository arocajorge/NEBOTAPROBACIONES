using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprobacionesApi.Models
{
    public class BonoDetModel
    {
        public string TITULO { get; set; }
        public string DESCRIPCION { get; set; }
        public DateTime? FECHA { get; set; }
        public int? DIAS { get; set; }
        public string COLOR { get; set; }
    }
}